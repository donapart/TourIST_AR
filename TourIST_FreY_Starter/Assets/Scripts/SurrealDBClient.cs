using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using WebSocketSharp;

/// <summary>
/// Simple SurrealDB client supporting REST and WebSocket communication.
/// </summary>

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
    private WebSocket ws;
    private bool reconnect = true;
    public float reconnectDelay = 5f;

    [System.Serializable]
    public class MarkerData
    {
        public string id;
        public string label;
        public double latitude;
        public double longitude;
    }

    public delegate void MarkerUpdateHandler(MarkerData data);
    public event MarkerUpdateHandler OnMarkerUpdate;

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
        string json = $"{{\"method\":\"query\",\"params\":[\"{sql}\", {{}}]}}";
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

    // ---------------- WebSocket section -----------------
    public void ConnectWebSocket()
    {
        if (ws != null && ws.IsAlive) return;
        ws = new WebSocket(webSocketUrl);
        ws.OnOpen += (s, e) =>
        {
            Debug.Log("WS connected");
            AuthenticateWebSocket();
        };
        ws.OnMessage += HandleWebSocketMessage;
        ws.OnError += (s, e) => Debug.LogError("WS error: " + e.Message);
        ws.OnClose += (s, e) =>
        {
            Debug.LogWarning("WS closed: " + e.Reason);
            if (reconnect) StartCoroutine(ReconnectWebSocket());
        };
        ws.ConnectAsync();
    }

    private void AuthenticateWebSocket()
    {
        string login = $"{{\"id\":1,\"method\":\"signin\",\"params\":[{{\"user\":\"{username}\",\"pass\":\"{password}\"}}]}}";
        ws.Send(login);
        string use = $"{{\"id\":2,\"method\":\"use\",\"params\":[\"{ns}\",\"{db}\"]}}";
        ws.Send(use);
        string live = "{\"id\":3,\"method\":\"live\",\"params\":[\"SELECT * FROM marker\"]}";
        ws.Send(live);
    }

    private void HandleWebSocketMessage(object sender, MessageEventArgs e)
    {
        Debug.Log("WS msg: " + e.Data);
        if (!e.IsText || !e.Data.Contains("marker")) return;
        try
        {
            var data = JsonUtility.FromJson<MarkerData>(e.Data);
            if (data != null) OnMarkerUpdate?.Invoke(data);
        }
        catch
        {
            string id = Extract(e.Data, "id":"", "\"");
            double.TryParse(Extract(e.Data, "latitude":, ","), out double lat);
            double.TryParse(Extract(e.Data, "longitude":, ","), out double lon);
            string label = Extract(e.Data, "label":"", "\"");
            if (!string.IsNullOrEmpty(id))
                OnMarkerUpdate?.Invoke(new MarkerData { id = id, label = label, latitude = lat, longitude = lon });
        }
    }

    private IEnumerator ReconnectWebSocket()
    {
        yield return new WaitForSeconds(reconnectDelay);
        ConnectWebSocket();
    }

    private string Extract(string source, string startToken, string endToken)
    {
        int start = source.IndexOf(startToken);
        if (start == -1) return string.Empty;
        start += startToken.Length;
        int end = source.IndexOf(endToken, start);
        if (end == -1) end = source.Length;
        return source.Substring(start, end - start);
    }
}
