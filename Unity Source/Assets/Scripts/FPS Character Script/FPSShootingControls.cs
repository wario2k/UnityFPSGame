using UnityEngine;

using UnityEngine.Networking;

/* *
   CLASS NAME

          FPSShootingControls : NetworkBehaviour
                       
    DESCRIPTION

          This class handles all shooting controls for the game. 
          If the player shoots an object, proper animations need to be played.         

          This class inherits from Network Behavior as we need to sync shooting animations and interactions over the network.
                   
    AUTHOR

            Aayush B Shrestha

    DATE

            1:37pm 4/12/2019  
 * */

public class FPSShootingControls : NetworkBehaviour
{
    private Camera mainCamera; //will get reference to main camera

    //variables to determine rate of fire for the gun player is using
    private float fireRate = 15f;
    private float nextTimeToFire = 0f;

    //getting objects to render when an object is hit. 
    [SerializeField]
    private GameObject concrete_Impact, blood_Impact;

    public float damageAmount = 33.33f;



    /* *
    NAME

         void Start() - called to cache reference to the main camera.
                       
    DESCRIPTION

         This function gets called before the first update. 
         The reference to the main camera is acquired bby going through the FPS View in the gameObject as it is a child of that object and the attach it to the 
         reference in this class
                   
    AUTHOR

            Aayush B Shrestha

    DATE

            1:37pm 4/12/2019 
                      
 * */
    void Start()
    {
        mainCamera = transform.Find("FPS View").Find("FPS Camera").GetComponent<Camera>();
    }




    /* *

    NAME

          void Update() - Update is called once per frame
                       
    DESCRIPTION

          This function will only call shoot since we are using this to update the shooting 
          animations and updating the scene using RayCasting to display hits on objects

                   
    AUTHOR

            Aayush B Shrestha

    DATE

            1:37pm 4/12/2019  
                      
 * */

    void Update()
    {
        Shoot();
    }


    /* *

    NAME

          public void Shoot() - shoots gun on mouse click
                       
    DESCRIPTION

          This function restricts the rate of fire and plays necessary animations.
          When the player clicks the right mouse button, the shooting animation is triggered. 
          When an object is shot, RayCasting is used to determine the point of impact.
          Once the point of impact is determined, if the point is not an enemy player, the concrete shot animations is played at that point,
          to demonstrate where the shot was fired.
          If the point of impact was an enemy player, the player gets hit and it renders a tiny blood splatter animation.
          This is also used to deal damage to the player.
                  
    Returns
        
        Nothing      
                   
    AUTHOR

            Aayush B Shrestha

    DATE

            1:37pm 4/12/2019  
                      
 * */

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

    /* *

    NAME

          void CmdDealDamage(GameObject obj,Vector3 pos, Vector3 rotation) - Deals damage to the player over the network and gets point of impact.
    
    SYNOPSIS

        void CmdDealDamage(GameObject obj,Vector3 pos, Vector3 rotation)
        GameObject obj   - reference to the object that was just hit
        Vector3 pos      - position of the point of impact.
        Vector3 rotation - Normal from point of impact.</param>
                           
    DESCRIPTION

          This function allows the server to handle dealing damage to the player over the network to make sure all the health values are synced.
          It also uses the point of impact to generate the blood splatter animation since we want to see some feed back when the player is shot. 

        [Command] - Commands are sent from player objects on the client to player objects on the server. This enables to server to recognize updates.

        For security, Commands can only be sent from YOUR player object
        so you cannot control the objects of other players. To make a function into a command, 
        we have to add the [Command] custom attribute to it, and add the “Cmd” prefix. 
        This function will now be run on the server when it is called on the client. Any arguments will automatically be passed to the server with the command.

        Commands functions must have the prefix “Cmd”. 
        This is a hint when reading code that calls the command - this function is special and is not invoked locally like a normal function.


   Returns
        
        Nothing      
                   
    AUTHOR

            Aayush B Shrestha

    DATE

            1:37pm 4/12/2019  
                      
 * */
    [Command] 
    void CmdDealDamage(GameObject obj,Vector3 pos, Vector3 rotation)
    {
        obj.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
        Instantiate(blood_Impact, pos, Quaternion.LookRotation(rotation));
    }


}
