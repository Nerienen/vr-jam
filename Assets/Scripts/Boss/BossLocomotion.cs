using UnityEngine;
using UnityEngine.AI;

namespace Boss
{
    public class BossLocomotion : MonoBehaviour
    {
        [Header("Agent Config")]
        [SerializeField] private float bossSpeed;
        [SerializeField] private float bossHeight;
        [SerializeField] private float bossRadius;
        
        private NavMeshAgent agent;
        private PlayerFinder playerFinder;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                Debug.LogError($"Cannot find NavMeshAgent");
            }

            playerFinder = GetComponent<PlayerFinder>();
            if (playerFinder == null)
            {
                Debug.LogError($"Cannot find PlayerFinder");
            }

            playerFinder.OnPlayerFound += OnPlayerFound;
            playerFinder.OnPlayerKnownPosition += OnPlayerStillVisible;
            playerFinder.OnPlayerLost += OnPlayerLost;
        }

        private void Start()
        {
            SetUpAgent();
        }

        [ContextMenu("Set up agent")]
        private void SetUpAgent()
        {
            agent.speed = bossSpeed;
            agent.height = bossHeight;
            agent.radius = bossRadius;
        }

        private void MoveToTarget(Vector3 target)
        {
            agent.SetDestination(target);
        }

        private void OnPlayerFound(Vector3 playerPos)
        {
            MoveToTarget(playerPos);
        }

        private void OnPlayerStillVisible(Vector3 playerPos)
        {
            MoveToTarget(playerPos);
        }

        private void OnPlayerLost(Vector3 playerLastKnownPos)
        {
            MoveToTarget(playerLastKnownPos);
        }
    }
}