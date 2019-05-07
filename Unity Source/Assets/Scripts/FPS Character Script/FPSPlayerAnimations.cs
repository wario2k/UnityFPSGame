using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking; //for using networkbehavoir

/* *
   CLASS NAME

          FPSPlayerAnimations : NetworkBehaviour
                       
    DESCRIPTION

          This class handles all animations to be played in the game.
          The animations are played by setting triggers, which are then handled by the Animation Controller in Unity which is attached to the player object.        

          This class inherits from Network Behavior as we need to sync all animations over the network.
          We need to set triggers in the client and over the network as well to make sure animations are synced over the network
          so that animations on one player are seen by the other player.
                   
    AUTHOR

            Aayush B Shrestha

    DATE

            1:37pm 4/12/2019  
 * */
public class FPSPlayerAnimations : NetworkBehaviour
{

    private Animator anim;

    /// <summary>
    /// Const reference to names of the animator object settings 
    /// </summary>
    private const string MOVE = "Move";
    private const string VELOCITY_Y = "VelocityY";
    private const string CROUCH = "Crouch";
    private const string CROUCH_WALK = "CrouchWalk";

    private const string STAND_SHOOT = "StandShoot";

    private const string CROUCH_SHOOT = "CrouchShoot";

    private const string RELOAD = "Reload";

    private const string DEATH = "Death";


    //determines what postion to change player model animation to depending on the type of gun you are holding
    public RuntimeAnimatorController animController_Pistol, animController_MachineGun;

    //required to sync triggers and play animations over the network 
    private NetworkAnimator networkAnim;

   


    /* *

    NAME

          void Start() - called when the scene is loaded
                       
    DESCRIPTION

          This function handles caching as we want to be able to get reference to the network and local animator components.
          The references then allow us to set the appropriate triggers to play animations in the scene.         
                  
    Returns
        
        Nothing      
                   
    AUTHOR

            Aayush B Shrestha

    DATE

            1:37pm 4/12/2019  
                      
 * */

    private void Awake()
    {
        //getting animator object from the game state
        anim = GetComponent<Animator>();
        networkAnim = GetComponent<NetworkAnimator>();

    }

    /* *

    NAME

           public void Movement(float magnitude) - sets the movement trigger
    
    SYNOPSIS         
                 public void Movement(float magnitude) - sets the movement trigger
                   
                 float magnitude -> velocity in the x direction acquired from the scene.                
    DESCRIPTION
    
        This function sets the Magnitude trigger which is one of the several triggers in the animation controller in unity. 
        This is the speed the player is currently moving in the scene.
        Depending on your speed, the animations that are played are different.      
          
    Returns
        
        Nothing      
                   
    AUTHOR

            Aayush B Shrestha

    DATE

            1:37pm 4/12/2019  
                      
 * */

    public void Movement(float magnitude)
    {
        anim.SetFloat(MOVE, magnitude); 
    }


    /* *

     NAME

            public void PlayerJump(float velocity) - playing the jumping animation 

     SYNOPSIS         
                  public void PlayerJump(float velocity)

                  float velocity -> velocity in the y direction acquired from the scene.                
     DESCRIPTION

         This function sets the VELOCITY_Y trigger which is one of the several triggers in the animation controller in unity. 
         This is value is updated when the players momentum in the Y axis changes i.e the player is trying to jump. 
         If a certain threshold is met, the player moves up in the Y-axis while playing the jumping animation.
        
     Returns

         Nothing      

     AUTHOR

             Aayush B Shrestha

     DATE

             3:37pm 4/12/2019  

  * */

    public void PlayerJump(float velocity)
    {
        anim.SetFloat(VELOCITY_Y, velocity);
    }

