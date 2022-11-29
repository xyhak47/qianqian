using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelAnimation : MonoBehaviour
{
    public class Type
    {
        static public string smile = "smile";
        static public string talk_long = "talk_long";
        static public string talk_short = "talk_short";
    };



    private Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Play(string name)
    {
        animator.SetTrigger(name);
    }
}
