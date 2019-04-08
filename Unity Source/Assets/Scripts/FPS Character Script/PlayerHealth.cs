using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerHealth : NetworkBehaviour
{
    [SyncVar] //syncing player's health using server logic
    public float health = 100f;
    FPSPlayerAnimations anim;
    public void Start()
    {
        anim = GetComponent<FPSPlayerAnimations>();
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

            anim.Death();
            SceneManager.LoadScene("EndScene");
        }
    }
}
