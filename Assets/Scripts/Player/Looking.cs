using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looking : MonoBehaviour
{
    //Variables for player and its substituents
    public static bool isCursorLock = true;
    public Transform player;
    public Transform cams;
    public Transform weapon;

    //Various editorial variables for camera
    public float xSensitivity;
    public float ySensitivity;
    public float maxAngle;

    //Reference to player camera's center 
    private Quaternion camCenter;

    // Start is called before the first frame update
    void Start()
    {
        camCenter = cams.localRotation; //set rotation origin for cameras to camCenter
    }

    // Update is called once per frame
    void Update()
    {
        //Updating X & Y rotation, as well as user's cursor status
        SetY();
        SetX();
        UpdateCursorLock();
    }

    //Pre: n/a
    //Post: n/a
    //Desc: Sets the rotational value on the verticle axis
    void SetY()
    {
        //Getting mouse input on y-axis (applying smoothening and sensitivity)
        float input = Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime;
        Quaternion adjust = Quaternion.AngleAxis(input, -Vector3.right);
        Quaternion delta = cams.localRotation * adjust;

        //Disregards change in verticle rotation should it exceed threshold
        if (Quaternion.Angle(camCenter, delta) < maxAngle) cams.localRotation = delta;
        weapon.rotation = cams.rotation;
    }

    //Pre: n/a
    //Post: n/a
    //Desc: Sets the rotational value on the horizontal axis
    void SetX()
    {
        float input = Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime;
        Quaternion adjust = Quaternion.AngleAxis(input, Vector3.up);
        Quaternion delta = player.localRotation * adjust;
        player.localRotation = delta;
    }

    //Pre: n/a
    //Post: n/a
    //Desc: Updates the cursor's status
    void UpdateCursorLock()
    {
        //Determines whether the cursor is currently locked
        if (isCursorLock)
        {
            //Locks cursor, determines whether change in state has been requested
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if(Input.GetKeyDown(KeyCode.Backspace)) isCursorLock = false;
        }
        else
        {
            //Unlocks cursor, determines whether change in state has been requested
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (Input.GetKeyDown(KeyCode.Mouse0)) isCursorLock = true;

        }
    }
}
