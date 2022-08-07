using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BossfightTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject objectToEnable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.GetComponentInChildren<PlayerRespawn>())
            objectToEnable.SetActive(true);
    }
}
