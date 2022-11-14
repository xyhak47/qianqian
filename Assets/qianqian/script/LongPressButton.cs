using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class LongPressButton : Button
{
    // 长按
    public ButtonClickedEvent OnLongPress_begin = new ButtonClickedEvent();
    public ButtonClickedEvent OnLongPress_end = new ButtonClickedEvent();


    // 长按需要的变量参数
    private bool my_isStartPress = false;
    private float my_curPointDownTime = 0f;
    private float my_longPressTime = 0.01f;
    private bool my_longPressTrigger = false;


    void Update()
    {
        CheckIsLongPress();
    }

    #region 长按

    /// <summary>
    /// 处理长按
    /// </summary>
    void CheckIsLongPress()
    {
        if (my_isStartPress && !my_longPressTrigger)
        {
            if (Time.time > my_curPointDownTime + my_longPressTime)
            {
                my_longPressTrigger = true;
                my_isStartPress = false;
                if (OnLongPress_begin != null)
                {
                    OnLongPress_begin.Invoke();
                }
            }
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        // 按下刷新當前時間
        base.OnPointerDown(eventData);
        my_curPointDownTime = Time.time;
        my_isStartPress = true;
        my_longPressTrigger = false;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        // 指針擡起，結束開始長按
        base.OnPointerUp(eventData);

        if (OnLongPress_end != null)
        {
            OnLongPress_end.Invoke();
        }

        my_isStartPress = false;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        // 指針移出，結束開始長按，計時長按標志
        base.OnPointerExit(eventData);
        my_isStartPress = false;

    }

    #endregion
}