using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance = null;
    UIHandler()
    {
        Instance = this;
    }

    public LongPressButton btn;

    [HideInInspector]
    public UnityEvent OnLongPressBegin = new UnityEvent();

    [HideInInspector]
    public UnityEvent OnLongPressEnd = new UnityEvent();

    public Text RecognizedSpeech;

    private bool isLoading = false;


    // Start is called before the first frame update
    void Start()
    {
        btn.OnLongPress_begin.AddListener(() => 
        {
            if(OnLongPressBegin != null && !isLoading)
            {
                OnLongPressBegin.Invoke();
                //Debug.Log("OnLongPress_begin");
            }
        });

        btn.OnLongPress_end.AddListener(() =>
        {
            if (OnLongPressEnd != null && !isLoading)
            {
                OnLongPressEnd.Invoke();
                //Debug.Log("OnLongPress_end");
            }
        });
    }

    public void ShowRecognizedSpeech(string speech)
    {
        RecognizedSpeech.text = speech;
    }

    public void ShowLoading(bool loading)
    {
        isLoading = loading;
    }
}
