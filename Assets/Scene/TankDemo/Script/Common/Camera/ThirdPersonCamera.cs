using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public float Distance { get; set; }
    public float rotationSpeed { get; set; }
    public Vector2 direction { get; set; }
    void Start()
    {
        Distance = 5;
        rotationSpeed = 2;
        direction = new Vector2();
    }

    void Update()
    {
        int MOUSE_LEFT = 0;
        if(Input.GetMouseButton(MOUSE_LEFT))
        {
            updatePose();
            return;
        }

        float wheelValue = Input.GetAxis("Mouse ScrollWheel");
        if (wheelValue != 0)
        {
            float distance = (transform.position - transform.parent.position).sqrMagnitude;
            Distance -= wheelValue * (distance * Time.deltaTime * 3);
            Distance = Mathf.Min(Distance, 110);
            Distance = Mathf.Max(Distance, 2);
            updatePose();
        }
    }

    public void updatePose()
    {
        float dx = Input.GetAxis("Mouse X");
        float dy = -Input.GetAxis("Mouse Y");
        direction = new Vector2(direction.x + dx * rotationSpeed,
                        direction.y + dy * rotationSpeed);
        Quaternion rotate = new Quaternion();
        rotate = Quaternion.AngleAxis(direction.x, new Vector3(0, 1, 0)) * Quaternion.AngleAxis(direction.y, new Vector3(1, 0, 0));
        Matrix4x4 mat = Matrix4x4.Rotate(rotate);
        Vector3 dir = -mat.GetColumn(2);
        transform.position = transform.parent.position + dir * Distance;
        transform.rotation = rotate;
    }
}
