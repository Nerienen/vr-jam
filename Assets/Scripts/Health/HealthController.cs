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

        [Tooltip("Optional Event to be called once health is <= 0")]
        public UnityEvent onDestroyed;

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
        }
    }
}