using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumer : MonoBehaviour
{
    public Collider colliderMouth;
    public ParticleSystem CokeMonsterVFX1;
    public ParticleSystem CokeMonsterVFX2;
    public ParticleSystem CokeMonsterVFX3;
    public ParticleSystem CokeMonsterVFX4;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CokeMonsterBottle"))
        {
            Debug.Log("Entered Mouth");
            other.gameObject.GetComponent<ParticleSystem>().Play();
            CokeMonsterVFX1.Play();
            CokeMonsterVFX2.Play();
            CokeMonsterVFX3.Play();
            CokeMonsterVFX4.Play();
            
        }
    }

  
}
