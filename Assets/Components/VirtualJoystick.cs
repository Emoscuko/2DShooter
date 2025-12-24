using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("Settings")]
    public RectTransform joystickHandle; // The inner circle

    [HideInInspector] public Vector2 inputVector; // Output direction

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

        // 1. Convert Screen Touch to Local UI Coordinates
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBackground,
            eventData.position,
            eventData.pressEventCamera,
            out position))
        {
            // 2. Calculate offsets relative to size (Fixing the Center Pivot issue)
            // If the background is 200 wide, the radius is 100.
            // We divide position by radius to get a value between -1 and 1.
            position.x = (position.x / joystickBackground.sizeDelta.x) * 2;
            position.y = (position.y / joystickBackground.sizeDelta.y) * 2;

            // 3. Output the Input Vector
            inputVector = new Vector2(position.x, position.y);

            // Normalize so diagonal isn't faster (limit length to 1)
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            // 4. Move the Visual Handle
            joystickHandle.anchoredPosition = new Vector2(
                inputVector.x * (joystickBackground.sizeDelta.x / 2),
                inputVector.y * (joystickBackground.sizeDelta.y / 2)
            );
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        joystickHandle.anchoredPosition = originalPosition;
    }
}