using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RotateOnGrabLeftHand : MonoBehaviour
{
    [SerializeField]
    private Transform _grabPointTransform;
    private Vector3 _initialRotation;
    [SerializeField]
    private Vector3 _targetRotation;

    private void Start()
    {
        if (!_grabPointTransform) Debug.LogWarning(this + " has no grabpoint assigned");
        else _initialRotation = _grabPointTransform.rotation.eulerAngles;
    }

    public void SetNewRotation(SelectEnterEventArgs args) 
    {
        if (_grabPointTransform && _targetRotation != Vector3.zero && args.interactorObject.transform.gameObject.tag == "LeftHand") 
        {
            _grabPointTransform.localEulerAngles = _targetRotation;
        }else _grabPointTransform.localEulerAngles = _initialRotation;
    }
}
