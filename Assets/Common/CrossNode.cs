using UnityEngine;

namespace Common
{
    public class CrossNode : MonoBehaviour
    {
        public Color gizmoColor = Color.magenta;

        void OnDrawGizmos()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            
            Matrix4x4 originalMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = gizmoColor;
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
            Gizmos.matrix = originalMatrix;
        }
    }
}
