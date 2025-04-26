using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverScreen : MonoBehaviour
{
    public string restartBtn;
    public string mainMenuBtn;
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(restartBtn);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuBtn);
    }
}
