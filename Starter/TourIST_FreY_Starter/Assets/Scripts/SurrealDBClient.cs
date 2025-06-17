using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class SurrealDBClient : MonoBehaviour
{
    [Header("SurrealDB Settings")]
    public string restUrl = "https://dein-surreal-endpoint.de/rpc";
    public string webSocketUrl = "wss://dein-surreal-endpoint.de/rpc";
    public string username = "root";
    public string password = "root";
    public string ns = "touristfrey";
    public string db = "arapp";

    private string token;

    [Serializable]
    public class MarkerData
    {
        public string id;
        public string label;
        public double latitude;
        public double longitude;
    }

    [Serializable]
    private class QueryResult
    {
        public MarkerData[] result;
    }

    [Serializable]
    private class MarkerCache
    {
        public MarkerData[] markers;
    }

    private const string MarkerCacheKey = "MarkerCache";

    public IEnumerator LoginREST()
    {
        string json = $"{{"method":"signin","params":[{{"user":"{username}","pass":"{password}"}}]}}";
        using (UnityWebRequest www = new UnityWebRequest(restUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string response = www.downloadHandler.text;
                if (response.Contains("token"))
                {
                    var tokenStart = response.IndexOf("token":"") + 8;
                    var tokenEnd = response.IndexOf(""", tokenStart);
                    token = response.Substring(tokenStart, tokenEnd - tokenStart);
                }
            }
            else
            {
                Debug.LogError("SurrealDB Login Fehler: " + www.error);
            }
        }
    }

    public IEnumerator CreateMarker(string id, string label, double lat, double lon)
    {
        string sql = $"CREATE marker:{id} SET label='{label}', latitude={lat}, longitude={lon}, timestamp=time::now()";
        string json = $"{{"method":"query","params":["{sql}", {{}}]}}";

        using (UnityWebRequest www = new UnityWebRequest(restUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", $"Bearer {token}");
            www.SetRequestHeader("NS", ns);
            www.SetRequestHeader("DB", db);

            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
                Debug.Log("Marker gespeichert: " + www.downloadHandler.text);
            else
                Debug.LogError("SurrealDB Fehler: " + www.error);
        }
    }

    public IEnumerator GetMarkers(Action<MarkerData[]> onSuccess)
    {
        string sql = "SELECT * FROM marker;";
        string json = $"{{"method":"query","params":["{sql}", {{}}]}}";
        using (UnityWebRequest www = new UnityWebRequest(restUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", $"Bearer {token}");
            www.SetRequestHeader("NS", ns);
            www.SetRequestHeader("DB", db);

            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                string response = www.downloadHandler.text;
                Debug.Log("Marker Daten: " + response);
                try
                {
                    string trimmed = response.Trim().TrimStart('[').TrimEnd(']');
                    var result = JsonUtility.FromJson<QueryResult>(trimmed);
                    onSuccess?.Invoke(result.result);
                }
                catch (Exception e)
                {
                    Debug.LogError("Parse Fehler: " + e.Message);
                    onSuccess?.Invoke(null);
                }
            }
            else
            {
                Debug.LogError("SurrealDB Fehler: " + www.error);
                onSuccess?.Invoke(null);
            }
        }
    }

    public static void SaveMarkerCache(MarkerData[] markers)
    {
        var wrapper = new MarkerCache { markers = markers };
        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString(MarkerCacheKey, json);
        PlayerPrefs.Save();
    }

    public static MarkerData[] LoadMarkerCache()
    {
        if (!PlayerPrefs.HasKey(MarkerCacheKey))
            return new MarkerData[0];

        var json = PlayerPrefs.GetString(MarkerCacheKey);
        try
        {
            var wrapper = JsonUtility.FromJson<MarkerCache>(json);
            return wrapper.markers;
        }
        catch
        {
            return new MarkerData[0];
        }
    }
}
