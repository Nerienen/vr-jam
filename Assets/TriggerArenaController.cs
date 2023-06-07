using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArenaController : MonoBehaviour
{
    public Animator targetAnimator; // Reference to the Animator component on the target object
    public string parameterName = "playerEnteredArena"; // Name of the parameter to set

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetAnimator.SetBool(parameterName, true); // Set the parameter to true
        }
    }
}
