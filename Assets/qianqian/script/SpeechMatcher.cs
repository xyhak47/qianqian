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
        if(key.Contains("您好") || 
            key.Contains("你好") || 
            key.Contains("名字"))
        {
            return "Hello 大家好，我是芊芊，欢迎来到分身世界，" +
                "今后的日子里我将和大家一起交流，学习。" +
                "我也有不少的功能可以展示，首先您可以直接用语音和我交流，" +
                "把我当成你的小伙伴，另外我还可以和您分身世界的好友一起互动，" +
                "实时视频语音交流哦。您也可以先考考我哦";
        }
        else if (key.Contains("星期几"))
        {
            return "今天" + GetToday();
        }
        else if (key.Contains("天气"))
        {
            return "今天晴朗，适合散步哦";
        }
        else if (key.Contains("推荐"))
        {
            return "西湖,苏堤等等,西湖无疑是杭州之美的代表，" +
                "自然与人文相互映衬，" +
                "你不妨花上半天或一天在湖边徜徉一番，" +
                "无论怎么玩，都让人心情舒畅。";
        }

        return "未识别";
    }

    private string GetToday()
    {
        string weekstr = DateTime.Now.DayOfWeek.ToString();
        switch (weekstr)
        {
            case "Monday": weekstr = "星期一"; break;
            case "Tuesday": weekstr = "星期二"; break;
            case "Wednesday": weekstr = "星期三"; break;
            case "Thursday": weekstr = "星期四"; break;
            case "Friday": weekstr = "星期五"; break;
            case "Saturday": weekstr = "星期六"; break;
            case "Sunday": weekstr = "星期日"; break;
        }

        return weekstr;
    }

}
