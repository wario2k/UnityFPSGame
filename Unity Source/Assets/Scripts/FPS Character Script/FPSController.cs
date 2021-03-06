﻿using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

/* *
   CLASS NAME

          public class FPSController : NetworkBehaviour - inherits from network behavior
                       
    DESCRIPTION

          This is the primary controller for the player game object. 
          This class handles all movement and interactions in the game that the player object is capable of performing.        
                  

    AUTHOR

            Aayush B Shrestha

    DATE

            2:37pm 3/12/2019  
 * */

public class FPSController : NetworkBehaviour //for network controls
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
    //gets player character controller
    private CharacterController charController;
    private Vector3 moveDirection = Vector3.zero;

    //to test which layer you are currently on
    public LayerMask groundLayer;
    //calculate how far you're from the ground
    private float rayDistance;

    private float default_ControllerHeight;

    private Vector3 default_camPos;

    private float camHeight;

    //controller for animations 
    FPSPlayerAnimations playerAnimation;

    //color vars to render player models in different colors 
    private Color[] playerColors = {
                                                    new Color(255,195,0,255), //orange-yellow
                                                    new Color(252,208,193,255), //red
                                                    new Color(0,0,0,255) //black
                                                    };

    public Renderer playerRenderer;

    private PlayerHealth playerHealth;

    //determines when to load the win screen

    public bool isEnd = false;

    //reference for weapons 
    [SerializeField]
    private WeaponManager weapon_Manager;
    //reference to the current weapon held
    private FPSWeapon current_Weapon;
    public GameObject playerHolder, weaponsHolder; //for differentiating between local/client player objects
    public GameObject [] weapons_FPS; //get a reference to all the guns held

    public FPSMouseLook[] mouseLook; //mouse controls for multiple individuals
    private Camera mainCam;
    //controls for regulating rate of fire for weapons 
    private float fireRate = 15f;
    private float nextTimeToFire = 0f;

    [SerializeField] //reference for player layer weapons
    private WeaponManager handsWeapon_Manager;
    private FPSHandsWeapon current_Hands_Weapon;


/* *
    NAME

          void Start() - called before the first update to get all references and cache them
                       
    DESCRIPTION
          
          Gets all necessary components to handle all player interactions. 
          Also depending on whether the player is local or non-local, certain views have to be handled so that is also done.
          For the local player we want to render just the hands in their view and not the main body. 
          If the player is not local meaning we see an enemy player in the scene, we do not want to have just the hands rendered but also the full body.
          This is all done at start since this will make sure the correct models are rendered.
                  

    AUTHOR

            Aayush B Shrestha

    DATE

            2:37pm 2/12/2019  
 * */

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>(); //get health component

        //since player will be in an upright position when starting so normal walk speed is applied
        speed = walkSpeed;

        //getting references to all the views when initializing

        //firstPersonView goes into the FPS Player model and then finds the camera view 
        //that was defined inside of the fps player prefab and attaches itself accordingly

        firstPerson_View = transform.Find("FPS View").transform;

        charController = GetComponent<CharacterController>();
        is_Moving = false;
        //determines if the player is crouching if height is half
        rayDistance = charController.height * 0.5f + charController.radius;

        default_ControllerHeight = charController.height;
        //fps view in scene 
        default_camPos = firstPerson_View.localPosition;
        //get reference to player animation
        playerAnimation = GetComponent<FPSPlayerAnimations>();

        //activate whatever weapon is being carried 
        weapon_Manager.weapons[0].SetActive(true);
        //attach the component and get reference to the script to activate it 
        current_Weapon = weapon_Manager.weapons[0].GetComponent<FPSWeapon>();

        //getting player layer to get correct models in scene
        //activate first weapon
        handsWeapon_Manager.weapons[0].SetActive(true);

        //get reference to the active weapon's scripts
        current_Hands_Weapon = handsWeapon_Manager.weapons[0].GetComponent<FPSHandsWeapon>();

        //testing to apply appropriate masking for views
        if (isLocalPlayer)
        {
            makeLocalPlayerUpdates();
        }

        //if the palyer is not local 
        //if player is joining as a remote client we do not want to show them the extra camera layer so that the game seems more natural
        //update playerHolder layer to enemy layer to hide the fps camera object that contains the extra hands for perspective
        if (!isLocalPlayer)
        {
            makeNonLocalPlayerViewChanges();
        }

        //update mouse look for local/non-local players

        if(!isLocalPlayer)
        {
            for(int i = 0; i < mouseLook.Length; i++)
            {
                mouseLook[i].enabled = false;

            }
        }
        //getting reference to appropriate camera

        mainCam = transform.Find("FPS View").Find("FPS Camera").GetComponent<Camera>();
        mainCam.gameObject.SetActive(false);

        if(!isLocalPlayer)
        {
            changePlayerColor();
        }
    }



