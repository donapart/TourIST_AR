using System.Collections;
using UnityEngine;
using ARLocation;

public class MarkerManager : MonoBehaviour
{
    public SurrealDBClient dbClient;
    public GameObject markerPrefab;
    public Transform cameraTransform;

    void Start()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        yield return StartCoroutine(dbClient.LoginREST());
        yield return StartCoroutine(dbClient.GetMarkers(OnMarkersReceived));
    }

    void OnMarkersReceived(SurrealDBClient.MarkerData[] markers)
    {
        if (markers == null || markers.Length == 0)
        {
            markers = SurrealDBClient.LoadMarkerCache();
        }
        else
        {
            SurrealDBClient.SaveMarkerCache(markers);
        }

        foreach (var m in markers)
        {
            GameObject obj = Instantiate(markerPrefab);
            var placeAt = obj.GetComponent<PlaceAtLocation>();
            if (placeAt != null)
            {
                placeAt.Location = new Location
                {
                    Latitude = m.latitude,
                    Longitude = m.longitude,
                    Altitude = 0,
                    AltitudeMode = AltitudeMode.GroundRelative
                };
                placeAt.PlaceAtStart = true;
            }
            var overlay = obj.GetComponentInChildren<MarkerOverlayUI>();
            if (overlay != null)
            {
                overlay.cameraTransform = cameraTransform;
                overlay.markerTransform = obj.transform;
                overlay.SetLabel(m.label);
            }
        }
    }
}
