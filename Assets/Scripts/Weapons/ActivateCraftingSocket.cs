using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRJammies.Framework.Core.Crafting
{    public class ActivateCraftingSocket : MonoBehaviour
    {
        [SerializeField]
        float _despawnTimer = 3f;
        [SerializeField]
        float _timer;
        Vector3 LastPosition;

        private void Update()
        {
            var currentPosition = transform.position;

            if (currentPosition == LastPosition)
            {
                _timer += Time.deltaTime;
            }
            else _timer = 0;

            LastPosition = currentPosition;

            if (_timer > _despawnTimer || transform.position.y < -5)
                gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _timer = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Check if the trigger is a XR Socket and not active yet
            if (other.GetComponent<XRSocketInteractor>() && !other.GetComponent<XRSocketInteractor>().socketActive)
            {
                XRSocketInteractor _socket = other.GetComponent<XRSocketInteractor>();

                // Set the socket active, color it the material of this adhesive and destroy this object. 
                _socket.socketActive = true;
                _socket.GetComponent<MeshRenderer>().material = GetComponentInChildren<MeshRenderer>().material;
                this.gameObject.SetActive(false);
            }
        }
    }
}
