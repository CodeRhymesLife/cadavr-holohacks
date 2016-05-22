using UnityEngine;
using UnityEngine.VR.WSA.Input;

public GameObject heart;

public class GazeGestureManager : MonoBehaviour
{
    public static int CollisionLayerMask = 1 << 8;

    public static GazeGestureManager Instance { get; private set; }

    // Represents the hologram that is currently being gazed at.
    public GameObject FocusedObject { get; private set; }
    public GameObject SelectedObject { get; private set; }
    public RaycastHit LastHitInfo { get; set; }

    GestureRecognizer recognizer;

    // Use this for initialization
    void Start()
    {
        Instance = this;

        // Set up a GestureRecognizer to detect Select gestures.
        recognizer = new GestureRecognizer();
        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            GameObject oldSelectedObject = SelectedObject;

            if (oldSelectedObject != null)
            {
                SelectedObject = null;
                oldSelectedObject.SendMessageUpwards("OnDeselect");

                recognizer.CancelGestures();
                recognizer.StartCapturingGestures();
            }

            if (FocusedObject != null && FocusedObject != oldSelectedObject)
            {
                SelectedObject = FocusedObject;
                SelectedObject.SendMessageUpwards("OnSelect");
            }
        };
        recognizer.StartCapturingGestures();
    }

    // Update is called once per frame
    void Update()
    {
        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo, 30.0f, CollisionLayerMask))
        {
            LastHitInfo = hitInfo;

            // If the raycast hit a hologram, use that as the focused object.
            FocusedObject = LastHitInfo.collider.gameObject;
        }
        else
        {
            // If the raycast did not hit a hologram, clear the focused object.
            FocusedObject = null;
        }
    }
}