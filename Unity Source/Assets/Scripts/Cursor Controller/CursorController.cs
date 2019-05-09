using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* *
   CLASS NAME

         CursorController : MonoBehaviour
                       
    DESCRIPTION

         This is class handles Cursor interaction in game. When playing the game we do not want to have the cross-hairs and a cursor, 
         this class enables functionality to lock the crosshairs to make game-play look better.       
                   
    AUTHOR

            Aayush B Shrestha

    DATE

            7:37pm 4/02/2019  
                       
 * */
public class CursorController : MonoBehaviour
{


    /* *

                NAME

                     void Start()

                DESCRIPTION

                    Start is called before the first frame update and that is the perfect time to set the cursor to lock onto the crosshairs      

                RETURNS

                     Nothing

                AUTHOR

                     Aayush B Shrestha

                DATE

                     12:37pm 5/6/2019  

    * */

    void Start()
    {
       Cursor.lockState = CursorLockMode.None;
    }

    /* *

                NAME

                     void Update()
                     
                RETURNS

                    Nothing

                DESCRIPTION

                    The cursor controller is called at every frame update to make sure we unlock the cursor when the user wants.  

                AUTHOR

                        Aayush B Shrestha

                DATE

                        12:37pm 5/6/2019  

    * */

    void Update()
    {
        ControlCursor();
    }


    /* *

                NAME

                     void ControlCursor()

                DESCRIPTION

                    Controls the cursor with mouse interactions
                    This method locks the cursor with the crosshairs in game.
                    If the cursor is locked the user can press "Tab" to unlock it and "Tab" again to lock it.

                RETURNS

                    Nothing

                AUTHOR

                        Aayush B Shrestha

                DATE

                        12:37pm 5/6/2019  

    * */

    void ControlCursor()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {//if cursor is locked -> unlock it if tab is pressed 
            if(Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else //lock the cursor 
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
