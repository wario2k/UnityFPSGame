using UnityEngine.Networking;
/* *
   CLASS NAME

          PlayerHealth : NetworkBehaviour 
                       
    DESCRIPTION

          This class handles dealing damage to the player. Health is managed by the server and not the individual player objects. 
          The Sync var allows the server to handle dealing and updating damage to the player.         

          This class inherits from Network Behavior class which lets the damage to be dealt over the network. 
                   
    AUTHOR

            Aayush B Shrestha

    DATE

            2:37pm 4/12/2019  
                       
 * */
public class PlayerHealth : NetworkBehaviour
{
    [SyncVar] //syncing player's health using server logic
    private float health = 100f;
    //animation component
    FPSPlayerAnimations anim;

    /* *
    NAME

          public void Start()
                       
    DESCRIPTION

          This function is called before the update to cache the reference to the FPSPlayerAnimations component which handles playing animations in the scene.

    AUTHOR

            Aayush B Shrestha

    DATE

            12:37pm 3/10/2019  
    * */

    public void Start()
    {
        anim = GetComponent<FPSPlayerAnimations>();
    }

    /* *
   NAME

        bool isDead()

   DESCRIPTION

         This function is called by the player to determine whether or not he should no longer be able to move.
         If at any point after getting hit x number of times, the health goes below zero, you want to set the trigger to play the death animation.         

   RETURNS
        True if health is below 1f. 
               
   AUTHOR

           Aayush B Shrestha
        
   DATE

           12:37pm 3/10/2019  
        
           * */

    public bool isDead()
    {
        if (health < 1f)
            return true;
        return false;
    }


    /* *
    NAME

         public void TakeDamage(float damage) - deals damage to the player

    SYNOPSIS:
        
         public void TakeDamage(float damage) 
                float damage - damage to be dealt to the player        

    DESCRIPTION

          This function is called to deal damage to the player. Will decrease health based on damage dealt.        

    RETURNS
        
        None       

    AUTHOR

            Aayush B Shrestha

    DATE

            11:12 am 3/10/2019  

    * */
    public void TakeDamage(float damage)
    {
        //if object is active on the server
        //we don't want individual clients to manage the health bar for players
        if(!isServer)
        {
            return;
        }
        health -= damage;

        if(health <=1)
        {
            //playing death scene log interaction.
            print("Playing death scene from player health");
           
        }
    }
}
