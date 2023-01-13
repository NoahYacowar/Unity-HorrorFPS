using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EditText : MonoBehaviour
{
    //Storing reference to UI text object
    public Text round;

    //Pre: Player's score stored as an integer
    //Post: n/a
    //Desc: Sets UI to display user's score
    public void DisplayRound(int score)
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        round.text = "ROUND " + score.ToString();
    }
}
