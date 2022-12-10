using UnityEngine;
using System.Collections;
//using DG.Tweening;
using System;
using Unity.Collections;
public class CameraRotateAround : MonoBehaviour
{
    private Transform mainCamera;
    [Header("Ŀ���")] public Transform targetPos;
    [Header("��ʼ���Ƕ�")] public Vector3 InitRotation = new Vector3(45, 0, 0);

    [Header("��ת�ٶ�")] public float RationSpeed = 10f;
    [Header("��תƽ���ٶ�")] public float rationSmoothSpeed = 10f;
    [Header("λ��ƽ���ٶ�")] public float posSmoothSpeed = 20f;
    [Header("���ֵ��ٶ�")] public float zoomSpeed = 40f;
    [Header("�����ƶ���ƽ���ٶ�")] public float zoomDampening = 5f;
    [Header("��С����ת")] public float LimitMinRationX = 20f;
    [Header("������ת")] public float LimitMaxRationX = 60f;
    [Header("�ƶ�ʱ��")] public float durtion = 2f;
    [Header("��ʼλ�õĸ߶�")] public float StartPosHeight = 500f;
    [Header("��ת�뾶")] public float R = 240f;
    [Header("���ֵ���С����")] public float minR = 200f;

    [Header("�Զ���ȡ�뾶")] public bool AutoR = false;
    [Header("��ת�������")] public bool CursorEnable = false;

    private float currentR;
    private float computeR;
    private float x, y;
    private float wheel;

    private bool LimitCamreaRation = false;    //���������������ת

    private Vector3 targetDir;               //��������
    private Vector3 first_Direction;         //��ʼ�ƶ�ʱ�ķ���
    private Vector3 second_Direction;        //���ƶ���Ŀ��λ�ú�ͨ����������ĵڶ�������
    private Vector3 end;
    private Vector3 computeEnd;
    private Vector3 targetPosVector3;

    private Quaternion Rotation;             //�Զ���ȡ����ת
    private Quaternion RotationA;            //��ת����A 

    private void Start()
    {
        mainCamera = Camera.main.transform;
    }
    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            InitCameraRotateAround();
        }
        if (Input.GetMouseButton(1) && LimitCamreaRation)

        {
            ControlCamrea();
        }
        else
        {
            Cursor.visible = true;
        }
        ContorlScroll();
    }
    private void InitCameraRotateAround()
    {
        LimitCamreaRation = false;
        targetPosVector3 = targetPos.position;
        if (InitRotation == Vector3.zero || AutoR)
        {
            Rotation = mainCamera.rotation;
            x = Rotation.eulerAngles.y;
            y = Rotation.eulerAngles.x;
        }
        else
        {
            Rotation.eulerAngles = InitRotation;
            x = InitRotation.y;
            y = InitRotation.x;
        }
        if (AutoR)
        {
            R = Vector3.Distance(targetPos.position, mainCamera.position);
        }
        else
        {
            targetDir = (targetPosVector3 - mainCamera.position).normalized;
        }
        currentR = R;
        computeR = R;
        RotationA = Rotation;
        second_Direction = Rotation * Vector3.forward;
        end = targetPosVector3 - second_Direction * currentR;
        if (AutoR)
            StartCoroutine("CorrectInitPos");
        else
            CamreaMove();
    }
    private void ControlCamrea()
    {
        if (CursorEnable)
            Cursor.visible = false;
        x += Input.GetAxis("Mouse X") * RationSpeed;
        y -= Input.GetAxis("Mouse Y") * RationSpeed;
        y = Mathf.Clamp(y, LimitMinRationX, LimitMaxRationX);
        RotationA = Quaternion.Slerp(RotationA, Quaternion.Euler(y, x, 0), rationSmoothSpeed * Time.deltaTime);
        first_Direction = RotationA * targetDir;
        end = targetPosVector3 - first_Direction * currentR;
        SetCamera(RotationA, Vector3.Slerp(end, (targetPosVector3 - first_Direction * currentR), posSmoothSpeed * 0.02F));
    }
    private void CamreaMove()
    {
        /*   if (!AutoR)
           {
               mainCamera.DOKill();
               targetDir = targetPos.position - end;
               mainCamera.position = targetPos.position - targetDir.normalized * StartPosHeight;
               mainCamera.localRotation = Rotation;
               mainCamera.DOMove(end, durtion).OnComplete(() =>
               {
                   SetCamera(Rotation, end);
               });
           }
           else
           {
               //  SetCamera(Rotation, end);     
           }*/
        SetCamera(Rotation, end);
    }
    private void SetCamera(Quaternion Rotation, Vector3 end)
    {
        mainCamera.rotation = Rotation;
        mainCamera.position = end;
        targetDir = Vector3.forward;
        LimitCamreaRation = true;
    }
    private void ContorlScroll()
    {
        wheel = Input.GetAxis("Mouse ScrollWheel");
        if (wheel != 0 && LimitCamreaRation)
        {
            computeR -= (wheel > 0 ? 0.1F : -0.1F) * zoomSpeed;
            computeR = Mathf.Clamp(computeR, minR, R);
        }
        if (Mathf.Abs(Mathf.Abs(computeR) - Mathf.Abs(currentR)) >= 0.1f)
        {
            currentR = Mathf.Lerp(currentR, computeR, Time.deltaTime * zoomDampening);
            computeEnd = targetPosVector3 - (RotationA * Vector3.forward * currentR);
            mainCamera.position = computeEnd;
        }
    }
    private IEnumerator CorrectInitPos()
    {
        while (true)
        {
            if (Vector3.Distance(mainCamera.position, end) >= 0.01f)
            {
                mainCamera.rotation = Rotation;
                mainCamera.position = Vector3.Lerp(mainCamera.position, end, 0.15f);
                yield return null;
            }
            else
            {
                mainCamera.rotation = Rotation;
                mainCamera.position = end;
                targetDir = Vector3.forward;
                LimitCamreaRation = true;
                StopCoroutine("CorrectInitPos");
                break;
            }
        }
    }
}