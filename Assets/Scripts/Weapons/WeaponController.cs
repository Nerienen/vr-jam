using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class WeaponController : MonoBehaviour
{

    public void OnAttach(SelectEnterEventArgs obj)
    {
        // Reference the Socket and the grabbable that was attached
        var _craftingSocket = obj.interactorObject;
        GameObject attachedObject = obj.interactableObject.transform.gameObject;


        // Check if the referencing was succesfull 
        if (attachedObject != null)
        {
            // Deactivate the crafting socket
            _craftingSocket.transform.gameObject.SetActive(false);

            // Attach the socketed object to this gameobject and get rid of the individual parts like the rigidbody or the XRGrabInteractable of the attached object.
            Destroy(attachedObject.GetComponent<XRGrabInteractable>());
            Destroy(attachedObject.GetComponent<Rigidbody>());
            attachedObject.transform.parent = this.transform;
            attachedObject.transform.position = _craftingSocket.transform.position;
            attachedObject.transform.rotation = _craftingSocket.transform.rotation;

            // Destroy the crafting socket after use
            //Destroy(_craftingSocket.transform.gameObject);
        }
        else Debug.LogWarning(name+": null reference while trying to attach an object!");
    }
}
 