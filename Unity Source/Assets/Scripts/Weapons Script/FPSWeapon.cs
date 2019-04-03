﻿using System.Collections;
using UnityEngine;

/*
 * 
 * */
public class FPSWeapon : MonoBehaviour
{
    private const string MUZZLE_FLASH = "Muzzle Flash";
    public GameObject muzzleFlash;
    //used to find the game object in the scene
    GameObject item;
 
// Start is called before the first frame update
void Awake()
    {
        //tries to find the muzzle flash shader in the scene and attach it 
        item = transform.Find(MUZZLE_FLASH).gameObject;
        if(item == null)
        {
            Debug.Log("Could not find the object");
        }
        else
        {
            muzzleFlash = item;
        }
        muzzleFlash.SetActive(false);
    }

    // Called when the gun is fired to activate muzzle flash 
    public void Shoot()
    {
        //calls the enumerator to activate the flash and then turn it off in .01 seconds
        StartCoroutine(TurnOnMuzzleFlash());
    }

    /// <summary>
    /// This enumerator finds takes the muzzleFlash game object and briefly turns it on.
    /// </summary>
    /// <returns>nothing.</returns>
    IEnumerator TurnOnMuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.01f);
        muzzleFlash.SetActive(false);
    }
}
