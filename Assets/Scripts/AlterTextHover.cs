using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class AlterTextHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Storing reference to textmeshpro object
    private TMP_Text text;

    void Awake()
    {
        //Storing reference to text object
         text = GetComponent<TMP_Text>();
    }

    //Pre: 
    //Post:
    //Desc: Determines whether cursor is hovering over parent object
    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = Color.red; //Changes Color to red when hovering
    }

    //Pre: 
    //Post:
    //Desc: Determines whether cursor isn't hovering over parent object
    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = Color.white; //Changes color to white when not hovering
    }
}

