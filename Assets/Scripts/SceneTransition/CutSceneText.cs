using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CutSceneText : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public string nextScene;    
    void Start()
    {
        playableDirector.stopped += OnTimeLineDone;
    }

    void OnTimeLineDone(PlayableDirector director)
    {
        SceneManager.LoadScene(nextScene);
    }
}
