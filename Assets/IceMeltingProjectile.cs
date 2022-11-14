using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMeltingProjectile : MonoBehaviour
{

    IceHP iceHp;

    [SerializeField]
    GameObject icelet;

    private void Awake()
    {
        iceHp = icelet.GetComponent<IceHP>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ice"))
        {
            Debug.Log("Ice hit by Fireball");
            iceHp.isMelting = true;
           
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ice"))
        {
            Debug.Log("Fireball left Ice");
                iceHp.isMelting = false;
        }
    }
}
