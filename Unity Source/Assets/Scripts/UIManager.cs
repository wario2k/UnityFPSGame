using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/**
 * Description : Basic Main menu options to load into game screen
 * */

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }


    public void loadGame()
    {
        Debug.Log("Game Being triggered");
        SceneManager.LoadScene("IceWorld");
    }

    //date : may 6 2019 - 12:28pm
    public void endGame()
    {
        Application.Quit();
    }
}
