using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceHP : MonoBehaviour
{

    public float targetScale;
    public float timeToLerp = 0.25f;
    float scaleModifier = 1;
    public bool isMelting = false;
    



    public void FixedUpdate()
    {
        if (isMelting == true)
        {
            StartCoroutine(LerpFunction(targetScale, timeToLerp));
        }
        if (isMelting == false)
        {
            StopAllCoroutines();
        }

    }

    public IEnumerator LerpFunction(float endValue, float duration)
    {
        float time = 0;
        float startValue = scaleModifier;
        Vector3 startScale = transform.localScale;
        while (time < duration)
        {
            scaleModifier = Mathf.Lerp(startValue, endValue, time / duration);
            transform.localScale = startScale * scaleModifier;
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = startScale * endValue;
        scaleModifier = endValue;
    }

}





