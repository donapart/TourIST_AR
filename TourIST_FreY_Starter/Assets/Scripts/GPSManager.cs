using UnityEngine;
using ARLocation; // Namespace aus Appoly-Plugin!

public class GPSManager : MonoBehaviour
{
    public static GPSManager Instance { get; private set; }
    public LocationProvider locationProvider;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public Vector2 GetCurrentGPS()
    {
        if (locationProvider == null)
            locationProvider = FindObjectOfType<LocationProvider>();

        var loc = locationProvider.CurrentLocation;
        return new Vector2((float)loc.Latitude, (float)loc.Longitude);
    }

    public float GetCurrentCompass()
    {
        return Input.compass.trueHeading;
    }
}
