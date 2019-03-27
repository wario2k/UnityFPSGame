using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    private Transform firstPerson_View;
    private Transform firstPerson_Camera;

private Vector3 firstPerson_View_Rotation = Vector3.zero;

    public float walkSpeed = 6.75f;
    public float runSpeed = 10f;
    public float crouchSpeed = 4f;
    public float jumpSpeed = 8f;
    public float gravity = 20f;

    private float speed;

    private bool is_Moving, is_Grounded, is_Crouching;

    private float inputX, inputY;
    private float inputX_Set, inputY_Set;
    private float inputModifyFactor;

    private bool limitDiagonalSpeed = true;

    private float antiBumpFactor = 0.75f;

    private CharacterController charController;
    private Vector3 moveDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        //since player will be in an upright position when starting so normal walk speed is applied
        speed = walkSpeed;

        //getting references to all the views when initializing

        //firstPersonView goes into the FPS Player model and then finds the camera view 
        //that was defined inside of the fps player prefab and attaches itself accordingly

        firstPerson_View = transform.Find("FPS View").transform;

        charController = GetComponent<CharacterController>();
        is_Moving = false;
    }


    // Update is called once per frame
    void Update()
    {
        playerMovement();
    }



    /// <summary>
    /// This function determines what direction the player wants to move in based on 
    /// the allowed set of key presses W,A,S,D 
    /// </summary>
    void playerMovement()
    {
        //if w or s pressed the player has decided to move forward if W and backward if S 
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            //move in forward direction
            if (Input.GetKey(KeyCode.W))
            {
                inputY_Set = 1f;
            }
            //move backwards
            else
            {
                inputY_Set = -1f;
            }
        }
        //forward/backward motion has not been specifed 
        else
        {
            inputY_Set = 0;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            //to determine direction
            if (Input.GetKey(KeyCode.A))
            {
                inputX_Set = -1f;
            }
            else
            {
                inputX_Set = 1f;
            }
        }
        else
        {
            inputX_Set = 0f;
        }

        //interpolate turning/movement so that it seems less rigid 
        //changes value linnearly from inputY to inputY_set which is the value recieved and does it in time delta as specified. 
        inputY = Mathf.Lerp(inputY, inputY_Set, Time.deltaTime * 19f);
        inputX = Mathf.Lerp(inputX, inputX_Set, Time.deltaTime * 19f);

        //checks if movement in x axis and y axis are not 0 and diagonal Speed limiter flag is true
        //the input modify factor is to make sure that diagonal speeds do not exceed the max speed specified and 
        //no weird behavior is experienced 
        inputModifyFactor = Mathf.Lerp(inputModifyFactor,
                                            (inputY_Set != 0 && inputX_Set != 0 && limitDiagonalSpeed) ? 0.75f : 1.0f,
                                            Time.deltaTime * 19f
                                            );

        firstPerson_View_Rotation = Vector3.Lerp(firstPerson_View_Rotation,
                                                Vector3.zero,
                                                    Time.deltaTime * 5f);
        //this is taking the local reference to the camera and the fps character parent object instead of taking the local scene object 
        //to make sure we are rotating based on the players view and not the worlds' view

        firstPerson_View.localEulerAngles = firstPerson_View_Rotation;

        // if the player is on the ground 
        if (is_Grounded)
        {
            moveDirection = new Vector3(inputX * inputModifyFactor, // x-axis movement
                                                       -antiBumpFactor, //for smoother character animation if we bump into objects
                                                           inputY * inputModifyFactor); // y-axis movement

            //to make sure we're using local co-ordinates and not world co-ordinates 
            moveDirection = transform.TransformDirection(moveDirection) * speed;
        }
        //applying gravity to the game object 
        //since gravity is supposed to be pulling you to the ground the negative value is assigned to make sure the 
        //fps character model drops when jumping and doesn't float around the world.
        moveDirection.y -= gravity * Time.deltaTime;

        //checking if the player model is grounded by calling CollisionFlags.Below 
        is_Grounded = (charController.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;

        //the magnitude is set at .15 to make sure we have caught some momentum 
        //and we're actually moving and not just reacting to something in the world
        is_Moving = charController.velocity.magnitude > 0.15f;

    }
}

