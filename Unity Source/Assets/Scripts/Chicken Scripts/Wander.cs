using UnityEngine;
using System.Collections;

/* *

         CLASS NAME

              public class Wander : MonoBehaviour - Basic Wander AI

         DESCRIPTION

             This class is applied on the chicken UI component in the game. It tries to give the 

         AUTHOR

                 Aayush B Shrestha

         DATE

                 3:47pm 5/1/2019  

      * */

//since this is to be applied on a character controller we need a CharacterController for it to function as intended.
[RequireComponent(typeof(CharacterController))] 
public class Wander : MonoBehaviour
{

    private const string MOVE = "moveSpeed";

    public float speed = 3.5f;
    public float directionChangeInterval = 2;
    public float maxHeadingChange = 30;

    CharacterController controller;
    Animator chickenAnim;
    float heading;
    Vector3 targetRotation;

    /* *

         NAME

              void Awake() - cahcing necessary references 

         DESCRIPTION

             This function is acquring necessary references to the character and animation controller for 
             the chicken agent in the game. 
             It is also initializing the random direction the chicken will start moving in once the scene is loaded.

         AUTHOR

                 Aayush B Shrestha

         DATE

                 4:47pm 5/1/2019  

      * */

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        chickenAnim = GetComponent<Animator>();
        // Set random initial rotation
        heading = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, heading, 0);

        StartCoroutine(NewHeading());
    }

    /* *

        NAME

             private void OnCollisionEnter(Collision collision) - detects collision and changes direction
        
        SYNOPSIS

            private void OnCollisionEnter(Collision collision) 
                Collision collision - Describes the collision encountered

        DESCRIPTION

            This function is invoked when the chicken collides against a mesh agent in the scene.
            If a collision is detected, turn in the opposite direction and start moving there. 

        AUTHOR

                Aayush B Shrestha

        DATE

                5:47pm 5/1/2019  

     * */

    private void OnCollisionEnter(Collision collision)
    {
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
        var turnAround = transform.TransformDirection(Vector3.back);
        chickenAnim.SetFloat(MOVE, speed);
        controller.SimpleMove(turnAround * speed);
    }

    /* *

       NAME

            private void OnCollisionStay(Collision collision) - detects continuous collision

        SYNOPSIS

            private void OnCollisionStay(Collision collision) 
                Collision collision - Describes the collision encountered

       DESCRIPTION

           This function is invoked when the chicken keeps colliding against a mesh agent in the scene.
           This generally tends to happen when the chicken is trapped in a particular part of the map.
           It then tries to find a random direction to turn in so that it can escape.          

       AUTHOR

               Aayush B Shrestha

       DATE

               6;47pm 5/1/2019  

    * */

    private void OnCollisionStay(Collision collision)
    {
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
         //turn to a random direction
        heading = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, heading, 0);

        StartCoroutine(NewHeading());

    }


    /* *

       NAME

            void Update() - called once per frame to update movement of entity

       DESCRIPTION

           This function invokes the movement for the chicken. The trigger for the chicken to play animations is also set in this method.
           The chicken continues to move around the scene until the game ends.

       AUTHOR

               Aayush B Shrestha

       DATE

               6;47pm 5/1/2019  

    * */

    void Update()
    {
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
        var forward = transform.TransformDirection(Vector3.forward);
        chickenAnim.SetFloat(MOVE, speed);
        controller.SimpleMove(forward * speed);
    }

     /* *

       NAME

            IEnumerator NewHeading() - called to determine new direction

       DESCRIPTION

           This function waits until it is time for the chicken to move in a random direction again.
           If true it will invoke the method to determine new direction to move in.          
           This is being used instead of MonoBehaviour.InvokeRepeating so that the interval can be changed at runtime.

       AUTHOR

               Aayush B Shrestha

       DATE

               9:57pm 5/2/2019  

    * */

    IEnumerator NewHeading()
    {
        while (true)
        {
            NewHeadingRoutine();
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }


    /* *

       NAME

            void NewHeadingRoutine() - compute direction to move in

       DESCRIPTION

           This is a utility function that is used to randomly generate a new direction for the chicken to move in.
           It uses the max range of direction change and generates a new direction for the chicken to move towards.           

       AUTHOR

               Aayush B Shrestha

       DATE

               9:57pm 5/2/2019  

    * */

    void NewHeadingRoutine()
    {
        var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
        var ceil = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
        heading = Random.Range(floor, ceil);
        targetRotation = new Vector3(0, heading, 0);
    }
}