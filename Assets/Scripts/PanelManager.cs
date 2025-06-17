using UnityEngine;
using System.Collections.Generic;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance { get; private set; }

    private Dictionary<string, GameObject> panels = new Dictionary<string, GameObject>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterPanel(string name, GameObject panel)
    {
        panels[name] = panel;
    }

    public void ShowPanel(string name)
    {
        if (panels.TryGetValue(name, out var panel))
        {
            panel.SetActive(true);
        }
    }

    public void HidePanel(string name)
    {
        if (panels.TryGetValue(name, out var panel))
        {
            panel.SetActive(false);
        }
    }
}
