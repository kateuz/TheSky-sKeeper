using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuScript : MonoBehaviour {
    
    public string mapToLoad;
   public void Play()
    {
        SceneManager.LoadScene(mapToLoad);
    }
    
    public void Quit() {  
        Application.Quit();
    }
}