using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SpawnGrabInteractableOnGrab : MonoBehaviour
{
    [SerializeField]
    private GameObject GrabInteractablePrefab;
    [SerializeField]
    private XRInteractionManager InteractionManager; 

    private void Start()
    {
        InteractionManager = FindObjectOfType<XRInteractionManager>();
        if (!InteractionManager) Debug.LogWarning(this + " didn't find an Interaction Manager!");
        if (!GrabInteractablePrefab) Debug.LogWarning(this + " has no prefab to spawn assigned!");
    }

    public void OnGrab(SelectEnterEventArgs args)
    {
        var Interactable = Instantiate<GameObject>(GrabInteractablePrefab);
        Interactable.transform.position = this.transform.position;
        var GrabInteractableScript = Interactable.transform.root.GetComponentInChildren<XRGrabInteractable>();
        if (!GrabInteractableScript) Debug.LogWarning(this + " prefab has no XRGrabInteractable script assigned!");
        
        InteractionManager.SelectEnter(args.interactorObject, GrabInteractableScript);
    }
}
