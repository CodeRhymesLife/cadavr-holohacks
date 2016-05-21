using UnityEngine;
using System.Collections;

public class Catheter : MonoBehaviour {

	public void MoveUp()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
    }

    public void MoveDown()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
    }

    public void RotateLeft()
    {
        transform.Rotate(Vector3.left * 180);
    }

    public void RotateRight()
    {
        transform.Rotate(Vector3.right * 180);
    }
}
