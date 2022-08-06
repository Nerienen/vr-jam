using UnityEngine;

namespace VRJammies.Framework.Core.Boss
{
    public class Boss : MonoBehaviour
    {
        public BehaviorState state;

        private void Start()
        {
            state = BehaviorState.Idle;
        }
    }
}