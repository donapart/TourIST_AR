using UnityEngine;
using System.Collections;

public class OCRManager : MonoBehaviour
{
    public float ocrInterval = 5f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= ocrInterval)
        {
            StartCoroutine(RunOCR());
            timer = 0f;
        }
    }

    IEnumerator RunOCR()
    {
        // Capture current frame as Texture2D
        yield return new WaitForEndOfFrame();
        Texture2D screenshot = ScreenCapture.CaptureScreenshotAsTexture();

#if USE_TESSERACT
        // Path to the "tessdata" directory must exist in StreamingAssets
        string dataPath = System.IO.Path.Combine(Application.streamingAssetsPath, "tessdata");

        using (var engine = new Tesseract.TesseractEngine(dataPath, "eng"))
        {
            var pix = Tesseract.Pix.LoadFromMemory(screenshot.EncodeToPNG());
            using (var page = engine.Process(pix))
            {
                Debug.Log("OCR result: " + page.GetText());
            }
        }
#else
        // Fallback if Tesseract library is not available
        Debug.LogWarning("Tesseract engine not installed - OCR skipped.");
#endif

        Destroy(screenshot);
    }
}
