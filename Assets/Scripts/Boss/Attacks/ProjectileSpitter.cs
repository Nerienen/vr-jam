using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRJammies.Framework.Core.Health;

namespace VRJammies.Framework.Core.Boss
{
    public class ProjectileSpitter : MonoBehaviour
    {
        [SerializeField]
        private GameObject _target;
        [SerializeField]
        private GameObject _projectilePrefab;
        [SerializeField]
        private List<GameObject> _projectileList;
        [SerializeField]
        private Transform _output;
        [SerializeField]
        private float _force = 10;
        [SerializeField]
        private bool _isActive = false;


        [SerializeField]
        private float _attackSpeed = 0.125f;
        private float _timer = 0f;


        private void Start()
        {
            if (!_projectilePrefab) Debug.LogWarning(this.name + " has no projectile prefab assigned!");
            if(!_output) Debug.LogWarning(this.name + " has no projectile output assigned!");

        }

        private void Update()
        {
            if (_isActive && _target)
            {
                transform.LookAt(_target.transform);

                _timer += Time.deltaTime;
                if (_timer >= _attackSpeed)
                {
                    SpawnProjectile();
                    _timer = 0;
                }
            }
            else _timer = 0;
        }

        private void LookAtTarget(GameObject target) 
        {
            LookAtTarget(target);
        }

        private void SpawnProjectile()
        {
            bool didSpawn = false;

            // Check the projectile list for inactive projectiles
            foreach (var projectile in _projectileList)
            {
                if (!projectile.activeSelf)
                {
                    // If you found an inactive object, use that and tick of didSpawn as true
                    projectile.transform.position = this.transform.position;
                    projectile.SetActive(true);
                    ShootProjectile(projectile);
                    didSpawn = true;
                    break;
                }
            }

            // If after the loop there was no inactive object, spawn a new one
            if (!didSpawn)
            {
                var projectile = Instantiate(_projectilePrefab, _output.position, _output.rotation);
                _projectileList.Add(projectile);
                ShootProjectile(projectile);
            }

            /*
            if (_projectileList.Count < _maxProjectiles)
            {
                var projectile = Instantiate(_projectilePrefab, _output.position, _output.rotation);
                _projectileList.Add(projectile);
                ShootProjectile(projectile);
            }
            else
            {
                foreach (var projectile in _projectileList)
                {
                    if (!projectile.activeSelf)
                    {
                        ShootProjectile(projectile);
                        break;
                    }
                }
            }
            */
        }

        private void ShootProjectile(GameObject projectile)
        {
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            rb.velocity = Vector3.zero;

            projectile.transform.position = _output.position;
            projectile.transform.rotation = _output.rotation;
            projectile.GetComponent<DamageColliderProjectile>().SetSpawner(this.gameObject);
            projectile.SetActive(true);
            rb.velocity = transform.forward * _force;
            projectile.transform.parent = null;            
        }
    }
}
