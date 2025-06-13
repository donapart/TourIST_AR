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

    public IEnumerator GetMarkers()
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
                Debug.Log("Marker Daten: " + www.downloadHandler.text);
            else
                Debug.LogError("SurrealDB Fehler: " + www.error);
        }
    }
}
