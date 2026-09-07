using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    public void MoveScreen(DoorType direction)
    {
        float cameraHeight = cam.orthographicSize * 2f;
        float cameraWidth = cameraHeight * cam.aspect;

        Vector3 targetPos = transform.position;

        switch (direction)
        {
            case DoorType.Top: targetPos.y += cameraHeight; break;
            case DoorType.Bottom: targetPos.y -= cameraHeight; break;
            case DoorType.Left: targetPos.x -= cameraWidth; break;
            case DoorType.Right: targetPos.x += cameraWidth; break;
        }

        transform.position = targetPos;

        // Force camera matrix to update immediately so frustum planes update in the exact same frame
        cam.ResetWorldToCameraMatrix();
    }
}