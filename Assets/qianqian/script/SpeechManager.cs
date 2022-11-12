using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechManager : MonoBehaviour
{
    public static SpeechManager Instance = null;
    SpeechManager()
    {
        Instance = this;
    }

    private void Start()
    {
        UIHandler.Instance.OnLongPressBegin.AddListener(BeginSpeech);
        UIHandler.Instance.OnLongPressEnd.AddListener(EndSpeech);
    }

    private void BeginSpeech()
    {
        Debug.Log("BeginSpeech");

    }

    private void EndSpeech()
    {
        Debug.Log("EndSpeech");

    }
}
