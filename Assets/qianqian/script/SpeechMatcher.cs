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


    public static string KEY_recommend = "推荐";




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
        if(key.Contains("您好") || 
            key.Contains("你好") ||
            key.Contains("好") ||
            key.Contains("名字"))
        {
            //return "Hello 大家好，我是芊芊，欢迎来到分身世界，" +
            //    "今后的日子里我将和大家一起交流，学习。" +
            //    "我也有不少的功能可以展示，首先您可以直接用语音和我交流，" +
            //    "把我当成你的小伙伴，另外我还可以和您分身世界的好友一起互动，" +
            //    "实时视频语音交流哦。您也可以先考考我哦";
            return "你好，欢迎来到分身世界，我是你的虚拟分身，我叫千千。你可以直接用语音和我交流。";
        }
        else if (key.Contains("星期几"))
        {
            return "今天" + GetToday();
        }
        else if (key.Contains("天气"))
        {
            return "今天晴朗，适合散步哦";
        }
        else if (key.Contains(KEY_recommend))
        {
            //return "西湖,苏堤等等,西湖无疑是杭州之美的代表，" +
            //    "自然与人文相互映衬，" +
            //    "你不妨花上半天或一天在湖边徜徉一番，" +
            //    "无论怎么玩，都让人心情舒畅。";
            return "西湖,无疑是杭州之美的代表,自然与人文相互映衬,你不妨花上半天,在湖边徜徉一番,无论怎么玩,都让人心情舒畅";
        }

        return "听不清，再说一遍";
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
