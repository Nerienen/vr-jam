using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HolsterSocketController : MonoBehaviour
{
    XRGrabInteractable movementType;

    private void Start()
    {
        
    }
    public void OnAttach(SelectEnterEventArgs args) 
    {
        args.interactableObject.transform.root.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        if (args.interactableObject.transform.root.GetComponentInChildren<DestroyOnStandStill>()) 
        {
            args.interactableObject.transform.root.GetComponentInChildren<DestroyOnStandStill>().enabled = false;
        }
    }
    public void OnDetach(SelectExitEventArgs args)
    {
        args.interactableObject.transform.root.localScale = new Vector3(1f, 1f, 1f);

        if (args.interactableObject.transform.root.GetComponentInChildren<DestroyOnStandStill>())
        {
            args.interactableObject.transform.root.GetComponentInChildren<DestroyOnStandStill>().enabled = true;
        }
    }
}
