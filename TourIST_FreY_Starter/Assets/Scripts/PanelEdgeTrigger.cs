using UnityEngine;
using UnityEngine.EventSystems;

public class PanelEdgeTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject panel;
    public float animationDuration = 0.3f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        panel.SetActive(true);
        // TODO: Play slide-in animation via Animator or Lerp
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // TODO: Play slide-out animation, then deactivate
        panel.SetActive(false);
    }
}
