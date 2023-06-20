using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HueLerperCbp : MonoBehaviour
{
 

    public float lerpSpeed = 1f; // Speed of the hue lerping
    private Material material; // Reference to the GameObject's material

    private float currentHue; // Current hue value
    private float targetHue; // Target hue value

    private float minHue = 175f; // Minimum hue value (cyan)
    private float maxHue = 360f; // Maximum hue value (red)

    private void Start()
    {
        // Get the material component attached to the GameObject
        Renderer renderer = GetComponent<Renderer>();
        material = renderer.material;

        // Get the initial hue value from the material's emission color
        Color emissionColor = material.GetColor("_EmissionColor");
        Color.RGBToHSV(emissionColor, out currentHue, out _, out _);
    }

    private void Update()
    {
        // Calculate the next hue value based on the lerp speed
        currentHue = Mathf.Lerp(currentHue, targetHue, lerpSpeed * Time.deltaTime);

        // Wrap the hue value within the desired range (175 to 360)
        currentHue = WrapHue(currentHue, minHue, maxHue);

        // Update the emission color with the new hue value
        Color newEmissionColor = Color.HSVToRGB(currentHue, 1f, 1f);
        material.SetColor("_EmissionColor", newEmissionColor);

        // Ensure the emission property is active
        material.EnableKeyword("_EMISSION");

        // If the current hue is close to the target hue, generate a new random target hue
        if (Mathf.Abs(currentHue - targetHue) < 0.01f)
        {
            targetHue = Random.Range(minHue, maxHue);
        }
    }

    // Wrap the hue value within a specified range
    private float WrapHue(float hue, float minHue, float maxHue)
    {
        if (hue < minHue)
        {
            hue += maxHue - minHue;
        }
        else if (hue > maxHue)
        {
            hue -= maxHue - minHue;
        }

        return hue;
    }


}
