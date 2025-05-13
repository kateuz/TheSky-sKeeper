using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MesosphereDataScanner : MonoBehaviour
{
    [Header("References")]
    public MesospherePlayerMovement playerMovement;
    public GameObject scanButton;
    public string cutsceneSceneName;

    private bool isPlayerInRange = false;

    private void Start()
    {
        // Ensure scan button is hidden at start
        if (scanButton != null)
        {
            scanButton.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Killian"))
        {
            isPlayerInRange = true;
            if (scanButton != null)
            {
                scanButton.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Killian"))
        {
            isPlayerInRange = false;
            if (scanButton != null)
            {
                scanButton.SetActive(false);
            }
        }
    }

    public void OnScan()
    {
        if (isPlayerInRange)
        {
            // Load the cutscene scene
            SceneManager.LoadScene(cutsceneSceneName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
