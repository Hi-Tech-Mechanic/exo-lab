using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    public float rotationSpeed = 1;

    private void Update()
    {
        float angle = (this.transform.localRotation.x + 1) * this.rotationSpeed;
        var point = new Vector3(0, 0, 1);
        //var b = new UnityEngine.Quaternion(0, 0, angle, 0);
        this.transform.Rotate(point, angle);
        //this.transform.Rotate(b, Space.Self);// = new UnityEngine.Quaternion(0, 0, angle, 0);// Rotate(Vector3.up, angle, Space.World);
    }
}
