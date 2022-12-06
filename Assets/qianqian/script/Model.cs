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

    public void MoveZ(float percent)
    {
        // from -40 ~ 10
        float min = -40, max = 10;

        float z = transform.position.z * percent;
        z = Mathf.Clamp(z, min, max);

        transform.position.Set(transform.position.x, transform.position.y, z);
    }
}
