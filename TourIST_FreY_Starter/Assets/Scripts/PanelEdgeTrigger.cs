using UnityEngine;
using UnityEngine.EventSystems;

public class PanelEdgeTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject panel;
    public float animationDuration = 0.3f;

    private Vector2 hiddenPosition;
    private Vector2 shownPosition;
    private RectTransform rect;

    void Awake()
    {
        rect = panel.GetComponent<RectTransform>();
        shownPosition = rect.anchoredPosition;
        hiddenPosition = shownPosition + new Vector2(-rect.rect.width, 0);
        rect.anchoredPosition = hiddenPosition;
        panel.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        panel.SetActive(true);
        StartCoroutine(Slide(true));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(Slide(false));
    }

    IEnumerator Slide(bool show)
    {
        float t = 0f;
        Vector2 start = rect.anchoredPosition;
        Vector2 target = show ? shownPosition : hiddenPosition;
        while (t < animationDuration)
        {
            rect.anchoredPosition = Vector2.Lerp(start, target, t / animationDuration);
            t += Time.deltaTime;
            yield return null;
        }
        rect.anchoredPosition = target;
        if (!show)
            panel.SetActive(false);
    }
}
