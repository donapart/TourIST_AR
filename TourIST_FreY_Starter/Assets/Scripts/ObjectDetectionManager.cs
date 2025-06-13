using UnityEngine;

public class ObjectDetectionManager : MonoBehaviour
{
    public bool debugDraw = true;

    void Update()
    {
        // Farberkennung pseudocode
        /*
        Color32[] pixels = cameraTexture.GetPixels32();
        for (int i = 0; i < pixels.Length; i++) {
            var px = pixels[i];
            if (px.r > 200 && px.g < 50 && px.b < 50) {
                // DrawBoundingBox(...)
            }
            if (px.g > 200 && px.r < 50 && px.b < 50) {
                // DrawBoundingBox(...)
            }
        }
        */
    }
}
