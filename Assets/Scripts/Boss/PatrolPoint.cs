using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Boss
{
    public class PatrolPoint : MonoBehaviour
    {
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Selection.count == 0 || Selection.activeGameObject.GetComponentInChildren<PatrolPoint>() == null) return;
            
            Handles.color = Color.blue;
            Handles.DrawSolidDisc(transform.position, transform.up, 0.3f);
        }
#endif
    }
}