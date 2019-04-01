﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSPlayerAnimations : MonoBehaviour
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

    //caled like start when game is initiated 
    private void Awake()
    {
        //getting animator object from the game state
        anim = GetComponent<Animator>();

    }
    /// <summary>
    /// Movement acc to specified magnitude.
    /// </summary>
    /// <param name="magnitude">velocity of the game object.</param>
    public void Movement(float magnitude)
    {
        anim.SetFloat(MOVE, magnitude); 
    }

    /// <summary>
    /// Player decides to jump this needs to update the animation
    /// </summary>
    /// <param name="velocity">Velocity in the y direction </param>
    public void PlayerJump(float velocity)
    {
        anim.SetFloat(VELOCITY_Y, velocity);
    }

    /// <summary>
    /// Update player stance to go to crouching position.
    /// </summary>
    /// <param name="isCrouching">Is crouching is a flag that the player object has and is set to true if the player wants to crouch</param>
    public void PlayerCrouch(bool isCrouching)
    {
        anim.SetBool(CROUCH, isCrouching);
    }

    /// <summary>
    /// This method handles the player moving in crouched stance 
    /// Speed is reduced since walking speed > crouch speed. 
    /// </summary>
    /// <param name="magnitude">magnitude of movement pulled from the game object</param>
    public void PlayerCrouchWalk(float magnitude)
    {
        anim.SetFloat(CROUCH_WALK, magnitude);
    }
   

    /// <summary>
    /// This function will handle animations for shooting depending on stand/crouch positions 
    /// </summary>
    /// <param name="isStanding">If set to <c>true</c> is standing.</param>
    public void Shoot(bool isStanding)
    { 
        if(isStanding)
        {
            anim.SetTrigger(STAND_SHOOT);
        }
        else
        {
            anim.SetTrigger(CROUCH_SHOOT);
        }
    }
    /// <summary>
    /// This method will handle the reload animation for the weapon.
    /// </summary>
    public void ReloadGun()
    {
        anim.SetTrigger(RELOAD); 
    }

}
