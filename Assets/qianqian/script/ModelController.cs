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
        //ModelAnimationController.Instance.Play(ModelAnimation.Type.idle);
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

    public void MoveY(float delta)
    {
        model.MoveY(delta);
    }

    public void HandleTouchZ(float scale)
    {
        model.MoveZ(scale);
    }

    public GameObject GetModelHead()
    {
        return model.GetModelHead();
    }

    public void HandleScale(float scale)
    {
        scale = Mathf.Clamp(scale, 1f, 3f);

        model.HandleScale(scale);

        CameraViewController.Instance.InZoomMode = scale >= 2f;
    }

    public void ResetPosition()
    {
        model.ResetPosition();
    }

    public void LerpModelYtoOrigin(float delta)
    {
        model.LerpModelYtoOrigin(delta);
    }

    public void LerpModelYtoTop(float delta)
    {
        model.LerpModelYtoTop(delta);
    }
}
