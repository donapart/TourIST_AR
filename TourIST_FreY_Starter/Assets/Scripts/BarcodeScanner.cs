using UnityEngine;
using ZXing;

public class BarcodeScanner : MonoBehaviour
{
    private IBarcodeReader barcodeReader = new BarcodeReader();

    public void ScanBarcode(Texture2D cameraTexture)
    {
        var result = barcodeReader.Decode(cameraTexture.GetPixels32(),
                                          cameraTexture.width, cameraTexture.height);
        if (result != null)
        {
            Debug.Log("Barcode/QR gefunden: " + result.Text);
        }
    }
}
