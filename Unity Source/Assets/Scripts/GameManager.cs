using UnityEngine;
using UnityEngine.SceneManagement;


/* *
   CLASS NAME

          public class GameManager : MonoBehaviour
                       
    DESCRIPTION

          This is a utility class that is utilized by the Server in game to determine when to end the game.
          This was seperated into a seperate class in order for it to be invokable by gameobjects, without requiring an EventSystem.  

    AUTHOR

            Aayush B Shrestha

    DATE

            12:37pm 5/6/2019  
                     
 * */
public class GameManager : MonoBehaviour
{
    /* *

       NAME

              public void EndGame() - Loads the end game scene. 

       DESCRIPTION

           This method is being called by the server game object on the player when another player dies or disconnects from the game.
           Since the game is only played between two player, when the other player dies or disconnects, this method is called 
           to load the win screen, which will allow the player to either quit or start a new server for the game.          


       Returns

           Nothing      

       AUTHOR

               Aayush B Shrestha

       DATE

               3:37pm 4/13/2019  

    * */

    public void EndGame()
    {
        Debug.Log("Game Has Ended!");
        SceneManager.LoadSceneAsync(2);
    }
}
