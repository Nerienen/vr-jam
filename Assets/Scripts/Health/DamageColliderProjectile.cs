using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRJammies.Framework.Core.Health
{
    [RequireComponent(typeof(Rigidbody))]
    public class DamageColliderProjectile : DamageCollider
    {
        [SerializeField]
        private GameObject _projectileSpawner;
        [SerializeField]
        private string _layerToIgnore;

        protected override void Start()
        {
            // Initiliaze values and null checks
            _minForce = 0f;
            _destroyOnDamaging = false;

            if (!ColliderRigidbody) ColliderRigidbody = this.GetComponentInChildren<Rigidbody>();
            if (!DamagingCollider)
            {
                Debug.Log(this+" has no specific Damage collider defined, will try to find one on this as default");
                DamagingCollider = GetComponentInChildren<Collider>();
            }
            if (!_projectileSpawner) Debug.LogWarning(this+" has no projectile spawner assigned!");

        }

        // Call the custom public collision event in case this object can have collision events
        protected override void OnCollisionEnter(Collision collision)
        {
            if (!this.isActiveAndEnabled || collision.gameObject.layer == LayerMask.NameToLayer(_layerToIgnore))
            {

                return;
            }

            OnCollisionEvent(collision);
            DeactivateThis();
        }

        public void DeactivateThis()
        {
            if (!_projectileSpawner)
            {
                Destroy(this.gameObject);
                return;
            }


            this.transform.parent = _projectileSpawner.transform;
            this.transform.position = _projectileSpawner.transform.position;
            this.transform.rotation = _projectileSpawner.transform.rotation;
            this.gameObject.SetActive(false);
        }
        public void SetSpawner(GameObject spawner) 
        {
            _projectileSpawner = spawner;
        }
    }
}