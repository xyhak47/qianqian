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
    private float origin_distance;

    public bool InZoomMode = false;
    private bool need_reset_camera_position = false;
    private float y_offset_in_MoveY = 0;
    private Vector3 camera_pos;

    // Start is called before the first frame update
    void Start()
    {
        target = ModelController.Instance.model.transform;
        origin_distance = Vector3.Distance(transform.position, target.transform.position);

        transform.LookAt(ModelController.Instance.GetModelHead().transform);
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        camerarotate();
        camerazoom();

        Debug.Log(transform.position.x + " " + transform.position.y + " " + transform.position.z);
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
        if (InZoomMode) return;

        float roateSpeed = 5f;
        float rotatedAngle = transform.eulerAngles.x + y * roateSpeed;
        float angle_min = 5f, angle_max = 40f;

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

        StoreCameraPosition();
    }

    public void RotateAroundX(float x)
    {
        //need_reset_camera_position = true;

        float speed = 5f;
        transform.RotateAround(target.transform.position, Vector3.up, x * speed);

        StoreCameraPosition();
    }

    public void MoveY(float delta_y)
    {
        if (!InZoomMode) return;

        need_reset_camera_position = true;

        //transform.Translate(Vector3.up * (delta_y * 50f) * Time.deltaTime);

        //float x = transform.position.x;
        //float z = transform.position.z;
        //float y = transform.position.y;
        //y += delta_y;
        ////float min = -16f, max = 8.5f;
        ////y = Mathf.Clamp(y, min, max);
        //transform.position = new Vector3(x, y, z);

        //y_offset_in_MoveY = y;



        // use model y
        ModelController.Instance.MoveY(-delta_y);

    }

    //public void MoveX(float x)
    //{
    //    transform.Translate(Vector3.left * (x * 50f) * Time.deltaTime);
    //}

    public void Zoom(bool In, float delta)
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        //Debug.Log("distance in zoom = " + distance + " In = " + In);
        float limit_near = 50f;
        float limit_far = origin_distance;
        bool too_near = distance <= limit_near;
        bool too_far = distance >= limit_far;

        if (In && too_near) return;
        if (!In && too_far) return;

        InZoomMode = distance <= 70f;

        if (need_reset_camera_position && InZoomMode)
        {
            ModelController.Instance.ResetPosition();

            //transform.position = camera_pos;
            //transform.position = new Vector3(
            //    transform.position.x,
            //    transform.position.y - y_offset_in_MoveY,
            //    transform.position.z);

            y_offset_in_MoveY = 0;
            //transform.LookAt(ModelController.Instance.GetModelHead().transform);

            need_reset_camera_position = false;
        }

        delta *= In ? 1 : -1;

        Vector3 direction = ModelController.Instance.GetModelHead().transform.position - transform.position;
        direction.Normalize();
        //transform.Translate(Camera.main.transform.forward * delta, Space.World);
        transform.Translate(direction * delta, Space.World);

        StoreCameraPosition();
    }

    private void StoreCameraPosition()
    {
        camera_pos = transform.position;
    }
}
