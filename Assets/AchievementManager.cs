using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementManager : MonoBehaviour
{
    //Text data for editing
    public TMP_Text[] textComponents;
    private string[] intro = new string[] { "Max Round Achieved: ", "Highest Score: ", "Successfull Hits: " };

    //Pre: n/a
    //Post: n/a
    //Desc: Asigning user data to correct text GameObject
    private void Awake()
    {
        //Reads in user data, storing into string array
        string[] userData = FileEditor.ReadFile("Assets/UserData.txt");

        //Loops through number of indexes
        for(int i = 0; i < textComponents.Length; i++)
        {
            textComponents[i].text = intro[i] + userData[i];
        }
    }

}
