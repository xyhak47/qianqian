using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private Touch oldTouch1;  //上次触摸点1(手指1) 
    private Touch oldTouch2;  //上次触摸点2(手指2)
    void Update()
    {
        //没有触摸，就是触摸点为0
        if (Input.touchCount <= 0)
        {
            return;
        }
        //一、单点触摸， 水平上下旋转
        if (1 == Input.touchCount)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 deltaPos = touch.deltaPosition;//触摸点位置，在两帧时间内的改变（即这一帧触摸点的位置，减去前一帧触摸点的位置）


            //if (Mathf.Abs(touch.deltaPosition.x) > Mathf.Abs(touch.deltaPosition.y))
            //{
            //    //绕Y轴旋转
            //    transform.Rotate(Vector3.down * deltaPos.x, Space.World);
            //}
            //else
            //{
            //    //绕X轴旋转
            //    transform.Rotate(Vector3.right * deltaPos.y, Space.World);
            //}

            ModelController.Instance.Rotate(deltaPos.x);
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

                float offset_percent = (newDistance - oldDistance) / oldDistance;

                ModelController.Instance.MoveZ(offset_percent);
            } 
        }
    }
}
