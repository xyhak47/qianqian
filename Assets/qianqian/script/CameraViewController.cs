using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewController : MonoBehaviour
{
    public static CameraViewController Instance = null;
    CameraViewController()
    {
        Instance = this;
    }

    private Transform target;


    public bool InZoomMode = false;

    private Vector3 zoom_direction;

    // Start is called before the first frame update
    void Start()
    {
        target = ModelController.Instance.model.transform;

        zoom_direction = ModelController.Instance.GetModelHead().transform.position - transform.position;
        zoom_direction.Normalize();

        Debug.Log(zoom_direction);
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        camerarotate();
        camerazoom();
#endif
    }


    private void camerarotate() 
    {
        var mouse_x = Input.GetAxis("Mouse X");
        var mouse_y = -Input.GetAxis("Mouse Y");
        if (Input.GetKey(KeyCode.Mouse1))
        {
            MoveY(mouse_y);
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            RotateAroundX(mouse_x);

            RotateAroundY(mouse_y);
        }
    }

    private void camerazoom() 
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Zoom(true, 5f);
        }


        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Zoom(false, 5f);
        }
    }

    public void RotateAroundY(float y)
    {
        float roateSpeed = 5f;
        float rotatedAngle = transform.eulerAngles.x + y * roateSpeed;
        float angle_min = 5f, angle_max = 20f;

        if (rotatedAngle <= angle_min)
        {
            transform.RotateAround(target.transform.position, transform.right, (y * roateSpeed) + (angle_min - rotatedAngle));
        }
        else if (rotatedAngle >= angle_max)
        {
            transform.RotateAround(target.transform.position, transform.right, (y * roateSpeed) - (rotatedAngle - angle_max));
        }
        else
        {
            transform.RotateAround(target.transform.position, transform.right, y * roateSpeed);
        }
    }

    public void RotateAroundX(float x)
    {
        float speed = 5f;
        transform.RotateAround(target.transform.position, Vector3.up, x * speed);
    }

    public void MoveY(float delta_y)
    {
        float x = transform.position.x;
        float z = transform.position.z;
        transform.Translate(Vector3.up * (delta_y * 50f) * Time.deltaTime);

        float y = transform.position.y;
        float min = -25f, max = 10.94f;
        y = Mathf.Clamp(y, min, max);
        transform.position = new Vector3(x, y, z);
    }

    public void MoveX(float x)
    {
        transform.Translate(Vector3.left * (x * 50f) * Time.deltaTime);
    }

    public void Zoom(bool In, float delta)
    {
        delta *= In ? 1 : -1;

        float x = transform.position.x;
        float y = transform.position.y;

        transform.Translate(Vector3.forward * delta);
        //transform.Translate(zoom_direction * delta);

        float z = transform.position.z;
        float min = -40, max = 20;
        z = Mathf.Clamp(z, min, max);
        transform.position = new Vector3(x, y, z);

        InZoomMode = z >= -3f;

        Debug.Log("z = " + z);
    }
}
