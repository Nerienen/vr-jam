using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using VRJammies.Framework.Core.Health;

namespace VRJammies.Framework.Core.Crafting 
{ 
public class WeaponController : MonoBehaviour
{
        private List<DamageCollider> _damageColliders = new List<DamageCollider>();

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


                if (attachedObject.GetComponentInChildren<DamageCollider>()) 
                {
                    // Reference the damageCollider script
                    DamageCollider damageCollider = attachedObject.GetComponentInChildren<DamageCollider>();

                    // If its not already in the reference list of damage colliders, add it
                    if (!_damageColliders.Contains(damageCollider))
                        _damageColliders.Add(damageCollider);

                    // Activate the damage collider script. 
                    damageCollider.enabled = true;
                    damageCollider.SetCraftingSocket(_craftingSocket.transform.gameObject);
                }else Debug.Log(attachedObject + " has no damage collider script attached");
            }
            else Debug.LogWarning(name+": null reference while trying to attach an object!");
        }

        private void OnCollisionEnter(Collision collision)
        {
            //Debug.Log(name+" has collided with: "+collision.gameObject.name);
            foreach (DamageCollider damageCollider in _damageColliders)
            {
                damageCollider.OnCollisionEvent(collision);
            }
            
        }
    }
}
