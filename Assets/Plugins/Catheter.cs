using UnityEngine;
using System.Collections;

public class Catheter : MonoBehaviour {

    public static bool RotateLeft { get; set; }
    public static bool RotateRight { get; set; }
    public static bool MoveUp { get; set; }
    public static bool MoveDown { get; set; }

    public void Update()
    {
        if (RotateRight)
            transform.Rotate(0, 1, 0);

        if (RotateLeft)
            transform.Rotate(0, -1, 0);

        if (MoveUp)
            transform.position = new Vector3(transform.position.x - 0.001f, transform.position.y, transform.position.z);

        if (MoveDown)
            transform.position = new Vector3(transform.position.x + 0.001f, transform.position.y, transform.position.z);
    }
}
