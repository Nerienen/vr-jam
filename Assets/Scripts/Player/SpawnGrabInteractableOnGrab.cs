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
    [SerializeField]
    private List<GameObject> _projectileList = new List<GameObject>();

    private void Start()
    {
        var go = SpawnProjectile();
        go.SetActive(false);

        InteractionManager = FindObjectOfType<XRInteractionManager>();
        if (!InteractionManager) Debug.LogWarning(this + " didn't find an Interaction Manager!");
        if (!GrabInteractablePrefab) Debug.LogWarning(this + " has no prefab to spawn assigned!");
    }

    public void OnGrab(SelectEnterEventArgs args)
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
                AttachProjectile(projectile, args);
                didSpawn = true;
                break;
            }
        }

        // If after the loop there was no inactive object, spawn a new one
        if (!didSpawn)
            AttachProjectile(SpawnProjectile(), args);
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
