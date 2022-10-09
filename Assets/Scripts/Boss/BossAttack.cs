using System.Collections.Generic;
using Extensions;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VRJammies.Framework.Core.Boss
{
    public class BossAttack : MonoBehaviour
    {
        [SerializeField] private float attackCooldownInSeconds;
        [SerializeField] private float meleeAttackRange;
        [SerializeField] private float rangedAttackRange;
        [SerializeField] private List<AttackBase> meleeAttacks;
        [SerializeField] private List<AttackBase> rangedAttacks;

        private float nextAttackTime;
        private Boss boss;
        private PlayerFinder playerFinder;
        private bool hasPlayer;
        private Vector3 playerPos;
        private bool didAttack;
        private bool doneAttacking;
        
        private void Awake()
        {
            playerFinder = GetComponent<PlayerFinder>();
            if (playerFinder == null)
            {
                Debug.LogError($"Cannot find PlayerFinder");
            }

            boss = GetComponentInParent<Boss>();
            if (boss == null)
            {
                Debug.LogError($"Cannot find Boss script");
            }
            
            playerFinder.OnPlayerFound += OnPlayerFound;
            playerFinder.OnPlayerKnownPosition += OnPlayerVisible;
            playerFinder.OnPlayerLost += OnPlayerLost;
        }

        private void Start()
        {
            foreach (var attack in rangedAttacks)
            {
                attack.DoneAttacking += OnDoneAttacking;
            }
            
            foreach (var attack in meleeAttacks)
            {
                attack.DoneAttacking += OnDoneAttacking;
            }
        }

        private void Update()
        {
            if (didAttack && doneAttacking)
            {
                boss.state = BehaviorState.Idle;
                didAttack = false;
                doneAttacking = false;
            }
            
            if (hasPlayer)
            {
                TryAttack();
            }
        }

        private void TryAttack()
        {
            if (Time.time < nextAttackTime || (didAttack && !doneAttacking)) return;

            var distToPlayer = (playerPos - transform.position).magnitude;
            AttackBase attack = null;
            if (distToPlayer < meleeAttackRange)
            {
                attack = meleeAttacks.Choose();
            }
            else if (distToPlayer < rangedAttackRange)
            {
                attack = rangedAttacks.Choose();
            }

            if (attack == null || !attack.CanAttack()) return;
            boss.state = BehaviorState.Attacking;
            attack.Attack();
            didAttack = true;
            nextAttackTime = Time.time + attackCooldownInSeconds;
        }

        private void OnDoneAttacking()
        {
            doneAttacking = true;
        }

        private void OnPlayerLost(Player.Player player)
        {
            hasPlayer = false;
            playerPos = player.transform.position;
        }

        private void OnPlayerFound(Player.Player player)
        {
            hasPlayer = true;
            playerPos = player.transform.position;
        }

        private void OnPlayerVisible(Player.Player player)
        {
            hasPlayer = true;
            playerPos = player.transform.position;
        }

        public void UpdateAttackList()
        {
            var attacksToRemove = new List<AttackBase>();
            foreach (var attack in meleeAttacks)
            {
                if (attack == null || attack.enabled == false)
                {
                    attacksToRemove.Add(attack);
                }
            }
            foreach (var attack in attacksToRemove)
            {
                meleeAttacks.Remove(attack);
            }
            
            attacksToRemove.Clear();
            foreach (var attack in rangedAttacks)
            {
                if (attack == null || attack.enabled == false)
                {
                    attacksToRemove.Add(attack);
                }
            }
            foreach (var attack in attacksToRemove)
            {
                rangedAttacks.Remove(attack);
            }
            
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(transform.position, transform.up, rangedAttackRange);
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, transform.up, meleeAttackRange);
        }
#endif
    }
}