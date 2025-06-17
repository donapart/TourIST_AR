using UnityEngine;

public class RecognitionManager : MonoBehaviour
{
    [Header("Feature Toggles")]
    public bool objectDetectionActive;
    public bool ocrActive;
    public bool barcodeActive;
    public bool faceDetectionActive;

    public ObjectDetectionManager objectDetectionManager;
    public OCRManager ocrManager;
    public BarcodeScanner barcodeScanner;
    public FaceDetectionManager faceDetectionManager;

    void Start()
    {
        UpdateFeatureStates();
    }

    public void SetObjectDetection(bool active)
    {
        objectDetectionActive = active;
        if (objectDetectionManager != null)
            objectDetectionManager.enabled = active;
    }

    public void SetOCR(bool active)
    {
        ocrActive = active;
        if (ocrManager != null)
            ocrManager.enabled = active;
    }

    public void SetBarcode(bool active)
    {
        barcodeActive = active;
        if (barcodeScanner != null)
            barcodeScanner.enabled = active;
    }

    public void SetFaceDetection(bool active)
    {
        faceDetectionActive = active;
        if (faceDetectionManager != null)
            faceDetectionManager.enabled = active;
    }

    public void UpdateFeatureStates()
    {
        SetObjectDetection(objectDetectionActive);
        SetOCR(ocrActive);
        SetBarcode(barcodeActive);
        SetFaceDetection(faceDetectionActive);
    }
}
