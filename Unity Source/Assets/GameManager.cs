using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
/* 
 * <summary>
/// GameManager Class
/// Will handle end condition of the game 
///     Current Logic: 
///         End Game when either player Health Bar < one
/// </summary> 
/// */
public class GameManager : MonoBehaviour
{
    private const string END= "EndScene";
    public void EndGame()
    {
        Debug.Log("Game Has Ended!");
        SceneManager.LoadSceneAsync(1);
    }

    //to be used as a corutine whenever a change scene is required 
    IEnumerator loadNewScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(1);
    }



}
