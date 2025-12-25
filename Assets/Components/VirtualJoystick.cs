using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("Settings")]
    public RectTransform joystickHandle;

    [Header("Output")]
    public bool isPressed; // <--- The boolean you requested
    [HideInInspector] public Vector2 inputVector;

    private Vector2 originalPosition;
    private RectTransform joystickBackground;

    void Start()
    {
        joystickBackground = GetComponent<RectTransform>();
        originalPosition = joystickHandle.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBackground,
            eventData.position,
            eventData.pressEventCamera,
            out position))
        {
            position.x = (position.x / joystickBackground.sizeDelta.x) * 2;
            position.y = (position.y / joystickBackground.sizeDelta.y) * 2;

            inputVector = new Vector2(position.x, position.y);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            joystickHandle.anchoredPosition = new Vector2(
                inputVector.x * (joystickBackground.sizeDelta.x / 2),
                inputVector.y * (joystickBackground.sizeDelta.y / 2)
            );
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true; // Set to true when touched
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false; // Set to false when released
        inputVector = Vector2.zero;
        joystickHandle.anchoredPosition = originalPosition;
    }
}