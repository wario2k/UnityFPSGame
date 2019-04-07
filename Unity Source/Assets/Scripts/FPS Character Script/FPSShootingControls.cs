using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking; 

public class FPSShootingControls : NetworkBehaviour
{
    private Camera mainCamera; //will get reference to main camera

    //variables to determine rate of fire for the gun player is using
    private float fireRate = 15f;
    private float nextTimeToFire = 0f;

    [SerializeField]
    private GameObject concrete_Impact, blood_Impact;

    public float damageAmount = 5f;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = transform.Find("FPS View").Find("FPS Camera").GetComponent<Camera>();
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
                //if we hit other players 
                if (hit.transform.tag == "Enemy") 
                {
                    //since we're dealing with server logic, we need to call the [Command] method invocation to make sure the server handles the health
                    //and not any client players
                    CmdDealDamage(hit.transform.gameObject, hit.point, hit.normal);
                }
                else// we're probably hitting something in the world and not a player object
                {
                    //information about the object that is hit is stored in @RaycastHit hit
                    //using that information we are instantiating a concrete impact game object which shows where we registered the hit
                    Instantiate(concrete_Impact, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }
    }
    /// <summary>
    /// Deals damage to the player hit using the server to manage health state and not rely on client to manage inidividual health states
    /// </summary>
    /// <param name="obj">Game object reference</param>
    /// <param name="pos">Position of impact</param>
    /// <param name="rotation">Normal from point of impact.</param>
    [Command] //can be put on methods from network behavior and allow them to be invoked on the server from the client 
    void CmdDealDamage(GameObject obj,Vector3 pos, Vector3 rotation)
    {
        obj.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
        Instantiate(blood_Impact, pos, Quaternion.LookRotation(rotation));
    }
}
