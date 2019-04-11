using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// GameManager Class
/// Will handle end condition of the game 
///     Current Logic: 
///         End Game when either player Health Bar < 1
///     Future Game Logic:
///        Log Each Players Damage 
///         Display at the End of Game
/// </summary>
public class GameManager : MonoBehaviour
{
    private const string END= "EndScene";
    public void EndGame()
    {
        Debug.Log("Game Has Ended!");
        SceneManager.LoadScene(1);
    }
}
