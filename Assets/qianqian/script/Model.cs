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

    public void MoveZ(float scale)
    {
        float min = 1, max = 45;

        float z = transform.position.z / scale; 
        z = Mathf.Clamp(z, min, max);

       // UIHandler.Instance.ShowRecognizedSpeech("z = " + z + ", percent = " + scale);

        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }
}
