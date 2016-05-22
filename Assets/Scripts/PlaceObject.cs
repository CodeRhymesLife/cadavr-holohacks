using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class PlaceObject : MonoBehaviour
{
    bool placing = false;

    // Called by GazeGestureManager when the user performs a Select gesture
    public void OnSelect()
    {
        placing = true;
        SpatialMapping.Instance.DrawVisualMeshes = true;
    }
    public void OnDeselect()
    {
        placing = false;
        SpatialMapping.Instance.DrawVisualMeshes = false;
    }
    // Update is called once per frame
    void Update()
    {
        // If the user is in placing mode,
        // update the placement to match the user's gaze.

        if (placing)
        {
            // Do a raycast into the world that will only hit the Spatial Mapping mesh.
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
                30.0f, SpatialMapping.PhysicsRaycastMask))
            {
                // Move this object's parent object to
                // where the raycast hit the Spatial Mapping mesh.
                transform.position = hitInfo.point + hitInfo.normal * 0.1f;
            }
        }
    }
}