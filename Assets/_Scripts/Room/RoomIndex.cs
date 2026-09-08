using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class RoomIndex : MonoBehaviour
{
    public string markerText = "Spawn Point";
    public Color textColor = Color.yellow;

#if UNITY_EDITOR
    // Cached GUIStyle, created once and reused for all RoomIndex instances
    // to avoid per-frame allocations (GC pressure) in the Scene view.
    private static GUIStyle _style;

    private void OnDrawGizmos()
    {
        SceneView sv = SceneView.currentDrawingSceneView;
        if (sv != null && sv.camera != null)
        {
            Camera cam = sv.camera;

            // Distance-based culling: skip drawing the label if the object
            // is too far from the Scene view camera. Note: for orthographic
            // cameras, distance doesn't reflect zoom level as accurately as
            // orthographicSize does, so adjust this check if needed for your scale.
            float dist = Vector3.Distance(cam.transform.position, transform.position);
            if (dist > 60f) return;
        }

        // Lazily initialize the shared style only once
        if (_style == null)
        {
            _style = new GUIStyle();
            _style.fontSize = 12;
            _style.fontStyle = FontStyle.Bold;
        }

        // Color is set per-object since the style itself is shared
        _style.normal.textColor = textColor;

        Handles.Label(transform.position + Vector3.up * 0.5f, markerText, _style);
    }
#endif
}