using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        //getting current rotation threshold for the rotation object
        originalRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //This function can be called depending on thresholds set in unity 
    void FixedUpdate()
    {

    }

    //called to perform final updates 
    void LateUpdate()
    {
        HandleRotation();
    }

    //this function will place restrictions on the level of rotation allowed 
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

    //this function will be called to maintain desired mouse sensitivity 
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
