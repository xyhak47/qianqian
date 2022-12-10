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

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    void Start()
    {
    }

    public void Play(string name)
    {
        animator.SetTrigger(name);
    }
}
