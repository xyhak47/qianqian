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
        CheckDevice();

        _audioSource = gameObject.GetComponent<AudioSource>();

        UIHandler.Instance.OnLongPressBegin.AddListener(UserSpeechBegin);
        UIHandler.Instance.OnLongPressEnd.AddListener(UserSpeechEnd);
                     
        // voice to word
        _asr = new Asr(APIKey, SecretKey);
        StartCoroutine(_asr.GetAccessToken());


        // word to voice
        _tts = new Tts(APIKey, SecretKey);
        StartCoroutine(_tts.GetAccessToken());
    }

    private void UserSpeechBegin()
    {
        if (_audioSource.isPlaying)
        {
            ResetSelf();
        }


        //Debug.Log("BeginSpeech");
        UIHandler.Instance.ShowRecognizedSpeech("BeginSpeech");

        _clipRecord = Microphone.Start(null, false, 30, 16000);

        if(_clipRecord == null)
        {
            UIHandler.Instance.ShowRecognizedSpeech("Microphone start fail");
        }
    }

    private void UserSpeechEnd()
    {
        //Debug.Log("EndSpeech");
        UIHandler.Instance.ShowRecognizedSpeech("EndSpeech");

        Microphone.End(null);
        var data = Asr.ConvertAudioClipToPCM16(_clipRecord);
        StartCoroutine(_asr.Recognize(data, s =>
        {
            if(s.result != null && s.result.Length > 0)
            {
                string result = s.result[0];

                SynthesisSpeech(result);
            }
            else
            {
                Synthesis("听不清，再说一遍");
                UIHandler.Instance.ShowRecognizedSpeech("未识别:" + s.err_msg);
            }
        }));
    }

    public void SynthesisSpeech(string speech)
    {
        string matched_speech = SpeechMatcher.Instance.Match(speech);

        string question_and_answer = "question = " + speech + " answer = " + matched_speech;
        UIHandler.Instance.ShowRecognizedSpeech(question_and_answer);

        Synthesis(matched_speech);
    }

    private void Synthesis(string speech)
    {
        StartCoroutine(_tts.Synthesis(speech, s =>
        {
            if (s.Success)
            {
                //if(restart)
                //{
                //    _audioSource.Stop();
                //    _audioSource.clip = null;
                //    return;
                //}

                _audioSource.clip = s.clip;
                _audioSource.Play();

                float duration = _audioSource.clip.length;
                string type = duration >= 3f ? ModelAnimation.Type.talk_long : ModelAnimation.Type.talk_short;

                ModelAnimationController.Instance.Play(type, duration, ()=>
                {
                });
            }
            else
            {
            }
        }));
    }

    public void CheckDevice()
    {
        string[] devices = Microphone.devices;
        if (devices.Length > 0)
        {
            //Debug.Log("设备有麦克风:" + devices[0]);
            UIHandler.Instance.ShowRecognizedSpeech("设备有麦克风:" + devices[0]);
        }
        else
        {
            //Debug.Log("设备没有麦克风");
            UIHandler.Instance.ShowRecognizedSpeech("设备没有麦克风");
        }
    }

    private void ResetSelf()
    {
        _audioSource.Stop();
        _audioSource.clip = null;

        ModelAnimationController.Instance.Play(ModelAnimation.Type.idle);
    }

}
