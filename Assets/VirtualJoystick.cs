using UnityEngine;
using UnityEngine.EventSystems; // Required for UI touch detection

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("Settings")]
    public RectTransform joystickHandle; // The inner circle
    public float handleRange = 100f;     // How far the handle can move

    [HideInInspector] public Vector2 inputVector; // We read this from other scripts!

    private Vector2 originalPosition;

    void Start()
    {
        originalPosition = joystickHandle.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        // Calculate where we are touching relative to the joystick background
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            GetComponent<RectTransform>(), 
            eventData.position, 
            eventData.pressEventCamera, 
            out position))
        {
            // Normalize the position (0 to 1)
            position.x = (position.x / GetComponent<RectTransform>().sizeDelta.x);
            position.y = (position.y / GetComponent<RectTransform>().sizeDelta.y);

            // Calculate input vector (Direction)
            inputVector = new Vector2(position.x * 2 - 1, position.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            // Move the handle visually
            joystickHandle.anchoredPosition = new Vector2(
                inputVector.x * (GetComponent<RectTransform>().sizeDelta.x / 2),
                inputVector.y * (GetComponent<RectTransform>().sizeDelta.y / 2)
            );
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData); // Start dragging immediately
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Reset handle when finger is lifted
        inputVector = Vector2.zero;
        joystickHandle.anchoredPosition = originalPosition;
    }
}