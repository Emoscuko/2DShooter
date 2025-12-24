using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    // Update is called once per frame
    void LateUpdate()
    {
        // Forces the UI to always face the same direction, regardless of parent rotation
        transform.rotation = Quaternion.identity;
    }
}