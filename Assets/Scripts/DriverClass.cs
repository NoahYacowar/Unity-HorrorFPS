using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class DriverClass : MonoBehaviour
{
    //Defining length and width of grid
    int roomsLength = 100;
    int roomsWidth = 100;

    //Various GameObjects and their required data (for enemy and player)
    private Player player;
    private Enemy enemy;
    public Transform playerTransform;
    public Transform enemyTransform;
    public int playerHealth;
    public int enemyHealth;
    public int enemyHealthIncrement;

    [Space]

    //Objects used in random room generation
    private RoomManager roomManager;
    public GameObject[] roomPrefabs;

    //Variables which delay the grid obstacles being redrawn
    private bool shouldRedrawGrid = false;
    private bool holdNew = true;
    private float delayGridTime = 0.3f;
    private float delayGridCount;
    private float delayGridHold = 0;

    //Various player stats being initiated
    private int roundNum = 1;
    private int playerPoints;
    private int successfullShots = 0;

    //Various UI elements
    public PauseMenu pauseMenu;
    public EditText completeLevelUI;
    public GameOverScreen gameOverScreen;

    //Constants storing index values of user stats
    const int MAX_ROUND_INDEX = 0;
    const int MAX_POINTS_INDEX = 1;
    const int TOTAL_HITS_INDEX = 2;

    // Start is called before the first frame update
    void Start()
    {
        //Generates grid of rooms
        roomManager = new RoomManager(roomPrefabs);
        roomManager.RegenerateRooms();
        FindObjectOfType<Grid>().CreateGrid();

        //Instantiating player
        player = new Player(playerTransform, playerHealth);
        player.SetPlayerPosition(GetNewGridPosition());

        //Instantiating Enemy
        enemy = new Enemy(enemyTransform, enemyHealth);
        enemy.SetPlayerPosition(GetNewGridPosition());
    }

    //Pre:
    //Post:
    //Desc: Returns position of a randomly selected node within grid
    Vector3 GetNewGridPosition()
    {
        return roomManager.GetNewGridPosition();
    }

    // Update is called once per frame
    void Update()
    {
        DelayGridRedraw();
        Debug.Log(enemy.GetHealth());
        if (enemy.GetHealth() <= 0)
        {
            NextRound();
        }

        if(Input.GetKeyDown(KeyCode.B)) NextRound();

        //Calculates distance between player and enemy. Should it be les than 4, player will be considered hit
        if (Mathf.Abs(Vector3.Distance(playerTransform.position, enemyTransform.position)) <= 4) DamagePlayer();

        //Should the player press escape key, menu is paused
        if (Input.GetKeyDown(KeyCode.Escape)) PauseGame();
    }

    //Pre:
    //post:
    //Desc: Delays the grid being redrawn by specified time
    private void DelayGridRedraw()
    {
        //Assigning timer variables
        delayGridCount = Time.time;
        if (holdNew) delayGridHold = delayGridCount;
        holdNew = false;

        //Determines whether long enough duration has elapsed
        if (delayGridCount >= delayGridHold + delayGridTime)
        {
            FindObjectOfType<Grid>().CreateGrid();
            holdNew = true;
        }
    }

    //Pre:
    //post:
    //Desc: method is called whenever a hit has been detected on the enemy
    public void EnemyShot(int damage)
    {
        //Setting adjustements for enemy being shot
        playerPoints += 10;
        enemy.IncrementHealth(damage);
        enemy.SetPlayerPosition(GetNewGridPosition());
        successfullShots++;
    }

    //Pre:
    //Post:
    //Player: If proximity has been detected betwen player and enemy, the following handles the logic
    private void DamagePlayer()
    {
        //Setting adjustements for enemy being hit
        player.IncrementHealth(-1);
        Debug.Log("Player Damaged: " + player.GetHealth());
        enemy.SetPlayerPosition(GetNewGridPosition());
        if (player.GetHealth() == 0) EndGame();
    }

    //Pre:
    //Post:
    //Desc: Removes all previous prefabs found on room grid
    private void RemoveGridPrefabs()
    {
        int gridLength = roomManager.GetGridLength();

        for(int x = 0; x < gridLength; x++)
        {
            for (int y = 0; y < gridLength; y++) if(roomManager.GetRoomNode(x, y) != null) Destroy(roomManager.GetRoomNode(x, y));
        }
    }

    //Pre:
    //Post:
    //Desc: Makes necessary adjustemnts to create next round
    private void NextRound()
    {
        roundNum++;
        completeLevelUI.DisplayRound(roundNum);
        FindObjectOfType<Unit>().SpeedIncrement(0.01f);
        enemy.SetNewMaxHealth(enemy.GetMaxHealth() + enemyHealthIncrement);
        RemoveGridPrefabs();
        roomManager.RegenerateRooms();
        player.SetPlayerPosition(GetNewGridPosition());
        enemy.SetPlayerPosition(GetNewGridPosition());
        shouldRedrawGrid = true;
    }

    //Pre:
    //post:
    //Desc: Pauses the game
    private void PauseGame()
    {
        pauseMenu.PauseGame();
    }

    private void EndGame()
    {
        SaveUserData("Assets/UserData.txt");
        gameOverScreen.Setup(playerPoints);
    }

    private void SaveUserData(string fileName)
    {
        string[] updatedUserInfo = FileEditor.ReadFile(fileName);

        if (Convert.ToInt32(updatedUserInfo[MAX_ROUND_INDEX]) < roundNum) updatedUserInfo[MAX_ROUND_INDEX] = roundNum.ToString();
        if (Convert.ToInt32(updatedUserInfo[MAX_POINTS_INDEX]) < playerPoints) updatedUserInfo[MAX_POINTS_INDEX] = playerPoints.ToString();
        updatedUserInfo[TOTAL_HITS_INDEX] = (Convert.ToInt32(updatedUserInfo[TOTAL_HITS_INDEX]) + successfullShots).ToString();

        FileEditor.WriteFile(fileName, updatedUserInfo);
    }
}
