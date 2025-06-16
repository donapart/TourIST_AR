using UnityEngine;

public class PanelEdgeTrigger : MonoBehaviour
{
    public string panelName;

    void OnTriggerEnter(Collider other)
    {
        PanelManager.Instance.ShowPanel(panelName);
    }

    void OnTriggerExit(Collider other)
    {
        PanelManager.Instance.HidePanel(panelName);
    }
}
