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
        Debug.Log("CutSceneChanger Start");
        
        // Make sure VideoPlayer is enabled
        if (!videoPlayer.enabled)
        {
            videoPlayer.enabled = true;
        }

        // Set up video with audio
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);
        videoPlayer.frame = 0; // Ensure we start from the first frame
        videoPlayer.Prepare();

        videoPlayer.prepareCompleted += OnVideoPrepare;
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoPrepare(VideoPlayer vp)
    {
        Debug.Log("Video Prepared");
        
        // Make sure RawImage is enabled and visible
        if (!rawImage.gameObject.activeInHierarchy)
        {
            rawImage.gameObject.SetActive(true);
        }
        
        rawImage.texture = videoPlayer.texture;
        videoPlayer.frame = 0; // Double check we're at the start
        videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextScene);
    }

    private void Update()
    {
        // Keep RawImage enabled
        if (!rawImage.gameObject.activeInHierarchy)
        {
            Debug.Log("RawImage was disabled, re-enabling");
            rawImage.gameObject.SetActive(true);
        }
    }
}
