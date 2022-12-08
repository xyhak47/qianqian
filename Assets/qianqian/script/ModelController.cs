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

    public void HandleTouchXY(Vector2 delta)
    {
        if(model.InScale)
        {
            model.Rotate(delta);
        }
        else
        {

        }
    }

    public void HandleTouchZ(float scale)
    {
        model.MoveZ(scale);
    }

    public GameObject GetModelHead()
    {
        return model.GetModelHead();
    }
}
