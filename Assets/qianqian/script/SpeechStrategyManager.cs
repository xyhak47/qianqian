using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strategy
{
    public enum Type
    {
        talk,
        wait,
        idle,
    }

    public float duration = 0f;
    public Type type = Type.talk;
    public float begin = 0, end = 0;

    static public Strategy Create(Type t, float begin = 0, float end = 0)
    {
        Strategy s = new Strategy();
        s.type = t;
        s.duration = end - begin;
        s.begin = begin;
        s.end = end;
        return s;
    }
}

public class SpeechStrategyManager : MonoBehaviour
{
    public static SpeechStrategyManager Instance = null;
    SpeechStrategyManager()
    {
        Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Queue<Strategy> GetStrategyQueue(string key)
    {
        if(key.Contains(SpeechMatcher.KEY_recommend))
        {
            Queue<Strategy> queue = new Queue<Strategy>();
            queue.Enqueue(Strategy.Create(Strategy.Type.talk, 0, 0.640f));
            queue.Enqueue(Strategy.Create(Strategy.Type.wait));

            queue.Enqueue(Strategy.Create(Strategy.Type.talk, 0.781f, 3.029f));
            queue.Enqueue(Strategy.Create(Strategy.Type.wait));

            queue.Enqueue(Strategy.Create(Strategy.Type.talk, 3.169f, 5.185f));
            queue.Enqueue(Strategy.Create(Strategy.Type.wait));

            queue.Enqueue(Strategy.Create(Strategy.Type.talk, 5.339f, 7.072f));
            queue.Enqueue(Strategy.Create(Strategy.Type.wait));

            queue.Enqueue(Strategy.Create(Strategy.Type.talk, 7.197f, 8.836f));
            queue.Enqueue(Strategy.Create(Strategy.Type.wait));

            queue.Enqueue(Strategy.Create(Strategy.Type.talk, 8.930f, 10.022f));
            queue.Enqueue(Strategy.Create(Strategy.Type.wait));

            queue.Enqueue(Strategy.Create(Strategy.Type.talk, 10.147f, 11.972f));
            queue.Enqueue(Strategy.Create(Strategy.Type.wait));

            return queue;
        }

        return null;
    }
}
