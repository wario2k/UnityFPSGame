using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
/* 
 * <summary>
/// GameManager Class
/// Will handle end condition of the game 
///     Current Logic: 
///         End Game when either player Health Bar < 1
/// </summary> 
/// */
public class GameManager : MonoBehaviour
{
   
    public void EndGame()
    {
        Debug.Log("Game Has Ended!");
        SceneManager.LoadSceneAsync(2);
    }
}
