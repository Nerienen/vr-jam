using System;
using UnityEngine;

namespace VRJammies.Framework.Core.Boss
{
    public class AttackBase : MonoBehaviour, IAttack
    {
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