/* *
       
    NAME

          changePlayerColor() -> changes the color of the non-local player 
                       
    DESCRIPTION
          
          Since we don't want both players to look alike, depending on if the player is not the local player the color of his/her top is changed.
                  

    AUTHOR

            Aayush B Shrestha

    DATE

            2:37pm 2/12/2019  
                      
 * */
    public void changePlayerColor()
    {
        for (int i = 0; i < playerRenderer.materials.Length; i++) //accessing all materials instantiated on the element
        {
            playerRenderer.materials[i].color = playerColors[i]; // change non-local player's color
        }
    }

/* *

   NAME

         makeLocalPlayerUpdates() -> change the camera view depending on player

   DESCRIPTION

         The views being rendered in game are dependant on what the current player is.
         The local player only wants to see the hands while shooting since that is the behavior most FPS games have.
         In order to achieve this the player model is made in-active in the local player's view.

    RETURNS

        Nothing       


   AUTHOR

           Aayush B Shrestha

   DATE

           2:37pm 2/22/2019  

* */

    public void makeLocalPlayerUpdates()
    {
        //need to check if local player
        playerHolder.layer = LayerMask.NameToLayer("Player");

        foreach (Transform child in playerHolder.transform)//getting all child objects of the player holder
        {
            child.gameObject.layer = LayerMask.NameToLayer("Player");
        }

        for (int i = 0; i < weapons_FPS.Length; i++)
        {
            weapons_FPS[i].layer = LayerMask.NameToLayer("Player");
        }
        weaponsHolder.layer = LayerMask.NameToLayer("Enemy");

        foreach (Transform child in weaponsHolder.transform) //getting all child objects of the weapons holder
        {
            child.gameObject.layer = LayerMask.NameToLayer("Enemy"); //updating appropriate views
        }
    }



/* *

   NAME

         makeNonLocalPlayerViewChanges() -> change the camera view for non-local player

   DESCRIPTION

         This function updates the view for the enemy player. If we are observing an enemy in the game, 
         we only want to be able to see their player model and not their FPS view. The "Enemy" layer mask indicates
         that we want to hide the enemy's FPS Camera view so that we don't render an extra set of hands for the enemy player in the game.

    RETURNS

        Nothing       


   AUTHOR

           Aayush B Shrestha

   DATE

           2:37pm 2/22/2019  

* */

    public void makeNonLocalPlayerViewChanges()
    {
        playerHolder.layer = LayerMask.NameToLayer("Enemy");

        foreach (Transform child in playerHolder.transform)//getting all child objects of the player holder
        {
            child.gameObject.layer = LayerMask.NameToLayer("Enemy");
        }

        for (int i = 0; i < weapons_FPS.Length; i++)
        {
            weapons_FPS[i].layer = LayerMask.NameToLayer("Enemy");
        }

        weaponsHolder.layer = LayerMask.NameToLayer("Player");

        foreach (Transform child in weaponsHolder.transform) //getting all child objects of the weapons holder
        {
            child.gameObject.layer = LayerMask.NameToLayer("Player"); //updating appropriate views
        }

    }


/* *

   NAME

         public override void OnNetworkDestroy() - called when the server player is killed 
                
   DESCRIPTION

         If the non-server player kills the host, the server will shutdown. Instead of making the game crash in this case, 
         we want to switch over to the win scene which will allow the player to either exit the game or play another one.
         
    RETURNS

        Nothing       


   AUTHOR

           Aayush B Shrestha

   DATE

           2:37pm 2/22/2019  

* */

    public override void OnNetworkDestroy()
    {

        SceneManager.LoadSceneAsync(2);
    }


/* *

   NAME

         public override void OnNetworkDestroy() - called when another player disconnects from the server
                
   DESCRIPTION

         Since we are only dealing with two players, when one player disconnects from the game after being killed,
         we want to switch over to the win screen, Which will then allow the player to either quit the game or start a new one.        
         
    RETURNS

        Nothing       


   AUTHOR

           Aayush B Shrestha

   DATE

           2:37pm 2/22/2019  

* */

    private void OnDisconnectedFromServer( )
    {
        SceneManager.LoadSceneAsync(2);
    }

