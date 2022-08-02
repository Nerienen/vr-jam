using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SpawnPrefabOnTouch : MonoBehaviour
{
    public GameObject PrefabToSpawn;
    public Transform SpawnTransform;

    private bool _canSpawn = true;

    private void OnTriggerEnter(Collider other)
    {
        if (PrefabToSpawn && SpawnTransform && _canSpawn)
        {
            var objectToSpawn = Instantiate<GameObject>(PrefabToSpawn);
            objectToSpawn.transform.position = SpawnTransform.position;
            objectToSpawn.transform.rotation = SpawnTransform.rotation;

            _canSpawn = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _canSpawn = true;
    }
}
