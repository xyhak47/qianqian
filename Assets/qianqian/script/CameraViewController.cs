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

        //transform.LookAt(ModelController.Instance.GetModelHead().transform);
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        camerarotate();
        camerazoom();

        Vector3 direction = Camera.main.transform.forward + ModelController.Instance.GetModelHead().transform.localPosition;

        Debug.DrawRay(transform.position, direction, Color.red);
        //Debug.Log(transform.position.x + " " + transform.position.y + " " + transform.position.z);
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
        //return;

        if (InZoomMode) return;

        float roateSpeed = 5f;
        float rotatedAngle = transform.eulerAngles.x + y * roateSpeed;
        float angle_min = 0f, angle_max = 40f;

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
        //use camera fov
        delta *= !In ? 1 : -1;
        delta *= 1.1f;
        float fov = Camera.main.fieldOfView;
        fov += delta;
        float min = 6f, max = 36f;
        fov = Mathf.Clamp(fov, min, max);
        Camera.main.fieldOfView = fov;

        InZoomMode = fov <= 32f;

        //if (!In && fov == max)
        //{
        //    ModelController.Instance.LerpModelYtoOriginIfNeeded();
        //}
        float input_x = 1 - fov / max;
        float output_y = 1 - 5 * input_x / 6;
        output_y = (1 / output_y - 1) /5;

        if (In)
        {
            //if(fov <=22.5f)
            {
                ModelController.Instance.LerpModelYtoTop(output_y);
            }
        }
        else
        {
            ModelController.Instance.LerpModelYtoOrigin(1/output_y);
        }

        return;


        float distance = Vector3.Distance(transform.position, target.transform.position);
        Debug.Log("distance in zoom = " + distance + " In = " + In);
        float limit_near = 38f;
        float limit_far = origin_distance;
        bool too_near = distance <= limit_near;
        bool too_far = distance >= limit_far;

        InZoomMode = distance <= 90f;

        if (In && too_near)
        {
            //ModelController.Instance.LerpModelYtoTop(0.12f);
            return;
        }

        if (!In && too_far)
        {
            //ModelController.Instance.LerpModelYtoOrigin(0.12f);
            return;
        }


        if (!In && need_reset_camera_position && InZoomMode)
        {
            //ModelController.Instance.ResetPosition();

            //transform.position = camera_pos;
            //transform.position = new Vector3(
            //    transform.position.x,
            //    transform.position.y - y_offset_in_MoveY,
            //    transform.position.z);

            //y_offset_in_MoveY = 0;
            //transform.LookAt(ModelController.Instance.GetModelHead().transform);

            need_reset_camera_position = false;
        }



        delta *= In ? 1 : -1;

        Vector3 direction = ModelController.Instance.GetModelHead().transform.position - transform.position;
        //Vector3 direction = Camera.main.transform.forward + ModelController.Instance.GetModelHead().transform.localPosition;

        direction.Normalize();
        //transform.Translate(Camera.main.transform.forward * delta, Space.World);
        transform.Translate(direction * delta, Space.World);

        float x = transform.position.x;
        float z = transform.position.z;
        float y = transform.position.y;

        float min_z = -38f, max_z = 33f;
        z = Mathf.Clamp(z, min_z, max_z);

        //float min_y = -22.5f, max_y = 12.5f;
        //y = Mathf.Clamp(y, min_y, max_y);
        if(!In && z == min_z)
        {
            //y = Mathf.Lerp(0, y, 0.1f);
        }

        //transform.position = new Vector3(x, y, z);

        StoreCameraPosition();
    }

    private void StoreCameraPosition()
    {
        camera_pos = transform.position;
    }
}
