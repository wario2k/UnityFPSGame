using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/* *
   CLASS NAME

          public class UIManager : MonoBehaviour
                       
    DESCRIPTION

          This class handles all menu items in the game. We want to give the user the ability to load a new game or exit when in any menu.
          This functionality is applied to a gameobject and then invoked in game.         

    AUTHOR

            Aayush B Shrestha

    DATE

            12:37pm 5/6/2019  
                     
 * */

public class UIManager : MonoBehaviour
{

/* *
       
    NAME

          void Start() - initializing time scaling to ensure uniform time distrubiton.

    AUTHOR

            Aayush B Shrestha

    DATE

            12:37pm 5/6/2019   
 * */

    void Start()
    {
        Time.timeScale = 1;
    }

    /* *

        NAME

             public void loadGame() - starts new game.
        
        DESCRIPTION

            Called in menu items when the "Play Game" button is clicked. This will cause the game to reload and gives the 
            player the ability to start a new game.           

        AUTHOR

                Aayush B Shrestha

        DATE

                12:37pm 5/6/2019  
     * */

    public void loadGame()
    {
        SceneManager.LoadScene("IceWorld");
    }


    /* *

       NAME

            public void endGame() - exit the game.

       DESCRIPTION

           Called in menu items when the "Quit" button is clicked. This will cause the game to exit and close the application.

       AUTHOR

               Aayush B Shrestha

       DATE

               12:47pm 5/6/2019  
    * */

    public void endGame()
    {
        Application.Quit();
    }
}
