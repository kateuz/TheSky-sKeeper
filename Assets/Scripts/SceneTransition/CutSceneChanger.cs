using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class CutSceneChanger : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;
    public RawImage rawImage;
    public string nextScene;

    private void Start()
    {
        // Hide the RawImage initially
        //rawImage.gameObject.SetActive(false);

        videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
        videoPlayer.Prepare();

        videoPlayer.prepareCompleted += OnVideoPrepare;
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoPrepare(VideoPlayer vp)
    {
        // Show RawImage only when ready
        rawImage.texture = videoPlayer.texture;
        //rawImage.gameObject.SetActive(true);

        videoPlayer.Play();
        audioSource.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextScene);
    }
}
