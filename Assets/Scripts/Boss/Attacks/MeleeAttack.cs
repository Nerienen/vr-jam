using UnityEngine;

namespace VRJammies.Framework.Core.Boss
{
    public class MeleeAttack : AttackBase
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Transform forwardTransform;
        [SerializeField] private PlayerFinder playerFinder;
        [SerializeField] private float attackDelay = 1f;
        [SerializeField] private float rotationSpeed = 1f;
        [SerializeField] private Quaternion rotationOffset;
        
        private GameObject target;
        private float nextAttackTime;
        private bool canAttack;
        private Quaternion originalLocalRotation;

        private static readonly int AttackState = Animator.StringToHash("Attack");
        private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
        private static readonly int IsCharging = Animator.StringToHash("IsCharging");

        private void Start()
        {
            if (!playerFinder)
            {
                Debug.LogWarning(this.name + " has no player finder assigned!");
            }
            else
            {
                playerFinder.OnPlayerFound += OnPlayerFound;
                playerFinder.OnPlayerLost += OnPlayerLost;
            }

            originalLocalRotation = transform.localRotation;
        }

        private void Update()
        {
            if (target)
            {
                var isAttacking = animator.GetBool(IsAttacking);
                if (!isAttacking)
                {
                    var dir = target.transform.position - transform.position;
                    // HACK: Use the inverse rotation of the offset to compensate when
                    // the limb root transform's forward direction is not Vector3.forward
                    var rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z)) *
                                         Quaternion.Inverse(rotationOffset);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
                }

                // Use IsCharging for any wind-up animations
                animator.SetBool(IsCharging, true);
                canAttack = Time.time > nextAttackTime;
            }
            else
            {
                animator.SetBool(IsCharging, false);
                nextAttackTime = Time.time + attackDelay;
                transform.localRotation = Quaternion.Slerp(
                    transform.localRotation,
                    originalLocalRotation,
                    rotationSpeed * Time.deltaTime
                );
                canAttack = false;
            }
            
            // Check if the attack anim finished
            var state = animator.GetCurrentAnimatorStateInfo(0);
            if (state.shortNameHash == AttackState && state.normalizedTime >= 1)
            {
                animator.SetBool(IsAttacking, false);
                OnDoneAttacking();
            }

        }

        public override bool CanAttack()
        {
            return canAttack;
        }

        public override void Attack()
        {
            animator.SetBool(IsAttacking, true);
            onAttack.Invoke();
            nextAttackTime = Time.time + attackDelay;
        }
        
        private void OnPlayerFound(Player.Player player)
        {
            target = player.gameObject;
            animator.SetBool(IsCharging, true);
        }

        private void OnPlayerLost(Player.Player player)
        {
            target = null;
            animator.SetBool(IsCharging, false);
        }
    }
}