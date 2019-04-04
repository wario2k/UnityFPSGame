using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSShootingControls : MonoBehaviour
{
    private Camera mainCamera; //will get reference to main camera

    //variables to determine rate of fire for the gun player is using
    private float fireRate = 15f;
    private float nextTimeToFire = 0f;

    [SerializeField]
    private GameObject concrete_Impact;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    //This function will only call shoot since we are using this to update the shooting animations and updating the scene using RayCasting to display hits on objects
    void Update()
    {
        Shoot();
    }

    public void Shoot()
    {
        if(Input.GetMouseButtonDown(0) && Time.time > nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate; //to control rate of fire to avoid firing too fast 
            //if object is hit -> determined by tracing a ray from the main camera to the direction the camera is pointing at 
            //will be used to show impact
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit)) 
            {
                //information about the object that is hit is stored in @RaycastHit hit
                //using that information we are instantiating a concrete impact game object which shows where we registered the hit
                Instantiate(concrete_Impact, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }
}
