using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace VRJammies.Framework.Core.Boss
{
    public class BossLocomotion : MonoBehaviour
    {
        [Header("Agent Config")]
        [SerializeField] private float bossSpeed;
        [SerializeField] private float bossHeight;
        [SerializeField] private float bossGirth;
        [SerializeField] private float minDistanceToPlayer;
        [SerializeField] private float searchTimeInSeconds;
        [SerializeField] private float wanderRadius;
        [SerializeField] private float wanderIntervalInSeconds;
        [Range(0, 1)]
        [SerializeField] private float randomPatrolChance;

        [Header("Events")]
        [Range(0, 300)]
        [SerializeField] private float locomotionChangeMultiplier = 1f;
        public UnityEvent<float> onBossLocomotionChange;
        public UnityEvent onBossLocomotionStart;
        public UnityEvent onBossLocomotionStop;

        private Boss boss;
        private NavMeshAgent agent;
        private PlayerFinder playerFinder;
        private float playerLostTime;
        private float nextWanderTime;
        private List<PatrolPoint> patrolPoints;

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

            patrolPoints = FindObjectsOfType<PatrolPoint>().ToList();

            playerFinder.OnPlayerFound += OnPlayerFound;
            playerFinder.OnPlayerKnownPosition += OnPlayerStillVisible;
            playerFinder.OnPlayerLost += OnPlayerLost;
        }

        private void Start()
        {
            SetUpAgent();
        }

        private void OnValidate()
        {
            SetUpAgent();
        }

        private void Update()
        {
            switch (boss.state)
            {
                // No locomotion in these states
                case BehaviorState.Attacking:
                case BehaviorState.Stunned:
                    if (!agent.isStopped)
                    {
                        onBossLocomotionStop.Invoke();
                        onBossLocomotionChange.Invoke(0f);
                    }
                    agent.isStopped = true;
                    break;
                case BehaviorState.PlayerLost:
                    SearchForPlayer();
                    break;
                case BehaviorState.PlayerFound:
                    MoveToTarget();
                    break;
                case BehaviorState.Idle:
                    TryToWander();
                    MoveToTarget();
                    break;
            }
        }

        [ContextMenu("Set up agent")]
        private void SetUpAgent()
        {
            agent = GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                Debug.LogError($"Cannot find NavMeshAgent");
                return;
            }
            
            agent.speed = bossSpeed;
            agent.height = bossHeight;
            agent.radius = bossGirth;
            agent.stoppingDistance = minDistanceToPlayer;
            agent.updatePosition = false;  // We're moving the agent manually
        }

        private void MoveToTarget()
        {
            if ((agent.destination - transform.position).magnitude <= minDistanceToPlayer)
            {
                if (!agent.isStopped)
                {
                    onBossLocomotionStop.Invoke();
                    onBossLocomotionChange.Invoke(0f);
                }
                agent.isStopped = true;
                return;
            }
            if (agent.isStopped)
            {
                onBossLocomotionStart.Invoke();
            }
            agent.isStopped = false;
            onBossLocomotionChange.Invoke((agent.nextPosition - transform.position).magnitude * locomotionChangeMultiplier);
            // Debug.Log($"Locomotion change: {(agent.nextPosition - transform.position).magnitude * locomotionChangeMultiplier}");
            transform.position = agent.nextPosition;
            // See here to couple locomotion with animation:
            // https://docs.unity3d.com/Manual/nav-CouplingAnimationAndNavigation.html
        }

        private void TryToWander()
        {
            if (Time.time < nextWanderTime) return;
            Vector3 pos;
            if (Random.value > randomPatrolChance && patrolPoints.Count > 0)
            {
                pos = patrolPoints.Choose().transform.position;
            }
            else
            {
                var hit = GetRandomNavMeshPosition();
                pos = hit.position;
            }
            agent.SetDestination(pos);
            nextWanderTime = Time.time + wanderIntervalInSeconds;
        }

        private NavMeshHit GetRandomNavMeshPosition()
        {
            var randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += transform.position;
            NavMesh.SamplePosition(randomDirection, out var hit, wanderRadius, 1);
            return hit;
        }

        private void SearchForPlayer()
        {
            if (agent.hasPath)
            {
                MoveToTarget();
                playerLostTime = Time.time + searchTimeInSeconds;

                var rotationDirection = Mathf.Sin(Time.time) > 0 ? 1 : -1;
                transform.Rotate(transform.up, agent.angularSpeed * Time.deltaTime * rotationDirection);
                return;
            }
            
            if (Time.time > playerLostTime)
            {
                boss.state = BehaviorState.Idle;
            }
            // TODO: Replace with a "searching" animation
            transform.Rotate(transform.up, agent.angularSpeed * Time.deltaTime);
        }

        private void SetTarget(Vector3 target)
        {
            agent.SetDestination(target);
        }

        private void OnPlayerFound(Player.Player player)
        {
            SetTarget(player.transform.position);
            
            // !!!
            // Had to use the old C# syntax here to be compatible with Unity 2020.3. Old if query:
            // if (boss.state is BehaviorState.Idle or BehaviorState.PlayerLost)
            // TODO: Test if the behaviour is still working.
            // !!!
            
            if (boss.state is BehaviorState.Idle || boss.state is BehaviorState.PlayerLost)
            {
                boss.state = BehaviorState.PlayerFound;
            }
        }

        private void OnPlayerStillVisible(Player.Player player)
        {
            SetTarget(player.transform.position);
        }

        private void OnPlayerLost(Player.Player player)
        {
            SetTarget(player.transform.position);
            boss.state = BehaviorState.PlayerLost;
            playerLostTime = Time.time + searchTimeInSeconds;
        }
    }
}