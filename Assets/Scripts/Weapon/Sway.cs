using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    //Variables encasing various sway determinents
    public float intensity;
    public float snappiness;

    //Quaternion storing the original rotational orientation
    private Quaternion originRotation;

    private void Start()
    {
        //Setting the reference origin rotation equal to the current rotation of parent object (weapon)
        originRotation = transform.localRotation;
    }

    private void Update()
    {
        //Calling update sway method to have continuous checking
        UpdateSway();
    }

    //Pre:
    //post:
    //Desc: Fluid movement of weapon player is currently holding
    private void UpdateSway()
    {
        //Having reference to mouse input values (on vertical and horizontal axis)
        float xMouse = Input.GetAxis("Mouse X");
        float yMouse = Input.GetAxis("Mouse Y");

        //Calculate target rotation
        Quaternion adjustX = Quaternion.AngleAxis(-intensity * xMouse, Vector3.up);
        Quaternion adjustY = Quaternion.AngleAxis(intensity * yMouse, Vector3.right);
        Quaternion targetRotation = originRotation * adjustX * adjustY;

        //Rotate towards target rotation
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * snappiness);
    }
}
