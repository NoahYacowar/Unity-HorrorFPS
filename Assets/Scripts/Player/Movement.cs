using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour 
{
    //Creating variables for player input
    private Vector3 playerMovementInput;
    private Vector2 playerMouseInput;

    //Variables referencing feet collision (both a layer and feet object)
    [SerializeField] private LayerMask floorMask;
    [SerializeField] private Transform feetTransform;

    //Variables storing data and references on/to weapon and camera data/objects
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform weapon;
    [SerializeField] private Transform weaponParent;
    private Vector3 weaponParentOrigin;
    private Vector3 targetWeaponBobPosition;
    private float baseFOV;
    private float sprintFOVModifier = 1.5f;
    public int maxAngle;

    //Referencing player's rigidbody
    [SerializeField] private Rigidbody playerBody;

    [Space]

    //Referencing various movement alterations
    [SerializeField] private float speed;
    private float adjustedSpeed;
    [SerializeField] private float sprintModifier;
    [SerializeField] private float sensitivity;
    [SerializeField] private float jumpForce;

    //Initializing counter variables for movement and idlecy
    private float movementCounter;
    private float idleCounter;

    void Start ()
    {
        //Referencing the original camera FOV and weapon's original position
        baseFOV = playerCamera.fieldOfView;
        weaponParentOrigin = weaponParent.localPosition;
    }


	// Update is called once per frame
	void FixedUpdate () {
        //Collecing user input from mouse and keys
        playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        playerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
      
        //Determines and stores whether player is sprinting
        bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool isSprinting = sprint && playerMovementInput.z > 0;
        adjustedSpeed = speed;

        //Appropriately testing and adjusting the player's speed with wether input meets criteria for sprint
        if (isSprinting)
        {
            //Altering player speed and camera FOV, mathf.Lerp used for smooth transitioning
            adjustedSpeed *= sprintModifier;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, baseFOV * sprintFOVModifier, Time.deltaTime * 8f);
        }
        else playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, baseFOV, Time.deltaTime * 8f); ;

        //Calling subprogram to move player
        MovePlayer();

        //Head Bob statements determinent on factors relating to player state (ie. sprint, stagnancy)
        if (playerMovementInput.x == 0 && playerMovementInput.z == 0) 
        { 
            //Calls upon head bob subprogram and is used in determining the weapon's parent's positioning
            HeadBob(idleCounter, 0.009f, 0.009f); 
            idleCounter += Time.deltaTime;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 2f);
        }
        else if(!isSprinting)
        {
            //Calls upon head bob subprogram and is used in determining the weapon's parent's positioning
            HeadBob(movementCounter, 0.02f, 0.02f); 
            movementCounter += Time.deltaTime * 3f;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 6f);
        }
        else
        {
            //Calls upon head bob subprogram and is used in determining the weapon's parent's positioning
            HeadBob(movementCounter, 0.08f, 0.05f);
            movementCounter += Time.deltaTime * 5f;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 10f);
        }
        
	}

    //Pre:
    //Post:
    //Desc: Moves player based on preevaluated circumstance, determines whether jump has been requested
    private void MovePlayer()
    {
        //Moving the player by predetermined velocity. Y isolated so as to not account for sprinting 
        Vector3 moveVector = transform.TransformDirection(playerMovementInput) * adjustedSpeed;
        playerBody.velocity = new Vector3(moveVector.x, playerBody.velocity.y, moveVector.z);
        
        //Determines whether space bar has been used 
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //Determines whether the player's feet are in contact with floor layer
            if (Physics.CheckSphere(feetTransform.position, 0.1f, floorMask))
            {
                //Upward force applied onto player
                playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    void HeadBob(float x, float xIntensity, float yIntensity)
    {
        targetWeaponBobPosition = weaponParentOrigin + new Vector3(Mathf.Cos(x) * xIntensity, Mathf.Sin(x * 2) * yIntensity, 0);
    }
}
