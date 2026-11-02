using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform buttonTransform;

    public float hoverScale = 1.1f;
    public float animationSpeed = 8f;

    private Vector3 originalScale;
    private bool isHovered;

    void Start()
    {
        if (buttonTransform == null)
            buttonTransform = GetComponent<RectTransform>();

        originalScale = buttonTransform.localScale;
    }

    void Update()
    {
        Vector3 targetScale = isHovered
            ? originalScale * hoverScale
            : originalScale;

        buttonTransform.localScale = Vector3.Lerp(
            buttonTransform.localScale,
            targetScale,
            Time.unscaledDeltaTime * animationSpeed
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
}
