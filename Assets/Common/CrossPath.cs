using UnityEngine;

namespace Common
{
    public class CrossPath : MonoBehaviour
    {
        public Vector3 size;
        public Color gizmoColor = Color.magenta;
        public Color parentColor = Color.grey;
        Vector3 pathHead = Vector3.zero;
        Vector3 pathTail = Vector3.zero;
        Vector3 pathXzSize = Vector3.zero;
        Vector3 pathArrowEdge = Vector3.zero; 
        Vector3 pathArrowLeft = Vector3.zero; 
        Vector3 pathArrowRight = Vector3.zero;
        
        BoxCollider pathCollider;

        void DrawBox()
        {
            Matrix4x4 originalMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireCube(Vector3.zero, size);
            pathXzSize.x = size.x;
            pathXzSize.z = size.z;
            Gizmos.DrawWireCube(Vector3.zero, pathXzSize);
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
            Gizmos.matrix = originalMatrix;
        }
        
        void OnDrawGizmos()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            
            SyncCollider();
            
            // Draw box
            DrawBox();
            
            Transform selfTransform = transform;
            Transform parent = selfTransform.parent;
            if (parent == null)
            {
                return;
            }

            UpdateDirectionToParent();
            
            Gizmos.color = gizmoColor;
            if (parent.TryGetComponent(out CrossPath parentPath))
            {
                // Draw line to parent
                Color parentLineColor = parentColor;
                Color color = parentPath.gizmoColor;
                Transform grandparent = parent.parent;
                if (grandparent != null && grandparent.TryGetComponent(out CrossNode grandparentNode))
                {
                    parentLineColor = grandparentNode.gizmoColor;
                    color = grandparentNode.gizmoColor;
                }
                Gizmos.color = parentLineColor;
                Gizmos.DrawLine(selfTransform.position, parent.position);
                Gizmos.color = color;
            }
            if (parent.TryGetComponent(out CrossNode parentNode))
            {
                Gizmos.color = parentNode.gizmoColor;
            }

            // 保存原始Gizmos矩阵
            Matrix4x4 originalMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;
            
            // Draw Arrow
            pathArrowEdge.z = 1f;
            pathArrowLeft.x = -0.5f;
            pathArrowLeft.z = 0.5f;
            pathArrowRight.x = 0.5f;
            pathArrowRight.z = 0.5f;
            Gizmos.DrawLine(pathArrowLeft, pathArrowEdge);
            Gizmos.DrawLine(pathArrowRight, pathArrowEdge);
            
            // Draw line
            pathHead.z = size.z / 2;
            pathTail.z = -size.z / 2;
            Gizmos.DrawLine(pathTail, pathHead);
            
            // 恢复原始Gizmos矩阵
            Gizmos.matrix = originalMatrix;
        }
        
        void UpdateDirectionToParent()
        {
            Transform parent = transform.parent;
            if (parent == null)
            {
                return;
            }
            if (!parent.TryGetComponent(out CrossNode _))
            {
                return;
            }
            Vector3 positionEnd = parent.position;
            Vector3 positionBegin = transform.position;
            Vector3 direction = positionEnd - positionBegin;
            
            transform.rotation = Quaternion.LookRotation(-direction.normalized);
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

        void LateUpdate()
        {
            UpdateDirectionToParent();
        }
    }
}
