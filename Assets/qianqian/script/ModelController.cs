using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelController : MonoBehaviour
{
    public static ModelController Instance = null;
    ModelController()
    {
        Instance = this;
    }

    public Model model;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Rotate(float delta)
    {
        model.Rotate(delta);
    }

    public void MoveZ(float scale)
    {
        model.MoveZ(scale);
    }
}
