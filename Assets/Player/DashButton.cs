using UnityEngine;
using UnityEngine.EventSystems; // Required for handling touch/clicks

public class DashButton : MonoBehaviour, IPointerDownHandler
{
    public PlayerMovement player; // Reference to your player

    // This function runs immediately when the button is pressed
    public void OnPointerDown(PointerEventData eventData)
    {
        if (player != null)
        {
            player.AttemptDash();
        }
    }
}