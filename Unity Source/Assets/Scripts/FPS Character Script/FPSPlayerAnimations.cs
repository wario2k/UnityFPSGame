using System.Collections;
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
    /// Player decides to jump
    /// </summary>
    /// <param name="velocity">Velocity in the y direction</param>
    public void PlayerJump(float velocity)
    {
        anim.SetFloat(VELOCITY_Y, velocity);
    }

    public void PlayerCrouch(bool isCrouching)
    {
        anim.SetBool(CROUCH, isCrouching);
    }

    /// <summary>
    /// Player is crouch walking.
    /// </summary>
    /// <param name="magnitude">magnitude of movement</param>
    public void PlayerCrouchWalk(float magnitude)
    {
        anim.SetFloat(CROUCH_WALK, magnitude);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
