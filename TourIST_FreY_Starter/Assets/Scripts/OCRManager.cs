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
        // TODO: Tesseract OCR integration
        yield return null;
    }
}
