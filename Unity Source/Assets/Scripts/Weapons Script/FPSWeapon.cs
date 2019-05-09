using System.Collections;
using UnityEngine;

/* *
   CLASS NAME

          public class FPSWeapon : MonoBehaviour
                       
    DESCRIPTION

          This is a utility class that is utilized by the Weapons in game to render the animation of being shot. 

    AUTHOR

            Aayush B Shrestha

    DATE

            12:37pm 5/6/2019  
                     
 * */

public class FPSWeapon : MonoBehaviour
{
    private const string MUZZLE_FLASH = "Muzzle Flash";
    public GameObject muzzleFlash;
    //used to find the game object in the scene
    GameObject item;

    /* *

       NAME

            void Awake() 

       DESCRIPTION

           This function is caching the reference to the Muzzleflash game object in the scene.
        
        RETURNS 

            Nothing

       AUTHOR

               Aayush B Shrestha

       DATE

               12:47pm 2/6/2019  
    * */
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

    /* *

      NAME

           void Shoot() 

      DESCRIPTION

          This function is called when the gun is fired to activate muzzle flash via the TurnOnMuzzleFlash couroutine. 

       RETURNS 

           Nothing

      AUTHOR

              Aayush B Shrestha

      DATE

              12:47pm 2/6/2019  
   * */
  
    public void Shoot()
    {
        //calls the enumerator to activate the flash and then turn it off in .01 seconds
        StartCoroutine(TurnOnMuzzleFlash());
    }

    /* *

         NAME

              IEnumerator TurnOnMuzzleFlash()

         DESCRIPTION

             This function is called when the gun is fired to activate muzzle flash in the scene and turn it of after a brief period of time. 

          RETURNS 

              Nothing

         AUTHOR

                 Aayush B Shrestha

         DATE

                 12:47pm 2/6/2019  
      * */

    IEnumerator TurnOnMuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.01f);
        muzzleFlash.SetActive(false);
    }
}
