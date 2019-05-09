using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* *
 * 
   CLASS NAME

          public class FPSHandsWeapon : MonoBehaviour
                       
    DESCRIPTION

          This is a utility class that is utilized by the Weapons in game to render the animations for the action and also couple the sound that goes with it. 

    AUTHOR

            Aayush B Shrestha

    DATE

            12:37pm 5/6/2019  
                     
 * */

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

    /* *

          NAME

               void Awake() 

          DESCRIPTION

              This function is caching the reference to the Muzzleflash game object in the scene. 
              This is happening before the first frame plays on scene awake to reduce load during updates. 
              This function is also getting references to the audio and animation players to trigger aniamtions and audio on gun actions.

           RETURNS 

               Nothing

          AUTHOR

                  Aayush B Shrestha

          DATE

                  12:47pm 2/10/2019  

       * */
    void Awake()
    {
        muzzleFlash = transform.Find("MuzzleFlash").gameObject;
        muzzleFlash.SetActive(false);
        audioManager = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }


    /* *

         NAME

              void Shoot() - updates all animations and sounds for shooting action

         DESCRIPTION

             This function is called when the player shoots using the mouse click action. 
             It triggers the shooting animation which is a slight kick-back on the gun in the scene 
             and also simultaneously plays the appropriate sound depending on what gun the player is shooting.

          RETURNS 

              Nothing

         AUTHOR

                 Aayush B Shrestha

         DATE

                 12:47pm 2/10/2019  

      * */

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


    /* *

         NAME

              IEnumerator TurnMuzzleFlashOn() - handles muzzle flash activation

         DESCRIPTION

             This IEnumerator is a utility function that is utilized by the Shoot() function to trigger the muzzle flash
             component when the gun is shot yield for completion for 0.05 seconds and then turn it off again so that the flash
             does not stay in the scene.

          RETURNS 

              Nothing

         AUTHOR

                 Aayush B Shrestha

         DATE

                 3:47pm 2/10/2019  

      * */

    IEnumerator TurnMuzzleFlashOn()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.SetActive(false);
    }

    /* *

            NAME

                
                public void Reload() - handles reloading action

            DESCRIPTION

                This function is called when the player tries to reload the gun in the scene. Initiates the couroutine to play the reloading sound
                and sets the trigger to play the reloading animation in the scene. This is different based on the type of gun the player is using.              
                        

             RETURNS 

                 Nothing

            AUTHOR

                    Aayush B Shrestha

            DATE

                    3:47pm 4/10/2019  

         * */

    public void Reload()
    {
        //initiate the sound for the reload animation
        StartCoroutine(PlayReloadSound());
        //set trigger to initiate animation in game object
        anim.SetTrigger(RELOAD);
    }

    /* *

         NAME

              IEnumerator TurnMuzzleFlashOn() - triggers reloading sound

         DESCRIPTION

             This IEnumerator is a utility function that is utilized by the Reload() function to trigger the reload animation and sound
             when the player tries to reload in the scene. It yeilds until both the sound and animation complete execution.           

          RETURNS 

              Nothing

         AUTHOR

                 Aayush B Shrestha

         DATE

                 3:47pm 2/10/2019  

      * */

    IEnumerator PlayReloadSound()
    {
        yield return new WaitForSeconds(0.08f);
        //set sound you want to play

        audioManager.clip = reloadClip;

        //play the clip
        audioManager.Play();
    }
}
