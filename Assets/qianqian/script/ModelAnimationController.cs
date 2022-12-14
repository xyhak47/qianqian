using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelAnimationController : MonoBehaviour
{
    public static ModelAnimationController Instance = null;
    ModelAnimationController()
    {
        Instance = this;
    }

    public ModelAnimation model_ani;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(string name)
    {
        model_ani.Play(name);
    }

    private IEnumerator _Play(string name, float duration, Action callback)
    {
        //Play(name);
        //yield return new WaitForSeconds(duration);
        //Play(ModelAnimation.Type.idle);

        SetTalkLayerWeight(1);
        yield return new WaitForSeconds(duration);
        SetTalkLayerWeight(0);

        if (callback != null)
        {
            callback();
        }
    }

    public void Play(string name, float duration, Action callback)
    {
        StartCoroutine(_Play(name, duration, callback));
    }

    public void SetTalkLayerWeight(float weight)
    {
        model_ani.SetTalkLayerWeight(weight);
    }

    public void ResetSelf()
    {
        SetTalkLayerWeight(0);
    }
    
}
