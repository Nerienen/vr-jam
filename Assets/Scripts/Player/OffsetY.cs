using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetY : MonoBehaviour
{
    [SerializeField]
    Transform head;
    CharacterController cc;
    float _yOffSet = -0.7f;

    // Start is called before the first frame update
    void Start()
    {
        cc = transform.root.GetComponentInChildren<CharacterController>();
        if (!cc) Debug.LogWarning(this + " has no character controller assigned!");
        if (!head) Debug.LogWarning(this + " has no head transform assigned!");
    }

    // Update is called once per frame
    void Update()
    {
        MoveToOffset();
    }

    void MoveToOffset()
    {
        Vector3 headWorldSpace = head.TransformPoint(Vector3.zero);
        transform.position = new Vector3(transform.parent.position.x, headWorldSpace.y + _yOffSet, transform.parent.position.z);
    }
}
