using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ZXing;
using ZXing.QrCode;
using ZXing.Common;
using TMPro;

public class MesosphereDataScanner : MonoBehaviour
{
    [Header("References")]
    public MesospherePlayerMovement playerMovement;
    public GameObject scanButton;
    public string cutsceneSceneName;
    public RawImage qrCodeDisplay;
    public float qrDisplayDuration = 2f;

    [Header("QR Code Settings")]
    [Tooltip("The URL that the QR code will redirect to")]
    public string redirectUrl = "https://drive.google.com/file/d/149nGyY4pS5GjB7ED6ROjQx2lMlnsWeVZ/view?usp=drive_link";
    //[Tooltip("Additional data to include in the QR code")]
    //public string additionalData = "";

    private bool isPlayerInRange = false;
    private Texture2D qrCodeTexture;
    private bool isScanning = false;

    private void Start()
    {
        // Ensure scan button is hidden at start
        if (scanButton != null)
        {
            scanButton.SetActive(false);
        }

        // Initialize QR code texture
        qrCodeTexture = new Texture2D(256, 256);
        if (qrCodeDisplay != null)
        {
            qrCodeDisplay.gameObject.SetActive(false);
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
        if (isPlayerInRange && !isScanning)
        {
            StartCoroutine(ScanSequence());
        }
    }

    private IEnumerator ScanSequence()
    {
        isScanning = true;

        // Generate QR code with URL
        string qrData = redirectUrl;
        //if (!string.IsNullOrEmpty(additionalData))
        //{
        //    qrData += "?data=" + System.Uri.EscapeDataString(additionalData);
        //}
        GenerateQRCode(qrData);

        // Show QR code
        if (qrCodeDisplay != null)
        {
            qrCodeDisplay.gameObject.SetActive(true);
        }

        // Wait for display duration
        yield return new WaitForSeconds(qrDisplayDuration);

        // Hide QR code
        if (qrCodeDisplay != null)
        {
            qrCodeDisplay.gameObject.SetActive(false);
        }

        SaveData();

        // Transition to cutscene
        SceneManager.LoadScene(cutsceneSceneName);

        isScanning = false;
    }

    private void GenerateQRCode(string text)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = qrCodeTexture.height,
                Width = qrCodeTexture.width,
                CharacterSet = "UTF-8",
                ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.H
            }
        };

        Color32[] pixels = writer.Write(text);
        qrCodeTexture.SetPixels32(pixels);
        qrCodeTexture.Apply();

        if (qrCodeDisplay != null)
        {
            qrCodeDisplay.texture = qrCodeTexture;
        }
    }

    private void SaveData()
    {
        // Add your data saving logic here
        Debug.Log("Data saved successfully!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
