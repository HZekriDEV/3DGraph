using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Camera camera;

    Vector3 prevPos;

    float distance;

    void Update()
    {
        camera.transform.Translate(new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel")));

        if (Input.GetMouseButtonDown(0))
        {
            prevPos = camera.ScreenToViewportPoint(Input.mousePosition);
            distance = Vector3.Distance(camera.transform.position, Vector3.zero);
        }

        if(Input.GetMouseButton(0))
        {
            Vector3 direction = prevPos - camera.ScreenToViewportPoint(Input.mousePosition);

            camera.transform.position = new Vector3();

            camera.transform.Rotate(new Vector3(1, 0, 0), direction.y * 180);
            camera.transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180, Space.World);
            camera.transform.Translate(new Vector3(0, 0, -distance));

            prevPos = camera.ScreenToViewportPoint(Input.mousePosition);
        }
    }
}
