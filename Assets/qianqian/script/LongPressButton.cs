using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class LongPressButton : Button
{
    // ����
    public ButtonClickedEvent OnLongPress_begin = new ButtonClickedEvent();
    public ButtonClickedEvent OnLongPress_end = new ButtonClickedEvent();


    // ������Ҫ�ı�������
    private bool my_isStartPress = false;
    private float my_curPointDownTime = 0f;
    private float my_longPressTime = 0.01f;
    private bool my_longPressTrigger = false;


    void Update()
    {
        CheckIsLongPress();
    }

    #region ����

    /// <summary>
    /// ������
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
        // ����ˢ�®�ǰ�r�g
        base.OnPointerDown(eventData);
        my_curPointDownTime = Time.time;
        my_isStartPress = true;
        my_longPressTrigger = false;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        // ָᘔE�𣬽Y���_ʼ�L��
        base.OnPointerUp(eventData);

        if (OnLongPress_end != null)
        {
            OnLongPress_end.Invoke();
        }

        my_isStartPress = false;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        // ָ��Ƴ����Y���_ʼ�L����Ӌ�r�L����־
        base.OnPointerExit(eventData);
        my_isStartPress = false;

    }

    #endregion
}