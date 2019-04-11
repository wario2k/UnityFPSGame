using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour
{
    [SyncVar] //syncing player's health using server logic
    private float health = 100f;
    FPSPlayerAnimations anim;
    public void Start()
    {
        anim = GetComponent<FPSPlayerAnimations>();
    }

    public bool isDead()
    {
        if (health < 1f)
            return true;
        return false;
    }
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
            //playing death scene
            print("Playing death scene from player health");
           
        }
    }
}
