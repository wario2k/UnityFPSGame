using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour
{
    [SyncVar] //syncing player's health using server logic
    public float health = 100f;

  
    public void TakeDamage(float damage)
    {
        //if object is active on the server
        //we don't want individual clients to manage the health bar for players
        if(!isServer)
        {
            return;
        }
        health -= damage;
        print("damage recieved");
        if(health <=0)
        {

        }
    }
}
