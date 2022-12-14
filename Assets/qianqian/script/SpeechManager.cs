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


    private bool InUserInput = false;


    public bool save_clip = false;

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
        InUserInput = true;

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
            if (InUserInput)
            {
                return;
            }

            if (s.result != null && s.result.Length > 0)
            {
                string result = s.result[0];

                SynthesisSpeech(result);
            }
            else
            {
                Speech temp = new Speech();
                temp.content = "????????????????";
                Synthesis(temp);
                UIHandler.Instance.ShowRecognizedSpeech("??????:" + s.err_msg);
            }
        }));

        InUserInput = false;
    }

    public void SynthesisSpeech(string key)
    {
        Speech matched_speech = SpeechMatcher.Instance.Match(key);

        string question_and_answer = "question = " + matched_speech.keyword + " answer = " + matched_speech.content;
        UIHandler.Instance.ShowRecognizedSpeech(question_and_answer);

        Synthesis(matched_speech);
    }

    private void Synthesis(Speech speech)
    {
        StartCoroutine(_tts.Synthesis(speech.content, response =>
        {
            if(InUserInput)
            {
                return;
            }

            if (response.Success)
            {
                StartCoroutine(HandleSpeechContent(response.clip, speech));
            }
            else
            {
            }
        }));
    }

    private IEnumerator HandleSpeechContent(AudioClip clip, Speech speech)
    {
        float total_duration = clip.length;
        //tring type = duration >= 3f ? ModelAnimation.Type.talk_long : ModelAnimation.Type.talk_short;
        string animation_type = ModelAnimation.Type.talk_short;

        total_duration -= 0.75f;
        _audioSource.clip = clip;


        //????????
        if(save_clip)
        {
            string outPath = Application.streamingAssetsPath;
            WavUtility.FromAudioClip(clip, out outPath, true);
        }



        Queue<Strategy> queue_strategy = speech.queue_strategy;
        if(queue_strategy == null)
        {
            ModelAnimationController.Instance.Play(animation_type, total_duration, null);
            //Debug.Log("_audioSource.timeSamples = " + _audioSource.timeSamples);
            _audioSource.Play();
        }
        else
        {
            while (queue_strategy.Count != 0)
            {
                Strategy strategy = queue_strategy.Dequeue();
                //Debug.Log(strategy.type);
                if(strategy.type == Strategy.Type.talk)
                {
                    //ModelAnimationController.Instance.Play(animation_type);
                    ModelAnimationController.Instance.SetTalkLayerWeight(1);
                    _audioSource.time = strategy.begin;
                    _audioSource.Play();
                    yield return new WaitForSeconds(strategy.duration);
                    _audioSource.Pause();
                }
                else if(strategy.type == Strategy.Type.wait)
                {
                    //ModelAnimationController.Instance.Play(ModelAnimation.Type.idle);
                    ModelAnimationController.Instance.SetTalkLayerWeight(0);

                    //float wait_sec = UIHandler.Instance.slider.value;
                    //yield return new WaitForSeconds(wait_sec);

                    yield return new WaitForSeconds(0.28f);

                }
            }

            // rewind 
            _audioSource.time = 0f;
        }


        yield break;
    }

    public void CheckDevice()
    {
        string[] devices = Microphone.devices;
        if (devices.Length > 0)
        {
            //Debug.Log("????????????:" + devices[0]);
            UIHandler.Instance.ShowRecognizedSpeech("????????????:" + devices[0]);
        }
        else
        {
            //Debug.Log("??????????????");
            UIHandler.Instance.ShowRecognizedSpeech("??????????????");
        }
    }

    private void ResetSelf()
    {
        StopAllCoroutines();

        _audioSource.time = 0f;
        _audioSource.Stop();

        ModelAnimationController.Instance.ResetSelf();
    }

}
