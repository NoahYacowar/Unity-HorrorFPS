using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    //Variables used in determining angular measure for menu camera
    float angle = 0;
    float updateFrameAngle = 0.06f;
    void Update()
    {
        //Rotating the camera is circular motion around central y-axis. Overall angle maintained
        transform.localRotation *= Quaternion.Euler(0, updateFrameAngle, 0);
        angle += updateFrameAngle;
    }
}
