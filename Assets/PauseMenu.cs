using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{

    public GameObject player;
    public GameObject enemy;
    public GameObject mapCam;
    public GameObject[] screens;

    //Pre:
    //Post:
    //Desc: handles all gameObjects for pausing
    public void PauseGame()
    {
        //Referencing parent, setting to active
        gameObject.SetActive(true);

        //Altering GameObjects for pausing game
        for (int i = 0; i < screens.Length; i++) screens[i].SetActive(false);
        enemy.SetActive(false);
        player.SetActive(false);
        mapCam.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    //Pre:
    //Post: 
    //Desc: handles all necessary game objects to appropriately resume game
    public void ResumeButton()
    {
        //Altering gameobjects appropriately
        gameObject.SetActive(false);
        enemy.SetActive(true);
        player.SetActive(true);
        mapCam.SetActive(false);

        //Unlocking the cursor for selection
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //Pre: n/a
    //Post: n/a
    //Desc: Reloads menu scene
    public void ExitButton()
    {
        SceneManager.LoadScene("Menu");
    }
}
