using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ImpressumPanel : MonoBehaviour
{
    public GameObject impressumPanel;
    public Text impressumText;

    void Start()
    {
#if UNITY_EDITOR
        var path = Application.dataPath + "/../Impressum.txt";
#else
        var path = Application.persistentDataPath + "/Impressum.txt";
#endif
        if (File.Exists(path))
            impressumText.text = File.ReadAllText(path);
        else
            impressumText.text = "Impressum nicht gefunden.";
    }

    public void ShowPanel()
    {
        impressumPanel.SetActive(true);
    }

    public void HidePanel()
    {
        impressumPanel.SetActive(false);
    }
}
