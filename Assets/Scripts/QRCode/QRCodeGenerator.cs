using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;
using ZXing.Common;
using UnityEngine.UI;
using TMPro;

public class QRCodeGenerator : MonoBehaviour
{

    [SerializeField]
    private RawImage rawImageReceiver;

    [SerializeField]
    private TMP_InputField textInputField;

    private Texture2D storeEncodedTexture;

    void Start()
    {
        storeEncodedTexture = new Texture2D(256, 256);
    }

    private Color32 [] Encode(string textForEncoding, int width, int height)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions {
                Height = height,
                Width = width
            }
        };

        return writer.Write(textForEncoding);
    }

    public void OnClickEncode()
    {
        EncodeTextToQRCode();
    }

    private void EncodeTextToQRCode()
    {
        string textWrite = string.IsNullOrEmpty(textInputField.text) ? "HEHEHHEHEHEHE" : textInputField.text;

        Color32[] convertPixelToTexture = Encode(textWrite, storeEncodedTexture.width, storeEncodedTexture.height);
        storeEncodedTexture.SetPixels32(convertPixelToTexture);
        storeEncodedTexture.Apply();

        rawImageReceiver.texture = storeEncodedTexture;
    }
}
