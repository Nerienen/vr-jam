using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRJammies.Framework.Core.Health
{
    public class HealthController : MonoBehaviour
    {
        [SerializeField]
        private int _currentHealth = 1;
        private int _startingHealth = 0;
        [SerializeField]
        private Transform _damageablesTransform;

        /// <summary>
        /// Destroy this object on Death? False if need to respawn.
        /// </summary>
        [Tooltip("Destroy this object on Death? False if need to respawn.")]
        public bool DestroyOnDeath = false;

        /// <summary>
        /// If true the object will be reactivated according to RespawnTime
        /// </summary>
        [Tooltip("If true the object will be reactivated according to RespawnTime")]
        public bool Respawn = false;

        /// <summary>
        /// If Respawn true, this gameObject will reactivate after RespawnTime. In seconds.
        /// </summary>
        [Tooltip("If Respawn true, this gameObject will reactivate after RespawnTime. In seconds.")]
        public float RespawnTime = 3f;

        [Tooltip("Optional Event to be called once health is <= 0")]
        public UnityEvent onDestroyed;

        [Tooltip("Optional Event to be called once the object has been respawned, if Respawn is true and after RespawnTime")]
        public UnityEvent onRespawn;

        private bool destroyed = false;

        private void Start()
        {
            _startingHealth = _damageablesTransform.childCount;
            _currentHealth = _startingHealth;


            if (!_damageablesTransform)
                Debug.LogWarning(name+" has no damageable parent object assigned");
        }

        public void DealDamage(int damageAmount)
        {

            if (destroyed)
            {
                return;
            }

            DamageCalculation(damageAmount);

            if (_currentHealth <= 0)
            {
                DestroyThis();
            }
        }

        private void DamageCalculation(int damageAmount)
        {
            // Substract the damage taken value from the current health
            _currentHealth -= damageAmount;
        }

        public void DestroyThis()
        {
            _currentHealth = 0;
            destroyed = true;

            // Invoke Callback Event
            if (onDestroyed != null)
            {
                onDestroyed.Invoke();
            }

            if (DestroyOnDeath)
            {
                Destroy(this.gameObject);
            }
            else if (Respawn)
            {
                StartCoroutine(RespawnRoutine(RespawnTime));
            }
        }
        
        private IEnumerator RespawnRoutine(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            _currentHealth = _startingHealth;
            destroyed = false;

            // Call Respawn Event
            if (onRespawn != null)
            {
                onRespawn.Invoke();
            }
        }
    }
}