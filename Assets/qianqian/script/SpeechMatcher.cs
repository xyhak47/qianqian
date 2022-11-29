using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechMatcher : MonoBehaviour
{
    public static SpeechMatcher Instance = null;
    SpeechMatcher()
    {
        Instance = this;
    }

    private void Start()
    {
        
    }

    public string Match(string speech)
    {
        return GetSpeech(speech);
    }

    private string GetSpeech(string key)
    {
        if(key.Contains("����") || 
            key.Contains("���") || 
            key.Contains("����"))
        {
            return "Hello ��Һã�����ܷܷ����ӭ�����������磬" +
                "�����������ҽ��ʹ��һ������ѧϰ��" +
                "��Ҳ�в��ٵĹ��ܿ���չʾ������������ֱ�����������ҽ�����" +
                "���ҵ������С��飬�����һ����Ժ�����������ĺ���һ�𻥶���" +
                "ʵʱ��Ƶ��������Ŷ����Ҳ�����ȿ�����Ŷ";
        }
        else if (key.Contains("���ڼ�"))
        {
            return "����" + GetToday();
        }
        else if (key.Contains("����"))
        {
            return "�������ʣ��ʺ�ɢ��Ŷ";
        }
        else if (key.Contains("�Ƽ�"))
        {
            return "����,�յ̵ȵ�,���������Ǻ���֮���Ĵ���" +
                "��Ȼ�������໥ӳ�ģ�" +
                "�㲻�����ϰ����һ���ں�������һ����" +
                "������ô�棬�����������泩��";
        }

        return "δʶ��";
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
