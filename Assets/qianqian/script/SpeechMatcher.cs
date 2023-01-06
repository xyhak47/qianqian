using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speech
{
    public string keyword;
    public string content;

    public Queue<Strategy> queue_strategy;
}



public class SpeechMatcher : MonoBehaviour
{
    public static SpeechMatcher Instance = null;
    SpeechMatcher()
    {
        Instance = this;
    }


    public static string KEY_recommend = "�Ƽ�";




    private List<Speech> List_Speech = new List<Speech>();



    private void Start()
    {
        
    }

    public Speech Match(string key)
    {
        //Speech speech = List_Speech.Find(s => s.keyword == key);
        //if(speech == null)
        //{
        //    speech = new Speech();
        //    speech.keyword = key;
        //    speech.content = GetSpeech(key);

        //    speech.queue_strategy = SpeechStrategyManager.Instance.GetStrategyQueue(key);

        //    List_Speech.Add(speech);
        //}

        Speech speech = new Speech();
        speech.keyword = key;
        speech.content = GetSpeech(key);

        speech.queue_strategy = SpeechStrategyManager.Instance.GetStrategyQueue(key);


        return speech;
    }

    private string GetSpeech(string key)
    {
        if(key.Contains("����") || 
            key.Contains("���") ||
            key.Contains("��") ||
            key.Contains("����"))
        {
            //return "Hello ��Һã�����ܷܷ����ӭ�����������磬" +
            //    "�����������ҽ��ʹ��һ������ѧϰ��" +
            //    "��Ҳ�в��ٵĹ��ܿ���չʾ������������ֱ�����������ҽ�����" +
            //    "���ҵ������С��飬�����һ����Ժ�����������ĺ���һ�𻥶���" +
            //    "ʵʱ��Ƶ��������Ŷ����Ҳ�����ȿ�����Ŷ";
            return "��ã���ӭ�����������磬���������������ҽ�ǧǧ�������ֱ�����������ҽ�����";
        }
        else if (key.Contains("���ڼ�"))
        {
            return "����" + GetToday();
        }
        else if (key.Contains("����"))
        {
            return "�������ʣ��ʺ�ɢ��Ŷ";
        }
        else if (key.Contains(KEY_recommend))
        {
            //return "����,�յ̵ȵ�,���������Ǻ���֮���Ĵ���" +
            //    "��Ȼ�������໥ӳ�ģ�" +
            //    "�㲻�����ϰ����һ���ں�������һ����" +
            //    "������ô�棬�����������泩��";
            return "����,�����Ǻ���֮���Ĵ���,��Ȼ�������໥ӳ��,�㲻�����ϰ���,�ں�������һ��,������ô��,�����������泩";
        }

        return "�����壬��˵һ��";
    }

    private string GetToday()
    {
        string weekstr = DateTime.Now.DayOfWeek.ToString();
        switch (weekstr)
        {
            case "Monday": weekstr = "����һ"; break;
            case "Tuesday": weekstr = "���ڶ�"; break;
            case "Wednesday": weekstr = "������"; break;
            case "Thursday": weekstr = "������"; break;
            case "Friday": weekstr = "������"; break;
            case "Saturday": weekstr = "������"; break;
            case "Sunday": weekstr = "������"; break;
        }

        return weekstr;
    }

}
