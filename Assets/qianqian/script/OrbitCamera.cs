using UnityEngine;
using System.Collections;
using UnityEngine.Animations;

public class OrbitCamera : MonoBehaviour
{
    public Transform pivot;
    public Transform camera;
    private float distance;

    public bool distanceAdjustable = true;
    public bool rotationAdjustable = true;

    void Start()
    {
        targetSideRotation = transform.eulerAngles.y;
        currentSideRotation = transform.eulerAngles.y;
        targetUpRotation = transform.eulerAngles.x;
        currentUpRotation = transform.eulerAngles.x;
        distance = -camera.localPosition.z;             //相机局部坐标z值为-1.8，那么相机与距离人物为1.8
    }

    void LateUpdate()
    {
        if (!pivot) return;

        Follow();
        DragRotate();
        ScrollScale();
        OcclusionJudge();
    }

    void Follow()
    {
        if (!pivot.gameObject.GetComponentInParent<ParentConstraint>())
        {
            transform.position = Vector3.Lerp(transform.position, pivot.position, Time.deltaTime * 5);             //相机跟随角色的插值，相机当前帧的实际位置为它们中间10%的位置
            camera.localPosition = -Vector3.forward * distance;                                                     //仅z有值，并且方向为负
        }
        else       //高速运动下不再插值
        {
            transform.position = pivot.position;
            camera.localPosition = -Vector3.forward * distance;
        }
    }

    public float MinimumDegree = 0;
    public float MaximumDegree = 60;
    private float targetSideRotation;
    private float targetUpRotation;
    private float currentSideRotation;
    private float currentUpRotation;
    void DragRotate()
    {
        if (!rotationAdjustable) return;

        if (Input.GetMouseButton(0))
        {
            targetSideRotation += Input.GetAxis("Mouse X") * 5;
            targetUpRotation -= Input.GetAxis("Mouse Y") * 5;
        }

        targetUpRotation = Mathf.Clamp(targetUpRotation, MinimumDegree, MaximumDegree);

        currentSideRotation = Mathf.LerpAngle(currentSideRotation, targetSideRotation, Time.deltaTime * 5);
        currentUpRotation = Mathf.Lerp(currentUpRotation, targetUpRotation, Time.deltaTime * 5);
        transform.rotation = Quaternion.Euler(currentUpRotation, currentSideRotation, 0);
    }

    float MinimumDistance = 1;
    float MaximumDistance = 4;
    void ScrollScale()
    {
        if (!distanceAdjustable) return;

        distance *= (1 - Input.GetAxis("Mouse ScrollWheel") * 0.2f);        //在原值的基础上调整为原值的百分比
        distance = Mathf.Clamp(distance, MinimumDistance, MaximumDistance);
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
            preferdDistance = distance;
    }

    float preferdDistance = 1;
    bool resumable = false;
    void OcclusionJudge()
    {
        if (Physics.Raycast(pivot.position, -camera.forward, distance))
        {
            resumable = true;

            distance = NearestObstacleDistance(pivot);

            while (Physics.Raycast(pivot.position, -camera.forward, distance) && distance > MinimumDistance)
            {
                distance *= 0.99f;
                distance = Mathf.Clamp(distance, MinimumDistance, MaximumDistance);
                float dist = Mathf.Lerp(-camera.transform.localPosition.z, distance, 1f);
                camera.localPosition = -Vector3.forward * dist;
            }
        }

        if (!resumable) return;

        if (resumable && !Physics.Raycast(pivot.position, -camera.forward, preferdDistance))
        {
            distance = preferdDistance;
            float dist = Mathf.Lerp(-camera.transform.localPosition.z, distance, 1f);
            camera.localPosition = -Vector3.forward * distance;
            resumable = false;
        }
    }

    float NearestObstacleDistance(Transform start)
    {
        float dis = float.MaxValue;
        RaycastHit hit;
        Physics.Raycast(start.position, start.forward, out hit);
        if (hit.distance != 0) dis = Mathf.Min(dis, hit.distance);
        Physics.Raycast(start.position, -start.forward, out hit);
        if (hit.distance != 0) dis = Mathf.Min(dis, hit.distance);
        Physics.Raycast(start.position, start.right, out hit);
        if (hit.distance != 0) dis = Mathf.Min(dis, hit.distance);
        Physics.Raycast(start.position, -start.right, out hit);
        if (hit.distance != 0) dis = Mathf.Min(dis, hit.distance);
        return dis;
    }
}