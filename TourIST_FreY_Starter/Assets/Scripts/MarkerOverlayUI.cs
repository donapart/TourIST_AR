using UnityEngine;
using UnityEngine.UI;

public class MarkerOverlayUI : MonoBehaviour
{
    public Text labelText;
    public Text distanceText;
    public Transform cameraTransform;
    public Transform markerTransform;

    void Update()
    {
        if (cameraTransform && markerTransform)
        {
            transform.position = markerTransform.position + Vector3.up * 0.5f;
            transform.LookAt(cameraTransform);
        }

        if (distanceText && cameraTransform && markerTransform)
        {
            float dist = Vector3.Distance(cameraTransform.position, markerTransform.position);
            distanceText.text = dist.ToString("F1") + "m";
        }
    }

    public void SetLabel(string text)
    {
        if (labelText) labelText.text = text;
    }
}
