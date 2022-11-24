using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelAnimation : MonoBehaviour
{
    public static ModelAnimation Instance = null;
    ModelAnimation()
    {
        Instance = this;
    }

    public class Type
    {
        static public string smile = "smile";
    };



    public Animator animator;


    void Start()
    {
    }

    public void Play(string name)
    {
        animator.SetTrigger(name);
    }
}
