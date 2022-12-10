using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private Touch oldTouch1;  //�ϴδ�����1(��ָ1) 
    private Touch oldTouch2;  //�ϴδ�����2(��ָ2)
    void Update()
    {
        //û�д��������Ǵ�����Ϊ0
        if (Input.touchCount <= 0)
        {
            return;
        }
        //һ�����㴥���� ˮƽ������ת
        if (1 == Input.touchCount)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 deltaPos = touch.deltaPosition;//������λ�ã�����֡ʱ���ڵĸı䣨����һ֡�������λ�ã���ȥǰһ֡�������λ�ã�


            if (Mathf.Abs(touch.deltaPosition.x) > Mathf.Abs(touch.deltaPosition.y))
            {
                //��Y����ת
                CameraViewController.Instance.RotateAroundX(deltaPos.x/10f);
            }
            else
            {
                if (CameraViewController.Instance.InZoomMode)
                {
                    CameraViewController.Instance.MoveY(-deltaPos.y/100f);
                }
                else
                {
                    //��X����ת
                    CameraViewController.Instance.RotateAroundY(-deltaPos.y / 100f);
                }
            }

            // ModelController.Instance.HandleTouchXY(deltaPos);
        }
        else if(2 == Input.touchCount)
        {
            Touch newTouch1 = Input.GetTouch(0);
            Touch newTouch2 = Input.GetTouch(1);

            if (newTouch2.phase == TouchPhase.Began)
            {
                oldTouch2 = newTouch2;
                oldTouch1 = newTouch1;
            }
            else if (newTouch2.phase == TouchPhase.Moved)
            {
                float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
                float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);

                //UIHandler.Instance.ShowRecognizedSpeech("delta distance = " + (newDistance - oldDistance));

                if (Mathf.Abs(newDistance - oldDistance) > 3f)
                {
                    float scale = newDistance / oldDistance;
                    //ModelController.Instance.HandleTouchZ(scale);
                    bool zoom_in = scale - 1 >= 0;
                    CameraViewController.Instance.Zoom(zoom_in, scale);
                }

                oldTouch2 = newTouch2;
                oldTouch1 = newTouch1;
            } 
        }
    }
}