/* *

  NAME

       public override void OnStartLocalPlayer() - set the tag for player object in game

  DESCRIPTION

        Since the tags are used to render specific views, it is important to set the local player view at start.        

   RETURNS

       Nothing       


  AUTHOR

          Aayush B Shrestha

  DATE

          2:37pm 2/22/2019  

* */

    public override void OnStartLocalPlayer()
    {
        tag = "Player";

    }

/* *

NAME

     void Update () - called once per frame to make necessary updates 
     
DESCRIPTION

      Since scripts are shared, we need to make sure a check is being made to ensure only one player is moving when certain
      actions are being invoked. This is handled once per frame since we don't want the local-player actions affecting the non-local player.
    
 RETURNS

     Nothing       


AUTHOR

        Aayush B Shrestha

DATE

        4:37pm 2/22/2019  

* */

    void Update()
    {
        if(isEnd)
        {
            //load new scene
            Debug.Log("Changing Scene");
            FindObjectOfType<GameManager>().EndGame();
            //testing destroy functionality
        }
        //updating so that local camera perspective is used instead of server player
        if (isLocalPlayer)
        {
            if(!mainCam.gameObject.activeInHierarchy)
            {
                mainCam.gameObject.SetActive(true);
            }

        }
        //making sure we're only updating for the local player and not activate for other's since we are sharing scripts
        //if we're not running on our own machine
        //exit
        if (!isLocalPlayer)
        {
            return;
        }

        PlayerMovement();

        SelectWeapon();

    }

    /* *

    NAME

         void  void PlayerMovement() - moves the player based on keyboard input
    DESCRIPTION
             
        This function determines what direction the player wants to move in based on 
        the allowed set of key presses W,A,S,D.
        Depending on the movement of the player, camera and rotation angles are also adjusted for smooth transitions in the scene.       

     RETURNS

         Nothing       

    AUTHOR

            Aayush B Shrestha

    DATE

            4:37pm 2/22/2019  

    * */

    void PlayerMovement()
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
            PlayerCrouchingAndSprinting();

            moveDirection = new Vector3(inputX * inputModifyFactor, // x-axis movement
                                                       -antiBumpFactor, //for smoother character animation if we bump into objects
                                                           inputY * inputModifyFactor); // y-axis movement

            //to make sure we're using local co-ordinates and not world co-ordinates 
            moveDirection = transform.TransformDirection(moveDirection) * speed;

            //player decides to jump
            PlayerJump();
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
        //the handle animations function will then update the animation of the player in the scene 
        HandleAnimations(); 

    }


    /* *

    NAME

         void PlayerCrouchingAndSprinting() - Handles crouching action
                
    DESCRIPTION
             
        This function determines if the player can crouch or stand up from crouching postion when the "c" key is hit.
        Camera view and animation is updated to reflect the position of the player since the crouching view would be lower than the standing view.
        The movement speed is also restricted based on player's orientation. If player goes from a sprint to a crouch, we want to lower his speed.      

     RETURNS

         Nothing       

    AUTHOR

            Aayush B Shrestha

    DATE

            4:37pm 2/22/2019  

    * */

    void PlayerCrouchingAndSprinting()
    { 
        if(Input.GetKey(KeyCode.C))
        {
            //set to crouching since button was pressed 
            if(!is_Crouching)
            {
                is_Crouching = true;
            }
            else
            {
                if(CanGetUp())
                {
                    is_Crouching = false;
                }
            }

            StopCoroutine(MoveCameraCrouch());
            StartCoroutine(MoveCameraCrouch());


        }
        //setting movment speed based on standing position of character 
        if(is_Crouching)
        {
            //reduced speed for moving while crouched 
            speed = crouchSpeed;
        }
        else
        {
            //if sprint key is held down we want the player to sprint
            if(Input.GetKey(KeyCode.LeftShift))
            {
                speed = runSpeed;
            }
            else //normal walk speed otherwise
             speed = walkSpeed;
        }
        //adjust player to crouch using animations 
        playerAnimation.PlayerCrouch(is_Crouching);
    }


    /* *

    NAME

         bool CanGetUp() - determines if the player is crouching
                
    DESCRIPTION
             
        This function determines if the player is in a crouched position. This is determined using a raycast. We don't want to abruptly 
        cancel crouching animation so we want to set the can get up flag to false until the animation is complete.

     RETURNS

         True if completely crouched else False.      

    AUTHOR

            Aayush B Shrestha

    DATE

            4:37pm 2/22/2019  

    * */

    bool CanGetUp()
    {
        //where we are casting the ray from 
        Ray groundRay = new Ray(transform.position, transform.up);
        //where we are going to be casting to 

        //out groundHit is the information recieved when ray cast is done 
        //@groundRay - source ray 
        //@charController.radius - radius of the sphere we're going to be casting on
        //@params rayDistance - max distance of ray cast 
        //@params groundLayer - test with which layers we are colliding.
        //@params groundHit - will store the information returned and tell us if we're touching the layer specified or not
        if (Physics.SphereCast(groundRay, charController.radius + 0.05f, out RaycastHit groundHit, rayDistance, groundLayer))
        {
            //@groundHit.point is the exact point where the sphere and the ground layers collide
            if (Vector3.Distance(transform.position, groundHit.point) < 2.3f)
            {
                //since we haven't actually completed the motion yet we will return false 
                return false;
            }
        }

        //once completely crouched you can get up again.
        return true;
    }


    /* *

    NAME

         IEnumerator MoveCameraCrouch() - handles camera transition for crouching
                
    DESCRIPTION
             
        This function handles the updating of camera postion when changing orientation from standing to crouching.
        This is called inside of a couroutine to ensure smooth transition.      

     RETURNS

         yield return - yeilds until the action has finished executing      

    AUTHOR

            Aayush B Shrestha

    DATE

            4:37pm 2/22/2019  

    * */

    IEnumerator MoveCameraCrouch()
    {
        //if player is crouching set lower the height to make them crouch 
        charController.height = is_Crouching ? default_ControllerHeight / 1.5f : default_ControllerHeight;
        charController.center = new Vector3(0f, charController.height / 2f, 0f);

        //if person is crouching we set the cam height to crouch level 
        camHeight = is_Crouching ? default_camPos.y / 1.5f : default_camPos.y;

        //keep checking if the person is stil crouching and update local view
        while(Mathf.Abs(camHeight - firstPerson_View.localPosition.y)>0.01f)
        {
            // lerping between local and updated position for croucuching view
            firstPerson_View.localPosition = Vector3.Lerp(firstPerson_View.localPosition,
                                        new Vector3(default_camPos.x, camHeight, default_camPos.z), Time.deltaTime * 11f);

            yield return null;
        }

    }

    /* *

    NAME

         void PlayerJump() - handles player jumping
                
    DESCRIPTION
             
        This function handles updating all actions for the player jumping when pressing the "space bar". 
        When jumping velocity in the y-axis needs to be updated and the camera needs to be checked to ensure 
        we are not in the air directly from a croiuching position.       

     RETURNS

         Nothing     

    AUTHOR

            Aayush B Shrestha

    DATE

            6:37pm 2/22/2019  

    * */

    void PlayerJump()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            if(is_Crouching)
            {
                if(CanGetUp())
                {
                    is_Crouching = false;
                    playerAnimation.PlayerCrouch(is_Crouching);

                    StopCoroutine(MoveCameraCrouch());
                    StartCoroutine(MoveCameraCrouch());
                }

            }
            else
            {
                moveDirection.y = jumpSpeed;
            }
        }
    }

    /* *

   NAME

        void HandleAnimations() - handles player animations in scene

   DESCRIPTION

       This function handles updating all animations for the player in game.Handles movement, camera rotation, shooting and reloading animations 
       and triggers appropriate animations for each action. The velocity of the player is acquired from the scene.      

    RETURNS

        Nothing     

   AUTHOR

           Aayush B Shrestha

   DATE

           6:37pm 2/22/2019  

   * */

    void HandleAnimations()
    {
        if(playerHealth.isDead())
        {
            Debug.Log("Playing Death Scene From Player Controller!");
            playerAnimation.Death();
            FindObjectOfType<GameManager>().EndGame();
            isEnd = true;
           
        }
        playerAnimation.Movement(charController.velocity.magnitude);
        playerAnimation.PlayerJump(charController.velocity.y);

        //handle crouching animation
        if(is_Crouching && charController.velocity.magnitude> 0f)
        {
            playerAnimation.PlayerCrouchWalk(charController.velocity.magnitude);

        }

        //handle shooting
        if(Input.GetMouseButtonDown(0) && Time.time > nextTimeToFire) //if right mb click and time since click is greater than next time to fire interval
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            //if is crouching trigger crouch fire else standing fire 
            if(is_Crouching)
            {
                playerAnimation.Shoot(false);
            }
            else
            {
                playerAnimation.Shoot(true);
            }
            //activate muzzle flash
            current_Weapon.Shoot();
            //activate shooting animation for player layer
            current_Hands_Weapon.Shoot();


        }
        //if playre hits "R" activate reload motion
        if(Input.GetKeyDown(KeyCode.R))
        {
            //play animations on game object
            playerAnimation.ReloadGun();
            //activate the animation for player layer
            current_Hands_Weapon.Reload();
        }
    }



    /* *

   NAME

        void activatePlayerLayerWeapon(int index) - enables player view for weapons

   SYNOPSIS
     void activatePlayerLayerWeapon(int index) 
        int index - index of weapon to be activated   

   DESCRIPTION

       This function handles changing in game rendering of weapons to display only the primary weapon and hide all other weapons from the scene when initially loading in.
       This makes sure we don't have the player holding multiple weapons simultaneously, when loading into the scene.
            
    RETURNS

        Nothing     

   AUTHOR

           Aayush B Shrestha

   DATE

           9:37pm 2/23/2019  

   * */

    void activatePlayerLayerWeapon(int index)
    {
        //if weapon isn't already selected, deactivates all other weapons in model and sets the selected weapon as active 
        if(!handsWeapon_Manager.weapons[index].activeInHierarchy)
        {
            for(int i = 0; i < handsWeapon_Manager.weapons.Length; i++)
            {
                handsWeapon_Manager.weapons[i].SetActive(false);
            }
            current_Hands_Weapon = null;
            handsWeapon_Manager.weapons[index].SetActive(true);

            current_Hands_Weapon = handsWeapon_Manager.weapons[index].GetComponent<FPSHandsWeapon>();

        }
    }

    /* *

   NAME

        void SelectWeapon() - handles switching of weapons

   DESCRIPTION

       This function handles changing in game rendering of weapons ensure when the player switches a weapon, that is set to active
       and all the other weapons in the heirarchy are disabled. This makes sure we don't have a player holding multiple weapons with the same hand.
       There also needs to be a check for the type of weapon, since loading the weapon animation is different for pistols and rifles.    
            
    RETURNS

        Nothing     

   AUTHOR

           Aayush B Shrestha

   DATE

           9:37pm 2/23/2019  

   * */
    void SelectWeapon()
    {

        //if player selects 1 select pistol
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            activatePlayerLayerWeapon(0);
            if(!weapon_Manager.weapons[0].activeInHierarchy) //if the weapons is not already active
            {
                //deactivate all other weapons from the scene
                for(int i = 0; i< weapon_Manager.weapons.Length; i++)
                {
                    weapon_Manager.weapons[i].SetActive(false);
                }
                current_Weapon = null;
                weapon_Manager.weapons[0].SetActive(true);
                //get reference to the new current weapon
                current_Weapon = weapon_Manager.weapons[0].GetComponent<FPSWeapon>();
                //change animation to use pistol stance 
                //expects @isPistol - bool
                playerAnimation.ChangeController(true);
            }
        }

        //player hits 2
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            activatePlayerLayerWeapon(1);
            if (!weapon_Manager.weapons[1].activeInHierarchy) //if the weapons is not already active
            {
                //deactivate all other weapons from the scene
                for (int i = 0; i < weapon_Manager.weapons.Length; i++)
                {
                    weapon_Manager.weapons[i].SetActive(false);
                }
                current_Weapon = null;
                weapon_Manager.weapons[1].SetActive(true);
                //get reference to the new current weapon
                current_Weapon = weapon_Manager.weapons[1].GetComponent<FPSWeapon>();
                //change animation to use pistol stance 
                //expects @isPistol - bool
                //passing false since this gun is not a pistol
                playerAnimation.ChangeController(false);
            }
        }

        //player hits 3 
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            activatePlayerLayerWeapon(2);
            if (!weapon_Manager.weapons[2].activeInHierarchy) //if the weapons is not already active
            {
                //deactivate all other weapons from the scene
                for (int i = 0; i < weapon_Manager.weapons.Length; i++)
                {
                    weapon_Manager.weapons[i].SetActive(false);
                }
                current_Weapon = null;
                weapon_Manager.weapons[2].SetActive(true);
                //get reference to the new current weapon
                current_Weapon = weapon_Manager.weapons[1].GetComponent<FPSWeapon>();
                //change animation to use pistol stance 
                //expects @isPistol - bool
                //passing false since this gun is not a pistol
                playerAnimation.ChangeController(false);
            }
        }

    }
}

