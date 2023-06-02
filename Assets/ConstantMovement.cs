using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConstantMovement : MonoBehaviour
{
    public float speed = 1f;
    public float movementRange = 1f;

    private float startPositionZ;

    private void Start()
    {
        startPositionZ = transform.position.z;
    }

    private void Update()
    {
        float targetPositionZ = startPositionZ + movementRange;
        float t = Mathf.PingPong(Time.time * speed, 1f);
        float newPositionZ = Mathf.Lerp(startPositionZ, targetPositionZ, t);

        Vector3 newPosition = transform.position;
        newPosition.z = newPositionZ;
        transform.position = newPosition;
    }
}
