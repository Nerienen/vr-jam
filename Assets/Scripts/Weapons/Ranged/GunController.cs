using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VRJammies.Framework.Core.Health;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private float _force = 15f;
    [SerializeField]
    private LiquidTank _liquidTankScript;

    public GunController(LiquidTank liquidTankScript)
    {
        _liquidTankScript = liquidTankScript;
    }

    [SerializeField]
    private GameObject _projectilePrefab;
    [SerializeField]
    private List<GameObject> _projectileList = new List<GameObject>();
    [SerializeField]
    private Transform _outputTransform;

    [SerializeField]
    private SelectEnterEventArgs selectEnterArgs;
    [SerializeField]
    private SelectExitEventArgs selectExitArgs;

    public void OnAttach(SelectEnterEventArgs args)
    {
        selectEnterArgs = args;

        if (args.interactableObject.transform.gameObject.TryGetComponent<DestroyOnStandStill>(out DestroyOnStandStill destroyScript))
            destroyScript.enabled = false;

        _liquidTankScript = args.interactableObject.transform.gameObject.GetComponentInParent<LiquidTank>();
        _projectilePrefab = _liquidTankScript.GetProjectilePrefab();

        foreach (var projectile in _projectileList)
        {
            if(projectile)
                Destroy(projectile);
        }
        _projectileList = new List<GameObject>();
    }

    public void OnDetach(SelectExitEventArgs args)
    {
        selectExitArgs = args;
        _liquidTankScript = null; 
        if (args.interactableObject.transform.gameObject.TryGetComponent<DestroyOnStandStill>(out DestroyOnStandStill destroyScript))
            destroyScript.enabled = true;

        foreach (var projectile in _projectileList)
        {
            Destroy(projectile);
        }
        _projectileList = new List<GameObject>();
    }

    public void OnActivate()
    {
        if (_liquidTankScript && _liquidTankScript.CheckCurrentLiquid() > 0 && _projectilePrefab)
        {
            bool didSpawn = false;

            // Check the projectile list for inactive projectiles
            foreach (var projectile in _projectileList)
            {
                if (projectile && !projectile.activeSelf)
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
                ShootProjectile(SpawnProjectile());

            _liquidTankScript.SubstractLiquid(1);
        }
        else
            EjectTank();
    }

    private void EjectTank()
    {
        // Method to eject doesn't work. Can let this method play an empty sound. 

        /*
        InteractionManager.SelectExit(selectEnterArgs.interactorObject, selectEnterArgs.interactableObject);

        var go = selectEnterArgs.interactableObject.transform.root.gameObject;
        go.GetComponentInChildren<Collider>().enabled = false;

        selectEnterArgs.interactableObject.transform.root.gameObject.GetComponentInChildren<Rigidbody>().velocity = new Vector3(1, 2, 0);

        Destroy(go, 5);
        */
    }

    private GameObject SpawnProjectile()
    {
        var projectile = Instantiate(_projectilePrefab, this.transform.position, this.transform.rotation);
        _projectileList.Add(projectile);
        return projectile;
    }

    private void ShootProjectile(GameObject projectile)
    {
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        rb.velocity = Vector3.zero;

        projectile.transform.position = _outputTransform.position;
        projectile.transform.rotation = _outputTransform.rotation;
        projectile.GetComponent<DamageColliderProjectile>().SetSpawner(this.gameObject);
        projectile.SetActive(true);
        rb.velocity = transform.forward * _force;
        projectile.transform.parent = null;

    }
}
