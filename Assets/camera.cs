using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform target; // Player's Transform
    public float smoothSpeed = 0.125f;
    public Vector3 offset; // Offset from the player

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}
