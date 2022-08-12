using System;
using UnityEngine;
using UnityEngine.Events;

namespace VRJammies.Framework.Core.Boss
{
    public class AttackBase : MonoBehaviour, IAttack
    {
        public UnityEvent onAttack;
        public UnityEvent onChargeUp;
        public event Action DoneAttacking;
        
        public virtual bool CanAttack()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Attack()
        {
            throw new System.NotImplementedException();
        }

        protected virtual void OnDoneAttacking()
        {
            DoneAttacking?.Invoke();
        }
    }
}