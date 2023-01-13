using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Pre: 
    //Post:
    //Desc: Handles appropriate change when starting game
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Pre: 
    //Post:
    //Desc: Handles appropriate change when leaving game
    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

}
