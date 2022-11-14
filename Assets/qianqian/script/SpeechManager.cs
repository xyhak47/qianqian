using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wit.BaiduAip.Speech;

public class SpeechManager : MonoBehaviour
{
    public static SpeechManager Instance = null;
    SpeechManager()
    {
        Instance = this;
    }

    // api key 
    private string APIKey = "Q8f3j0kwF5GSXx8MdwGpqKk5";
    private string SecretKey = "Z79qjlhLnOVB0pVG5SfZcnZS503UG8mh";

    private AudioClip _clipRecord;
    private AudioSource _audioSource;
    private Asr _asr;

    private Tts _tts;


    private void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();

        UIHandler.Instance.OnLongPressBegin.AddListener(UserSpeechBegin);
        UIHandler.Instance.OnLongPressEnd.AddListener(UserSpeechEnd);

        // voice to word
        _asr = new Asr(APIKey, SecretKey);


        // word to voice
        _tts = new Tts(APIKey, SecretKey);
        StartCoroutine(_tts.GetAccessToken());
    }

    private void UserSpeechBegin()
    {
        //Debug.Log("BeginSpeech");
        //UIHandler.Instance.ShowRecognizedSpeech("BeginSpeech");

        _clipRecord = Microphone.Start(null, false, 30, 16000);

        if(_clipRecord == null)
        {
            UIHandler.Instance.ShowRecognizedSpeech("Microphone start fail");
        }
    }

    private void UserSpeechEnd()
    {
        //Debug.Log("EndSpeech");
        //UIHandler.Instance.ShowRecognizedSpeech("EndSpeech");

        Microphone.End(null);
        var data = Asr.ConvertAudioClipToPCM16(_clipRecord);
        StartCoroutine(_asr.Recognize(data, s =>
        {
            if(s.result != null && s.result.Length > 0)
            {
                string result = s.result[0];
                string matched_speech = SpeechMatcher.Instance.Match(result);

                string question_and_answer = "question = " + result + " answer = " + matched_speech;
                UIHandler.Instance.ShowRecognizedSpeech(question_and_answer);

                Synthesis(matched_speech);
            }
            else
            {
                UIHandler.Instance.ShowRecognizedSpeech("Î´Ê¶±ð:" + s.err_msg);
            }
        }));
    }

    private void Synthesis(string speech)
    {
        StartCoroutine(_tts.Synthesis(speech, s =>
        {
            if (s.Success)
            {
                _audioSource.clip = s.clip;
                _audioSource.Play();
            }
            else
            {
        
            }
        }));
    }
}
