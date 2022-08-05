using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketController : MonoBehaviour
{
    public void OnAttach(SelectEnterEventArgs args) 
    {
        args.interactableObject.transform.gameObject.GetComponent<XRGrabInteractable>().movementType = XRBaseInteractable.MovementType.Instantaneous;
        args.interactableObject.transform.root.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }
    public void OnDetach(SelectExitEventArgs args)
    {
        args.interactableObject.transform.gameObject.GetComponent<XRGrabInteractable>().movementType = XRBaseInteractable.MovementType.Kinematic;
        args.interactableObject.transform.root.localScale = new Vector3(1f, 1f, 1f);
    }
}
