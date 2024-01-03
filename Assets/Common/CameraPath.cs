using UnityEngine;

namespace Common
{
    public class CameraPath : MonoBehaviour
    {
#if UNITY_EDITOR
        public Vector3 size;
        public Color gizmoColor = Color.magenta;
        public Color parentColor = Color.grey;
        Vector3 pathHead = Vector3.zero;
        Vector3 pathTail = Vector3.zero;
        Vector3 pathXzSize = Vector3.zero;
        Vector3 pathArrowEdge = Vector3.zero; 
        Vector3 pathArrowLeft = Vector3.zero; 
        Vector3 pathArrowRight = Vector3.zero;
#endif
        BoxCollider pathCollider;
        
        void OnDrawGizmos()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            
            Transform parent = transform.parent;
            var hasParent = parent != null && parent.GetComponent<CameraPath>() != null;
            if (hasParent)
            {
                // Draw line to parent
                Gizmos.color = parentColor;
                Gizmos.DrawLine(transform.position, parent.position);
            }
            
            // 保存原始Gizmos矩阵
            Gizmos.color = gizmoColor;
            Matrix4x4 originalMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;
            
            // Draw box
            Gizmos.DrawWireCube(Vector3.zero, size);
            pathXzSize.x = size.x;
            pathXzSize.z = size.z;
            Gizmos.DrawWireCube(Vector3.zero, pathXzSize);
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
            
            // Draw Arrow
            pathArrowEdge.z = 1f;
            pathArrowLeft.x = -0.5f;
            pathArrowLeft.z = 0.5f;
            pathArrowRight.x = 0.5f;
            pathArrowRight.z = 0.5f;
            Gizmos.DrawLine(pathArrowLeft, pathArrowEdge);
            Gizmos.DrawLine(pathArrowRight, pathArrowEdge);
            
            // Draw line
            if (!hasParent)
            {
                pathHead.z = size.z / 2;
                pathTail.z = -size.z / 2;
                Gizmos.DrawLine(pathTail, pathHead);
            }
            
            // 恢复原始Gizmos矩阵
            Gizmos.matrix = originalMatrix;
            
            SyncCollider();
        }

        void Awake()
        {
            SyncCollider();
        }

        void SyncCollider()
        {
            if (pathCollider == null)
            {
                pathCollider = GetComponent<BoxCollider>();
            }

            if (pathCollider == null)
            {
                return;
            }

            pathCollider.size = size;
        }
    }
}
