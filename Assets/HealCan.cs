using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRJammies.Framework.Core.Health;

public class HealCan : MonoBehaviour
{
    Damageable damageable;

    private void Start()
    {
        damageable = GameObject.FindGameObjectWithTag("Player").GetComponent<Damageable>();
    }

    public void OnActivate()
    {
        damageable.DealDamage(-1,DamageForm.PlayerDamage);
    }
}
