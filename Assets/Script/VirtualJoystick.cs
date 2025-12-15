using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IDragHandler
{
    [Header("References")]
    public RectTransform background;
    public RectTransform handle;

    [Header("Settings")]
    public float handleRange = 80f;
    public float deadZone = 0.15f;

    public Vector2 InputVector { get; private set; }

    Vector2 startPos;

    void Start()
    {
        startPos = background.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            eventData.position,
            eventData.pressEventCamera,
            out pos
        );

        pos = Vector2.ClampMagnitude(pos, handleRange);
        handle.anchoredPosition = pos;

        InputVector = pos / handleRange;
        if (InputVector.magnitude < deadZone)
            InputVector = Vector2.zero;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handle.anchoredPosition = Vector2.zero;
        InputVector = Vector2.zero;
    }
}
