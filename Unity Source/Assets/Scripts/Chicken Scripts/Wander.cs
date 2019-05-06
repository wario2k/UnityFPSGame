using UnityEngine;
using System.Collections;

/// <summary>
/// Creates wandering behaviour for a CharacterController - tested using chicken controller.
/// </summary>
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

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        chickenAnim = GetComponent<Animator>();
        // Set random initial rotation
        heading = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, heading, 0);

        StartCoroutine(NewHeading());
    }

    private void OnCollisionEnter(Collision collision)
    {
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
        var turnAround = transform.TransformDirection(Vector3.back);
        chickenAnim.SetFloat(MOVE, speed);
        controller.SimpleMove(turnAround * speed);
    }

    private void OnCollisionStay(Collision collision)
    {
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
        //turn around
        var turnAround = transform.TransformDirection(Vector3.back);
        chickenAnim.SetFloat(MOVE, speed);
        controller.SimpleMove(turnAround * speed);

    }

    void Update()
    {
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
        var forward = transform.TransformDirection(Vector3.forward);
        chickenAnim.SetFloat(MOVE, speed);
        controller.SimpleMove(forward * speed);
    }

    /// <summary>
    /// Repeatedly calculates a new direction to move towards.
    /// Use this instead of MonoBehaviour.InvokeRepeating so that the interval can be changed at runtime.
    /// </summary>
    IEnumerator NewHeading()
    {
        while (true)
        {
            NewHeadingRoutine();
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    /// <summary>
    /// Calculates a new direction to move towards.
    /// </summary>
    void NewHeadingRoutine()
    {
        var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
        var ceil = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
        heading = Random.Range(floor, ceil);
        targetRotation = new Vector3(0, heading, 0);
    }
}