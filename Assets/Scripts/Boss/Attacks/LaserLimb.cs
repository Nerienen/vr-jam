using System.Collections;
using UnityEngine;
using VRJammies.Framework.Core.Boss;

public class LaserLimb : AttackBase
{
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] LineRenderer indicatorRenderer;
    [SerializeField] ParticleSystem prepShot;
    [SerializeField] ParticleSystem bullet;

    public Transform target;
    public Transform indicatorPoint;
    public Transform firePoint;

    private bool indicating = false;
    private bool hasShot = false;

    private RaycastHit hitInfo;


    void Start()
    {
        
    }

    void FixedUpdate()
    {
        LookAtPlayer();

        if (indicating)
        {
            
            indicatorRenderer.enabled = true;
            ShowIndicator();
        }
        else
            indicatorRenderer.enabled = false;  

    }

    private void LookAtPlayer()
    {
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
}
