using UnityEngine;

/* *
   CLASS NAME

          FPSMouseLook : MonoBehavior 
                       
    DESCRIPTION

          This class handles and limits the amount of rotation that the player is allowed to do in game 
          to make sure camera movements are smooth and not choppy and don't exceed 360 degrees in the x-axis and 60 degrees in the y-axis. 
                  

    AUTHOR

            Aayush B Shrestha

    DATE

            2:37pm 3/12/2019  
 * */

public class FPSMouseLook : MonoBehaviour
{

    public enum RotationAxes 
    { 
        MouseX, 
        MouseY
                }

    public RotationAxes axes = RotationAxes.MouseY;

    //variables holding mouse sensitivity in the x and y axes 
    private float currentSensitivity_X = 1.5f;
    private float currentSensitivity_Y = 1.5f;

    //variables to set sensitivity 
    private float sensitivity_X = 1.5f;
    private float sensitivity_Y = 1.5f;

    //variables to control rotation of camera
    private float rotation_X,
                    rotation_Y;

    //limiting maximum and minimum rotation variables in the x and y axis respectively 
    private float minimum_X = -360f;
    private float maximum_X = 360f;

    private float minimum_Y = -60f;
    private float maximum_Y = 60f;

    private Quaternion originalRotation;

    private float mouseSensitivity = 1.7f;

    /*
     NAME

            void OnStart() - Start is called on the frame when a script is enabled just 
                             before any of the Update methods are called the first time. 
                       
    DESCRIPTION

          Using On start to cache the original rotation factor in order to not have to try to collect that information for every update.
                       
    RETURNS

           Nothing.

    AUTHOR

            Aayush B Shrestha

    DATE

            7:37pm 3/12/2019  
     * */


    void Start()
    {
        //getting current rotation threshold for the rotation object
        originalRotation = transform.rotation;
    }


    /**/
    /*
     NAME

            void LateUpdate() - LateUpdate is called after all Update functions have been called. This is useful to order script execution. 
                       
    DESCRIPTION

           The handle rotation function is being called in the late update inorder to prevent 
           the user from performing un-conventional rotations making the game look unrealistic. 
           Limiting the angle of rotation was important in order for the model's integrity to be maintained. 
                       
    RETURNS

           Nothing.

    AUTHOR

            Aayush B Shrestha

    DATE

            6:37pm 3/12/2019  
     * */
    void LateUpdate()
    {
        HandleRotation();
    }


    /**/
    /*
     float ClampAngle(float angle, float minimum, float maximum)
       
    NAME

             float ClampAngle(float angle, float minimum, float maximum) -  this function will place restrictions on the level of rotation allowed  

    SYNOPSIS
            float ClampAngle(float angle, float minimum, float maximum)
                float angle - the angle the use is trying to look in
                float minimum - the minimum degree of rotation that would not cause bad scene interactions.
                float maximum - the maximum degree of rotation that would not cause bad scene interactions.              
    DESCRIPTION

            This value takes in the angle provided and makes sure it is within an acceptable max and min range. 
                       

    RETURNS

           Nothing.

    AUTHOR

            Aayush B Shrestha

    DATE

            6:37pm 3/12/2019

    */

    float ClampAngle(float angle, float minimum, float maximum)
    {

        if(angle < -360f)
        {
            angle += 360f;
        }

        if(angle > 360f)
        {
            angle -= 360f;
        }

        return Mathf.Clamp(angle, minimum, maximum);
    }


    /**/
    /*
    void HandleRotation()
       
    NAME

            void HandleRotation() - this function will be called to maintain desired mouse sensitivity 

    SYNOPSIS
            void HandleRotation()
    DESCRIPTION

            This function checks current mouse sensitivity and tries to regulate how quickly the mouse moves in the scene.
            If the sensitivity is above the specified threshold, it clamps the value down or up to match the sensitivity we want.
            This sensitivity was determined using a lot of trial and error to get the most seemles experience. 
                       

    RETURNS

           Nothing.

    AUTHOR

            Aayush B Shrestha

    DATE

            6:27pm 3/12/2019

    */

    void HandleRotation()
    {
        //check current sensitivity 
        if(currentSensitivity_X != mouseSensitivity || currentSensitivity_Y != mouseSensitivity)
        {
            currentSensitivity_X = mouseSensitivity;
            currentSensitivity_Y = mouseSensitivity;
        }

        sensitivity_X = currentSensitivity_X;
        sensitivity_Y = currentSensitivity_Y;

        //changes only in the x axis
        if(axes == RotationAxes.MouseX)
        {
            //getting movement from the scene 
            rotation_X += Input.GetAxis("Mouse X") * sensitivity_X;
            //applying Clamp to limit under normal thresholds 
            rotation_X = ClampAngle(rotation_X, minimum_X, maximum_X);
            //creating the rotation around axis with angle specified 
            Quaternion xQuaternion = Quaternion.AngleAxis(rotation_X, Vector3.up);
            //applying transformation to the local game object 
            transform.localRotation = originalRotation * xQuaternion;
        }

        if(axes == RotationAxes.MouseY)
        {
            //getting movement from the scene 
            rotation_Y += Input.GetAxis("Mouse Y") * sensitivity_Y;
            //applying Clamp to limit under normal thresholds 
            rotation_Y = ClampAngle(rotation_Y, minimum_Y, maximum_Y);
            //creating the rotation around axis with angle specified
            //providing inverse to normalize camera rotation 
            Quaternion yQuaternion = Quaternion.AngleAxis(-rotation_Y, Vector3.right);
            //applying transformation to the local game object 
            transform.localRotation = originalRotation * yQuaternion;
        }


    }
}
