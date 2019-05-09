using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* *
   CLASS NAME

          public class WeaponManager : MonoBehaviour
                       
    DESCRIPTION

          This is a utility class that is used in the Player prefab to hold all the weapon game objects. 
          It is seperated into a seperate utility class to make navigation and accessing individual weapons easier,
          not requiring scaning the entire Player prefab, which is saving some time.        

    AUTHOR

            Aayush B Shrestha

    DATE

            12:37pm 5/6/2019  
                     
 * */
public class WeaponManager : MonoBehaviour
{
    //will simply hold all the weapons currently in the game
    public GameObject[] weapons;
   
}
