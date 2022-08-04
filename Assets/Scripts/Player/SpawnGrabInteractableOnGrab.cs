using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SpawnGrabInteractableOnGrab : MonoBehaviour
{
    // Grab Interactable to spawn
    [SerializeField]
    private GameObject GrabInteractablePrefab;
    // Reference to the interaction manager, needed to pass a interactable to an interactor (hand)
    [SerializeField]
    private XRInteractionManager InteractionManager;

    // List with references to spawned projectiles, used to only spawn another projectile if needed
    private List<GameObject> _projectileList = new List<GameObject>();

    private void Start()
    {
        InteractionManager = FindObjectOfType<XRInteractionManager>();
        if (!InteractionManager) Debug.LogWarning(this + " didn't find an Interaction Manager!");
        if (!GrabInteractablePrefab) Debug.LogWarning(this + " has no prefab to spawn assigned!");
    }

    public void OnGrab(SelectEnterEventArgs args)
    {
        // If there is no projectile in the list, spawn one
        if (_projectileList.Count == 0)
        {
            AttachProjectile(SpawnProjectile(), args);
        }
        else
        {
            // Check the projectile list for inactive projectiles
            foreach (var projectile in _projectileList)
            {
                if (!projectile.activeSelf)
                {
                    AttachProjectile(projectile, args);
                    break;
                }
                else
                    AttachProjectile(SpawnProjectile(), args);
            }
        }
    }

    private GameObject SpawnProjectile()
    {
        var projectile = Instantiate(GrabInteractablePrefab, this.transform.position, this.transform.rotation);
        _projectileList.Add(projectile);
        return projectile;
    }

    private void AttachProjectile(GameObject interactable, SelectEnterEventArgs args)
    {
        var GrabInteractableScript = interactable.transform.root.GetComponentInChildren<XRGrabInteractable>();
        if (!GrabInteractableScript) Debug.LogWarning(this + " prefab has no XRGrabInteractable script assigned!");
        else InteractionManager.SelectEnter(args.interactorObject, GrabInteractableScript); 
    }
}
