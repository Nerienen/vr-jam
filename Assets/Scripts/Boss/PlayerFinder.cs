using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VRJammies.Framework.Core.Boss
{
    public class PlayerFinder : MonoBehaviour
    {
        [Header("Behavior")]
        [SerializeField] private float secondsUntilLost = 5f;
        
        [Header("Vision")]
        [SerializeField] private float visionDistance = 100f;
        [SerializeField] private float visionConeAngle = 120f;
        [SerializeField] private bool useRaycast = true;
        [SerializeField] private LayerMask visionBlockingLayers;
        [SerializeField] private Transform eyesTransform;

        [Header("Performance")]
        [SerializeField] private bool dontCheckEveryFrame = true;
        [SerializeField] private float checkPlayerPositionDelayInSeconds = 0.05f;

        [Header("Editor")]
        [SerializeField] private bool alwaysDrawVisionCone = true;

        private float nextPlayerPositionCheck;
        private float lastFoundTime;
        private Vector3 lastKnownPlayerPosition;
        private Player.Player player;
        private bool hasFoundPlayer;

        public event Action<Vector3> OnPlayerFound;
        public event Action<Vector3> OnPlayerKnownPosition;
        public event Action<Vector3> OnPlayerLost;

        private void Awake()
        {
            player = FindObjectOfType<Player.Player>();
            if (player == null)
            {
                Debug.LogError($"Couldn't find player on awake");
            }
        }

        private void Update()
        {
            if (dontCheckEveryFrame && Time.time < nextPlayerPositionCheck) return;
            UpdatePlayerPos();
        }

        private void UpdatePlayerPos()
        {
            nextPlayerPositionCheck = Time.time + checkPlayerPositionDelayInSeconds;
            
            var playerPos = player.transform.position;
            var eyesPos = eyesTransform.position;
            var dirToPlayer = playerPos - eyesPos;
            var distToPlayer = dirToPlayer.magnitude;
            var angleToPlayer = Vector3.Angle(eyesTransform.forward, dirToPlayer);

            var foundPlayer = distToPlayer < visionDistance;
            foundPlayer = foundPlayer && (hasFoundPlayer || angleToPlayer < visionConeAngle / 2);
            if (foundPlayer && useRaycast)
            {
                if (Physics.Raycast(
                    eyesPos,
                    playerPos - eyesPos,
                    out var hit,
                    visionDistance,
                    visionBlockingLayers
                ))
                {
                    var foundGo = hit.transform.GetComponentInParent<Player.Player>();
                    if (foundGo == null)
                    {
                        foundPlayer = false;
                    }
                }
                else
                {
                    foundPlayer = false;
                }
            }
            
            if (foundPlayer)
            {
                lastKnownPlayerPosition = playerPos;
                if (!hasFoundPlayer)
                {
                    OnPlayerFound?.Invoke(lastKnownPlayerPosition);
                }
                else
                {
                    OnPlayerKnownPosition?.Invoke(lastKnownPlayerPosition);
                }
                
                hasFoundPlayer = true;
                lastFoundTime = Time.time;
            }
            
            if (!foundPlayer && hasFoundPlayer && Time.time - lastFoundTime > secondsUntilLost)
            {
                OnPlayerLost?.Invoke(lastKnownPlayerPosition);
                hasFoundPlayer = false;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(!alwaysDrawVisionCone) return;
            DrawVisionCone();
        }

        private void OnDrawGizmosSelected()
        {
            if(alwaysDrawVisionCone) return;
            DrawVisionCone();
        }

        private void DrawVisionCone()
        {
            Handles.color = new Color(255, 0, 0, .1f);
            var eyesPos = eyesTransform.position;
            var start = Quaternion.AngleAxis(-visionConeAngle / 2f, eyesTransform.up) * eyesTransform.forward;
            Handles.DrawSolidArc(
                eyesPos,
                eyesTransform.up,
                start,
                visionConeAngle,
                visionDistance
            );
        }
#endif
    }
}