    /* *

    NAME

           public void Death() - plays the dying animation

             
    DESCRIPTION

        This function sets the DEATH trigger which is one of the several triggers in the animation controllers that I implemented. 
        This is called when the player's health goes below 1f and plays the death animation.

    Returns

        Nothing      

    AUTHOR

            Aayush B Shrestha

    DATE

            3:37pm 4/12/2019  

 * */
    public void Death()
    {
        anim.SetTrigger(DEATH);
        networkAnim.SetTrigger(DEATH);

    }


    /* *

    NAME

            public void PlayerCrouch(bool isCrouching) - change the players orientation to be crouching.
    
    SYNOPSIS
            public void PlayerCrouch(bool isCrouching) - change the players orientation to be crouching.

            bool isCrouching - flag to determine whether or not the player is crouching
    DESCRIPTION

       This function determines when to change the player's posture to be crouching. If the flag is set to true the player's position is changed from standing to crouching.

    Returns

        Nothing      

    AUTHOR

            Aayush B Shrestha

    DATE

            3:37pm 4/12/2019  

 * */
    public void PlayerCrouch(bool isCrouching)
    {
        anim.SetBool(CROUCH, isCrouching);
    }


    /* *

    NAME

            public void PlayerCrouchWalk(float magnitude) - limit speed if the player is moving while crouched 
    
    SYNOPSIS
            public void PlayerCrouchWalk(float magnitude)
                      
            bool magnitude - velocity of the player in the x-direction 
                      
    DESCRIPTION

      This function plays the animation  of the player if the player is moving while crouched 
      so that the crouching player is not as fast as a standing player.
      The CROUCH_WALK trigger is set to start playing the crouch-walk animation in the scene.
          
    Returns

        Nothing      

    AUTHOR

            Aayush B Shrestha

    DATE

            3:37pm 4/12/2019  

 * */

    public void PlayerCrouchWalk(float magnitude)
    {
        anim.SetFloat(CROUCH_WALK, magnitude);
    }


    /* *

        NAME

                public void Shoot(bool isStanding)

        SYNOPSIS
                public void Shoot(bool isStanding)

                bool isStanding - is true if the player is standing

        DESCRIPTION

        This function will handle animations for shooting depending on whether the player is standing or crouching.

        Returns

            Nothing      

        AUTHOR

                Aayush B Shrestha

        DATE

                3:37pm 4/13/2019  

     * */
    public void Shoot(bool isStanding)
    { 
        if(isStanding)
        {
            anim.SetTrigger(STAND_SHOOT);

            //set network trigger
            networkAnim.SetTrigger(STAND_SHOOT);
        }
        else
        {
            anim.SetTrigger(CROUCH_SHOOT);

            //set network trigger
            networkAnim.SetTrigger(CROUCH_SHOOT);
        }
    }

    /* *

        NAME

                public void ReloadGun()


        DESCRIPTION

        This function will handle animations for Reloading the gun. The trigger is set in the scene to initiate the reloading animation.

        Returns

            Nothing      

        AUTHOR

                Aayush B Shrestha

        DATE

                3:37pm 4/13/2019  

     * */

    public void ReloadGun()
    {
        anim.SetTrigger(RELOAD);
        //set network trigger
        networkAnim.SetTrigger(RELOAD);
    }

    /* *

      NAME

             public void ChangeController(bool isPistol) - changes the animation controller based on the gun
      
      SYNOPSIS
             
              public void ChangeController(bool isPistol)       
              bool isPistol -> determines whether the player is holding a pistol or a rifle.

      DESCRIPTION
      
      Changes the controller based on what gun the player is currently holding.
      There are different ways the animations are being handled depending on the type of gun so this method sets the appropriate controller.        
     

      Returns

          Nothing      

      AUTHOR

              Aayush B Shrestha

      DATE

              3:37pm 4/13/2019  

   * */

    public void ChangeController(bool isPistol)
    {
        if(isPistol)
        {
            anim.runtimeAnimatorController = animController_Pistol;
        }
        else
        {
            anim.runtimeAnimatorController = animController_MachineGun;
        }
    }

}
