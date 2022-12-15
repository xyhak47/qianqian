using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    [HideInInspector]
    public bool InScale = false;

    private GameObject head = null;

    private Vector3 origin_postion;

    float min = -38f, max = -4f;

    private Vector3 top_postion;

    private void Awake()
    {
        head = transform.Find("head").gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        origin_postion = transform.position;

        top_postion = origin_postion;
        top_postion.y = min;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Rotate(Vector2 delta)
    {
        transform.Rotate(Vector3.down * delta.x, Space.World);
        transform.Rotate(Vector3.right * delta.y, Space.World);
    }

    public void MoveZ(float scale)
    {
        float min = 1, max = 45;

        float z = transform.position.z / scale; 
        z = Mathf.Clamp(z, min, max);

       // UIHandler.Instance.ShowRecognizedSpeech("z = " + z + ", percent = " + scale);

        transform.position = new Vector3(transform.position.x, transform.position.y, z);

        InScale = z == max;
    }

    public GameObject GetModelHead()
    {
        return head;
    }

    public void HandleScale(float scale)
    {
        transform.localScale *= scale;
    }

    public void MoveY(float delta)
    {
        float x = transform.position.x;
        float z = transform.position.z;
        float y = transform.position.y;
        y += delta;

        y = Mathf.Clamp(y, min, max);
        transform.position = new Vector3(x, y, z);
    }

    public void ResetPosition()
    {
        transform.position = origin_postion;
    }


    public void LerpModelYtoOrigin(float delta)
    {
        transform.position = Vector3.Lerp(transform.position, origin_postion, delta);
    }

    public void LerpModelYtoTop(float delta)
    {
        transform.position = Vector3.Lerp(transform.position, top_postion, delta);
    }
}
