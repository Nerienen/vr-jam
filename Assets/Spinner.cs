using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{

    float spinSpeed = 15F;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(GetComponent<Collider>().bounds.center, Vector3.up, spinSpeed * Time.deltaTime);
    }
}
