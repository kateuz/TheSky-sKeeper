using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipCutScene : MonoBehaviour
{
    public string skipScene;
    void Start()
    {
        //Hide the button for a few seconds
        Invoke("ShowButton", 2f);
    }

    void ShowButton()
    {
        gameObject.SetActive(true);
    }

    public void OnSkipButtonPressed()
    {
        SceneManager.LoadScene(skipScene);
    }
}
