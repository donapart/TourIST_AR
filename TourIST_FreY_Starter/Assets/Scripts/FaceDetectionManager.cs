using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARFaceManager))]
public class FaceDetectionManager : MonoBehaviour
{
    private ARFaceManager faceManager;

    void Awake()
    {
        faceManager = GetComponent<ARFaceManager>();
    }

    void OnEnable()
    {
        faceManager.facesChanged += OnFacesChanged;
    }

    void OnDisable()
    {
        faceManager.facesChanged -= OnFacesChanged;
    }

    private void OnFacesChanged(ARFacesChangedEventArgs args)
    {
        foreach (var face in args.added)
        {
            Debug.Log("Face detected: " + face.trackableId);
        }
    }
}
