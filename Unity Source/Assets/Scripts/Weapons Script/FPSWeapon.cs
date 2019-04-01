using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSWeapon : MonoBehaviour
{

    private GameObject muzzleFlash;
    // Start is called before the first frame update
    void Awake()
    {
        //tries to find it in the scene and attach it 
        muzzleFlash = transform.Find("Muzzle Flash").gameObject;
    }

    // Called when the gun is fired to activate muzzle flash 
    public void Shoot()
    {
        //calls the enumerator to activate the flash and then turn it off in .01 seconds
        StartCoroutine(TurnOnMuzzleFlash());
    }

    IEnumerator TurnOnMuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.01f);
        muzzleFlash.SetActive(false);
    }
}
