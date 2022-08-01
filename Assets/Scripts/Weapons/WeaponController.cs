using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class WeaponController : MonoBehaviour
{
    private XRSocketInteractor _craftingSocket;

    private void Start()
    {
        // get a reference to the craftingsocket. Currently only works with one socket on the object. 
        _craftingSocket = transform.root.GetComponentInChildren<XRSocketInteractor>();
        if (!_craftingSocket) Debug.LogWarning(name + " has no Socket Interactor assigned!");        
    }

    public void OnAttach()
    {
        // get a reference to the gameobject that got attached into the socket
        GameObject attachedObject = _craftingSocket.GetOldestInteractableSelected().transform.gameObject;

        // check if the referencing was succesfull 
        if (attachedObject)
        {
            // deactivate the crafting socket
            _craftingSocket.gameObject.SetActive(false);

            // attach the socketed object to this gameobject and get rid of the individual parts like the rigidbody or the XRGrabInteractable of the attached object.
            attachedObject.transform.parent = this.transform;
            attachedObject.transform.position = _craftingSocket.transform.position;
            attachedObject.transform.rotation = _craftingSocket.transform.rotation;
            Destroy(attachedObject.GetComponent<XRGrabInteractable>());
            Destroy(attachedObject.GetComponent<Rigidbody>());
        }
        else Debug.LogWarning(name+": null reference while trying to attach an object!");
    }
}
 