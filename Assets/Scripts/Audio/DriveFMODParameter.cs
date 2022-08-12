using FMODUnity;
using UnityEngine;

namespace VRJammies.Framework.Core.Audio
{
    public class DriveFMODParameter : MonoBehaviour
    {
        [SerializeField] private string paramName;
        [SerializeField] private StudioEventEmitter emitter;
        [SerializeField] private float inputMin;
        [SerializeField] private float inputMax;
        [SerializeField] private float outputMin;
        [SerializeField] private float outputMax;

        public void SetParam(float value)
        {
            var mappedValue = Mathf.Lerp(
                outputMin,
                outputMax,
                Mathf.InverseLerp(inputMin, inputMax, value)
            );
            emitter.SetParameter(paramName, mappedValue);
            // Debug.Log($"Set parameter to: {mappedValue}");
        }
    }
}