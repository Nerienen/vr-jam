using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VRJammies.Framework.Core.Boss
{
    public class PatrolPoint : MonoBehaviour
    {
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (
                Selection.activeGameObject == null ||
                Selection.activeGameObject.GetComponentInChildren<PatrolPoint>() == null
            ) return;
            
            Handles.color = Color.blue;
            Handles.DrawSolidDisc(transform.position, transform.up, 0.3f);
        }
#endif
    }
}