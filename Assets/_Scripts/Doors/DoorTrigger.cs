using System.Collections;
using UnityEngine;

public enum DoorType { Top, Bottom, Left, Right }

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private DoorType doorType;
    [SerializeField] private float boundsDisableDuration = 0.5f;

    private CameraController cameraController;

    private void Start()
    {
        if (Camera.main != null)
        {
            cameraController = Camera.main.GetComponent<CameraController>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<ScreenBoundsHandler>(out var boundsHandler))
        {
            StartCoroutine(DisableBoundsRoutine(boundsHandler));

            if (cameraController != null)
            {
                cameraController.MoveScreen(doorType);
            }
        }
    }

    private IEnumerator DisableBoundsRoutine(ScreenBoundsHandler boundsHandler)
    {
        boundsHandler.IsActive = false;
        yield return new WaitForSeconds(boundsDisableDuration);
        boundsHandler.IsActive = true;
    }
}