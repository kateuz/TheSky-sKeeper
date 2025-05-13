using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataScanner : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public MesospherePlayerMovement mesospherePlayerMovement;

    public GameObject learningPanel;
    public GameObject fogPs;

    public void OnScan()
    {

        if(learningPanel.activeSelf && fogPs.activeSelf)
        {
            learningPanel.SetActive(false);
            fogPs.SetActive(false);
            Debug.Log("Toggle Off");

            playerMovement.gravityScale = 4.1f;
            playerMovement.jumpPower = 11;
        }
        else
        {
            learningPanel.SetActive(true);
            fogPs.SetActive(true);
            Debug.Log("Toggle On");

            playerMovement.gravityScale = 1.5f;
            playerMovement.jumpPower = 8;

        }

    }
    //    public float scanRadius = 10f;

    //    public LayerMask dataLogLayer;

    //    public GameObject scanEffectPrefab;

    //    private bool logFound = false;
    //    void Start()
    //    {

    //    }
    //    void Update()
    //    {
    //        Scan();
    //    }

    //    public void Scan()
    //    {
    //        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, scanRadius, dataLogLayer);

    //        logFound = false;

    //        foreach (Collider2D hit in hits)
    //        {
    //            if (hit.CompareTag("DataLog"))
    //            {
    //                Debug.Log("Data Log Acquired!");
    //                hit.GetComponent<DataLogInteraction>().DownloadLog();
    //                logFound = true;
    //                break;
    //            }
    //        }

    //        if (!logFound)
    //        {
    //            Debug.Log("No signs detected :((");
    //            Debug.Log($"Scanning around {transform.position} with radius {scanRadius}");
    //            Debug.Log($"Found {hits.Length} colliders.");
    //        }
    //    }


}

