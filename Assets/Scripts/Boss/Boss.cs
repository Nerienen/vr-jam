using UnityEngine;

namespace Boss
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