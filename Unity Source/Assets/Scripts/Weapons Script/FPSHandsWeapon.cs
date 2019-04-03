using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSHandsWeapon : MonoBehaviour
{
    //for loading shooting and reloading audio clips for the gun objects
    public AudioClip shootClip, reloadClip;
    //this object is used to play the audio clip associated with the action 
    private AudioSource audioManager;

    //for updating muzzle flash when shooting 
    private GameObject muzzleFlash;
    //for updating animations in the game object
    private Animator anim;

    private const string SHOOT = "Shoot";
    private const string RELOAD = "Reload";

    /// <summary>
    /// ON awake attach all necessary controllers
    /// muzzleFlash object - controls flash when gun is shot
    /// audioManager - plays sound when gun is fired
    /// anim - plays animation for gun being shot
    /// </summary>
    void Awake()
    {
        muzzleFlash = transform.Find("MuzzleFlash").gameObject;
        muzzleFlash.SetActive(false);
        audioManager = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }
  
    /// <summary>
    /// This function is called when the player shoots to gun to paly the necessary animations
    /// </summary>
    public void Shoot()
    {
        //set sound you want to play
        audioManager.clip = shootClip;
        //play sound clip
        audioManager.Play();
        //display muzzle flash in game
        StartCoroutine(TurnMuzzleFlashOn());
        //set trigger to shoot to activate animation
        anim.SetTrigger(SHOOT);
    }

    /// <summary>
    /// Turns the muzzle flash on for 0.05 ms and then turns it off again 
    /// </summary>
    IEnumerator TurnMuzzleFlashOn()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.SetActive(false);
    }

    /// <summary>
    /// Handles animations and sounds for reload action on gun
    /// </summary>
    public void Reload()
    {
        //initiate the sound for the reload animation
        StartCoroutine(PlayReloadSound());
        //set trigger to initiate animation in game object
        anim.SetTrigger(RELOAD);
    }

    IEnumerator PlayReloadSound()
    {
        yield return new WaitForSeconds(0.08f);
        //set sound you want to play

        audioManager.clip = reloadClip;

        //play the clip
        audioManager.Play();
    }
}
