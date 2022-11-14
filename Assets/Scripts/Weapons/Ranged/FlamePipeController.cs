using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VRJammies.Framework.Core.Health;

public class FlamePipeController : MonoBehaviour
{
    [SerializeField]
    private float _force = 15f;
    [SerializeField]
    private LiquidTank _liquidTankScript;

    [SerializeField]
    private GameObject _projectilePrefab;
    [SerializeField]
    private Transform _outputTransform;

    DamageForm _damageForm = DamageForm.Fire;


    [SerializeField]
    private bool _activate = false;

    private void Update()
    {
        if (_activate) 
        {
            OnActivate();
            _activate = false;
        }
    }

    public void OnAttach(SelectEnterEventArgs obj)
    {
        // Reference the Socket and the grabbable that was attached
        var _craftingSocket = obj.interactorObject;
        GameObject attachedObject = obj.interactableObject.transform.gameObject;

        if (attachedObject.transform.gameObject.TryGetComponent<DestroyOnStandStill>(out DestroyOnStandStill destroyScript))
            destroyScript.enabled = false;

        _liquidTankScript = attachedObject.transform.gameObject.GetComponentInParent<LiquidTank>();
        _projectilePrefab = _liquidTankScript.GetProjectilePrefab();
        _projectilePrefab.GetComponent<DamageColliderProjectile>().SetDamageType(_damageForm);
        _projectilePrefab.GetComponent<DamageColliderProjectile>().SetLayerToIgnore("Grabbables");

        // Check if the referencing was succesfull 
        if (attachedObject != null)
        {
            // Deactivate the crafting socket
            _craftingSocket.transform.gameObject.SetActive(false);

            // Attach the socketed object to this gameobject and get rid of the individual parts like the rigidbody or the XRGrabInteractable of the attached object.
            Destroy(attachedObject.GetComponent<XRGrabInteractable>());
            Destroy(attachedObject.GetComponent<Rigidbody>());
            attachedObject.transform.parent = this.transform;
            attachedObject.transform.position = _craftingSocket.transform.position;
            attachedObject.transform.rotation = _craftingSocket.transform.rotation;
        }
        else Debug.LogWarning(name + ": null reference while trying to attach an object!");
    }

    public void OnActivate()
    {
        if (_liquidTankScript && _liquidTankScript.CheckCurrentLiquid() > 0 && _projectilePrefab)
        {
            ShootProjectile(SpawnProjectile());

           // Explode();
        }
    }

    private GameObject SpawnProjectile()
    {
        var projectile = Instantiate(_projectilePrefab, this.transform.position, this.transform.rotation);
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
        rb.velocity = projectile.transform.forward * _force;
        projectile.transform.parent = null;

    }

    private void Explode() 
    {
        Destroy(this.gameObject);
    }
}
