using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
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
        transform.Rotate(Vector3.down * delta, Space.World);
    }

    public void Scale(Vector3 scale)
    {
        transform.localScale = scale;
    }
}
