using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    // Start is called before the first frame update
    //set the cursor to lock onto the crosshairs 
  
    void Start()
    {
       Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        ControlCursor();
    }

    /// <summary>
    /// Controls the cursor with mouse interactions
    /// This method locks the cursor with the crosshairs in game.
    /// </summary>
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
