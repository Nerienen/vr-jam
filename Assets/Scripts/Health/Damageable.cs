using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Enum to consider damage types and weaknesses
public enum DamageForm { NoType, Blunt, Slash, PlayerDamage, Acid, Water, Fire }

namespace VRJammies.Framework.Core.Health
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField]
        private int _currentHealth = 0;
        [SerializeField]
        private int _startingHealth = 1;

        private HealthController healthController;

        /// <summary>
        /// Advanced damage calculations including weaknesses and resistances. 
        /// </summary>
        [SerializeField]
        private DamageForm _weakness;

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

        [Tooltip("Subbed to by PlayerHealthUIController to update Player UI")]
        public UnityEvent<int> onPlayerDamaged;

        private bool destroyed = false;


        public int GetStartingHealth()
        {
            return _startingHealth;
        }

        private void Start()
        {
            healthController = transform.root.GetComponentInChildren<HealthController>();
            //if (!healthController) Debug.LogWarning(this+" found no health controller on this object!");
            _currentHealth = _startingHealth;
        }

        public void DealDamage(int damageAmount, DamageForm damageType, DamageCollider damageDealer)
        {

            if (destroyed || !this.isActiveAndEnabled)
            {
                return;
            }

            DamageCalculation(damageAmount, damageType, damageDealer);

            if (_currentHealth <= 0)
            {
                DestroyThis();
            }
        }
        public void DealDamage(int damageAmount, DamageForm damageType)
        {

            if (destroyed || !this.isActiveAndEnabled)
            {
                return;
            }

            DamageCalculation(damageAmount, damageType, null);

            if (_currentHealth <= 0)
            {
                DestroyThis();
            }
        }

        public void TakeCollisionDamage(int damageAmount)
        {

            if (destroyed)
            {
                return;
            }

            _currentHealth -= damageAmount;

            if (_currentHealth <= 0)
            {
                DestroyThis();
            }

        }

        private void DamageCalculation(int damageAmount, DamageForm damageType, DamageCollider damageDealer)
        {
            // Value to calculate the actual amount of damage to take
            int damageTaken;

            // If the correct damage type was applied, set damage taken to input damage amount, otherwise set it to zero
            if (damageType == _weakness)
            {
                damageTaken = damageAmount;
            }
            else damageTaken = 0;

            // If there is no specified type assigned "NoType", also pass the input damage amount as damage taken
            if (_weakness == DamageForm.NoType)
            {
                damageTaken = damageAmount;
            }
            //to send info to PlayerHealthUIController
            else if(_weakness == DamageForm.PlayerDamage)
            {
                if(onPlayerDamaged != null)
                {
                    onPlayerDamaged.Invoke(damageAmount);
                }
            }

            // Substract the damage taken value from the current health
            _currentHealth -= damageTaken;

            if (damageTaken > 0 && damageDealer.ShouldDestroy())
                damageDealer.OnDestroyThis();
        }

        public void SetDestroyOnDeath(bool destroyOnDeath)
        {
            DestroyOnDeath = destroyOnDeath;
        }

        public void DestroyThis()
        {
            _currentHealth = 0;
            destroyed = true;
            if(healthController) healthController.DealDamage(1);

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

            // Call events
            if (onRespawn != null)
            {
                onRespawn.Invoke();
            }
        }
    }
}