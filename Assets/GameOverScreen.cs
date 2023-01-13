using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    //Defininig Objects for use towards instantiating an ending screen 
    public Text pointsText;
    public GameObject player;
    public GameObject enemyCamera;
    public GameObject[] screens;

    //Pre: the player's score stored in an integer
    //Post: n/a
    //Desc: Appropriately handles various world objects and user devices
    public void Setup(int score)
    {
        gameObject.SetActive(true);
        pointsText.text = score.ToString() + " POINTS";

        for (int i = 0; i < screens.Length; i++) screens[i].SetActive(false);
        enemyCamera.SetActive(true);
        player.SetActive(false); 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //Pre: n/a
    //Post: n/a
    //Desc: Reloads current scene
    public void RestartButton()
    {
        SceneManager.LoadScene("Main");
    }

    //Pre: n/a
    //Post: n/a
    //Desc: Reloads menu scene
    public void ExitButton()
    {
        SceneManager.LoadScene("Menu");
    }
}
