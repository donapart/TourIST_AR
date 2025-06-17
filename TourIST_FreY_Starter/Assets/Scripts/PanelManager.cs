using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public GameObject[] panels;
    private int activePanel = -1;

    public void ShowPanel(int index)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == index);
        }
        activePanel = index;
    }

    public void HideAllPanels()
    {
        foreach (var panel in panels)
            panel.SetActive(false);
        activePanel = -1;
    }
}
