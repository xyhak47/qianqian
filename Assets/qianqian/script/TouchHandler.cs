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
        //二、多点触摸, 放大缩小
        Touch newTouch1 = Input.GetTouch(0);
        Touch newTouch2 = Input.GetTouch(1);
        //第2点刚开始接触屏幕, 只记录，不做处理
        if (newTouch2.phase == TouchPhase.Began)
        {
            oldTouch2 = newTouch2;
            oldTouch1 = newTouch1;
            return;
        }
        //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型
        float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
        float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);
        //两个距离之差，为正表示放大手势， 为负表示缩小手势
        float offset = newDistance - oldDistance;
        //放大因子， 一个像素按 0.01倍来算(100可调整)
        float scaleFactor = offset / 100f;
        Vector3 localScale = transform.localScale;
        Vector3 scale = new Vector3(localScale.x + scaleFactor,
                                    localScale.y + scaleFactor,
                                    localScale.z + scaleFactor);
        //在什么情况下进行缩放
        if (scale.x >= 0.05f && scale.y >= 0.05f && scale.z >= 0.05f)
        {
            //transform.localScale = scale;
            ModelController.Instance.Scale(scale);
        }
        //记住最新的触摸点，下次使用
        oldTouch1 = newTouch1;
        oldTouch2 = newTouch2;
    }

}
