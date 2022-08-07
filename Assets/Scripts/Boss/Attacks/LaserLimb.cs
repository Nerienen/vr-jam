using System.Collections;
using UnityEngine;
using VRJammies.Framework.Core.Boss;

public class LaserLimb : AttackBase
{
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] LineRenderer indicatorRenderer;
    [SerializeField] ParticleSystem prepShot;
    [SerializeField] ParticleSystem bullet;

    [SerializeField] private PlayerFinder playerFinder;

    public Transform target;
    public Transform indicatorPoint;
    public Transform firePoint;

    private bool indicating = false;
    private bool hasShot = false;
    private bool foundPlayer = false;

    private RaycastHit hitInfo;


    void Start()
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
    }

    void FixedUpdate()
    {
        if (foundPlayer)
        {
            LookAtPlayer();
        }

        if (indicating && target)
        {
            
            indicatorRenderer.enabled = true;
            ShowIndicator();
        }
        else
            indicatorRenderer.enabled = false;  

    }

    private void LookAtPlayer()
    {
        if (target == null) return;
        
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    private void ShowIndicator()
    {
        if(Physics.Linecast(indicatorPoint.position, target.position, out hitInfo))
        {
            indicatorRenderer.SetPosition(0, indicatorPoint.position);
            indicatorRenderer.SetPosition(1, hitInfo.transform.position);
            if (!hasShot)
            {
                StartCoroutine(PrepShot());
                hasShot = true;
            }
        }
        else
        {
            indicatorRenderer.SetPosition(0, indicatorPoint.position);
            indicatorRenderer.SetPosition(1, target.position);
        }
    }
    public IEnumerator PrepShot()
    {
        prepShot.Play();
        yield return new WaitForSeconds(5f);
        prepShot.Stop();
        bullet.Play();
        yield return new WaitForSeconds(0.3f);
        bullet.Stop();

        indicating = false;
        hasShot = false;
        OnDoneAttacking();
    }

    public override bool CanAttack()
    {
        return !hasShot;
    }

    public override void Attack()
    {
        indicating = true;
    }

    private void OnPlayerFound(Player.Player player)
    {        
        target = player.transform;
        foundPlayer = true;
    }

    private void OnPlayerLost(Player.Player player)
    {
        target = null;
        foundPlayer = false;
    }
}
