using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Attach to any UI Button to get scale + color feedback on hover and click.
/// No setup needed beyond adding this component — it finds the Button automatically.
/// </summary>
[RequireComponent(typeof(Button))]
public class UIFeedbackController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Scale Animation")]
    public float hoverScale  = 1.08f;
    public float pressScale  = 0.93f;
    public float animSpeed   = 12f;

    [Header("Color Tint")]
    public Color hoverColor  = new Color(1f, 0.95f, 0.7f);  // warm yellow
    public Color pressColor  = new Color(0.7f, 0.95f, 0.7f); // soft green
    public Color normalColor = Color.white;

    private Vector3 targetScale;
    private Color targetColor;
    private Graphic graphic;

    void Awake()
    {
        targetScale = Vector3.one;
        graphic     = GetComponent<Graphic>();
        if (graphic != null) targetColor = normalColor;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * animSpeed);
        if (graphic != null)
            graphic.color = Color.Lerp(graphic.color, targetColor, Time.deltaTime * animSpeed);
    }

    public void OnPointerEnter(PointerEventData e)
    {
        targetScale = Vector3.one * hoverScale;
        targetColor = hoverColor;
        AudioManager.Instance?.PlayButtonClick();
    }

    public void OnPointerExit(PointerEventData e)
    {
        targetScale = Vector3.one;
        targetColor = normalColor;
    }

    public void OnPointerDown(PointerEventData e)
    {
        targetScale = Vector3.one * pressScale;
        targetColor = pressColor;
    }

    public void OnPointerUp(PointerEventData e)
    {
        targetScale = Vector3.one * hoverScale;
        targetColor = hoverColor;
    }
}
