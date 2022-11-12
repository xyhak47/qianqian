using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance = null;
    UIHandler()
    {
        Instance = this;
    }

    public LongPressButton btn;

    public UnityEvent OnLongPressBegin = new UnityEvent();
    public UnityEvent OnLongPressEnd = new UnityEvent();


    // Start is called before the first frame update
    void Start()
    {
        btn.OnLongPress_begin.AddListener(() => 
        {
            if(OnLongPressBegin != null)
            {
                OnLongPressBegin.Invoke();
                //Debug.Log("OnLongPress_begin");
            }
        });

        btn.OnLongPress_end.AddListener(() =>
        {
            if (OnLongPressEnd != null)
            {
                OnLongPressEnd.Invoke();
                //Debug.Log("OnLongPress_end");
            }
        });
    }
}
