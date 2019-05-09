using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* *
   CLASS NAME

          DestroyAfterTime : MonoBehaviour 
                       
    DESCRIPTION

         This is a basic utility class that is used to destroy game objects.
                   
    AUTHOR

            Aayush B Shrestha

    DATE

            2:37pm 2/12/2019  
                       
 * */
public class DestroyAfterTime : MonoBehaviour
{
    private readonly float timer = 1f;

    /* *
       
    NAME

          Start()
                       
    DESCRIPTION

         When called on a game object, it gets destroyed from the scene in the time specified.
                   
    AUTHOR

            Aayush B Shrestha

    DATE

            2:37pm 2/12/2019  
                       
 * */
    void Start()
    {
        Destroy(gameObject, timer); //will destroy the object after timer expires 
    }

}
