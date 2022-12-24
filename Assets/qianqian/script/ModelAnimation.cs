using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelAnimation : MonoBehaviour
{
    public class Type
    {
        static public string idle = "idle";

        static public string smile = "smile";

        static public string talk_long = "talk_long";
        static public string talk_short = "talk_short";

        static public string eye = "eye";
    };



    private Animator animator;


    private float target_weight_talk = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    void Start()
    {
    }

    private void Update()
    {
        LerpTalkWeight();
    }

    public void Play(string name)
    {
        animator.SetTrigger(name);
    }

    public void SetTalkLayerWeight(float weight)
    {
        target_weight_talk = weight;
    }

    private void LerpTalkWeight()
    {
        float current_weight = animator.GetLayerWeight(animator.GetLayerIndex("talk"));
        float lerp = Mathf.Lerp(current_weight, target_weight_talk, Time.deltaTime * 10f);
        animator.SetLayerWeight(animator.GetLayerIndex("talk"), lerp);
    }
}
