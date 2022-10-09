using System.Collections;
using UnityEngine;

namespace VRJammies.Framework.Core.Boss
{
    public class LimbDeathHandler : MonoBehaviour
    {
        [SerializeField] private Rigidbody limbRb;
        [SerializeField] private Transform launchTransform;
        [SerializeField] private float launchForce;
        [SerializeField] private float launchTorque = 100f;
        [Range(0, 60)]
        [SerializeField] private float disappearInSeconds;

        private BossAttack bossAttack;
        private AttackBase limbAttack;

        private void Awake()
        {
            limbRb.constraints = RigidbodyConstraints.FreezeAll;
            bossAttack = GetComponentInParent<BossAttack>();
            limbAttack = GetComponentInChildren<AttackBase>(true);
        }

        public void OnLimbDeath()
        {
            // Turn on RB
            limbRb.constraints = RigidbodyConstraints.None;
            
            //// Launch arm
            // Remove parent so the limb flies freely
            limbRb.transform.parent = null;
            limbRb.AddForce(launchTransform.forward * launchForce);
            limbRb.AddTorque(launchTransform.up * launchTorque);
            
            // Remove limb attack from list
            if (limbAttack)
            {
                limbAttack.enabled = false;
            }
            if (bossAttack)
            {
                bossAttack.UpdateAttackList();
            }
            
            // Start disappear coroutine/timer/invoke
            StartCoroutine(DeathCoroutine());
            
            // TODO: Trigger particle FX
        }

        private IEnumerator DeathCoroutine()
        {
            yield return new WaitForSeconds(disappearInSeconds);
            Destroy(gameObject);
        }
    }
}
