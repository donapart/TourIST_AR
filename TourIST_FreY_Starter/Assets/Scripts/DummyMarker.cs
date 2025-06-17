using UnityEngine;
using ARLocation;

public class DummyMarker : MonoBehaviour
{
    public string markerId = "frey-demo";
    public string label = "Museum FreY";
    public double latitude = 48.137154;
    public double longitude = 11.576124;

    void Start()
    {
        var placeAtLocation = GetComponent<PlaceAtLocation>();
        if (placeAtLocation != null)
        {
            placeAtLocation.Location = new Location()
            {
                Latitude = latitude,
                Longitude = longitude,
                Altitude = 0,
                AltitudeMode = AltitudeMode.GroundRelative
            };
            placeAtLocation.PlaceAtStart = true;
        }
    }
}
