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
    public Button btn_test;
    public Button btn_test1;

    [HideInInspector]
    public UnityEvent OnLongPressBegin = new UnityEvent();

    [HideInInspector]
    public UnityEvent OnLongPressEnd = new UnityEvent();

    public Text RecognizedSpeech;

    private bool isLoading = false;


    public Slider slider;
    public Text slider_value_text;


    // Start is called before the first frame update
    void Start()
    {
        btn.OnLongPress_begin.AddListener(() => 
        {
            if(OnLongPressBegin != null && !isLoading)
            {
                OnLongPressBegin.Invoke();
            }
        });

        btn.OnLongPress_end.AddListener(() =>
        {
            if (OnLongPressEnd != null && !isLoading)
            {
                OnLongPressEnd.Invoke();
            }
        });

        // test
        btn_test.onClick.AddListener(() => 
        {
            //SpeechManager.Instance.SynthesisSpeech("你好");
            //ModelAnimationController.Instance.Play(ModelAnimation.Type.smile);
        });

        btn_test1.onClick.AddListener(() =>
        {
            //SpeechManager.Instance.CheckDevice();
            //ModelAnimationController.Instance.Play(ModelAnimation.Type.idle);
        });

        slider_value_text.text = "间隔时间秒:" + slider.value.ToString();
        slider.onValueChanged.AddListener((float value) =>
        {
            slider_value_text.text = "间隔时间秒:" + value.ToString();
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
