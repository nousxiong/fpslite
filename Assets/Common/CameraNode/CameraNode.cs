using UnityEngine;

namespace Common.CameraNode
{
    public class CameraNode : MonoBehaviour
    {
        /// <summary>
        /// 路径宽度
        /// </summary>
        public float pathWidth = 1f;
#if UNITY_EDITOR
        public Color gizmoColor = Color.magenta;
        Vector3 pathCenter = new Vector3(0f, 0f, 0f);
        Vector3 pathSize = new Vector3(0f, 0f, 0f);
#endif
        
        void OnDrawGizmos()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            Quaternion rotation = transform.rotation;
            UpdateDirectionToParent();
            DrawPath();
            transform.rotation = rotation;
            DrawBox();
        }

        void UpdateDirectionToParent()
        {
            Transform parent = transform.parent;
            if (parent == null)
            {
                return;
            }
            var parentNode = parent.GetComponent<CameraNode>();
            if (parentNode == null)
            {
                return;
            }
            Vector3 positionEnd = parent.position;
            Vector3 positionBegin = transform.position;
            Vector3 direction = positionEnd - positionBegin;
            
            transform.rotation = Quaternion.LookRotation(direction.normalized);
        }

        void DrawPath()
        {
            Transform parent = transform.parent;
            if (parent == null)
            {
                return;
            }
            var parentNode = parent.GetComponent<CameraNode>();
            if (parentNode == null)
            {
                return;
            }
            
            Vector3 position = transform.position;
            Vector3 parentPosition = parent.position;
            
            // draw link
            Gizmos.color = gizmoColor;
            Gizmos.DrawLine(position, parentPosition);
            
            // draw path
            var distance = Vector3.Distance(position, parentPosition);
            Matrix4x4 originalMatrix = Gizmos.matrix;
            Gizmos.color = gizmoColor;
            Gizmos.matrix = transform.localToWorldMatrix;
            pathCenter.z = distance / 2f;
            pathSize.x = pathWidth;
            pathSize.z = distance;
            Gizmos.DrawWireCube(pathCenter, pathSize);
            Gizmos.matrix = originalMatrix;
        }

        void DrawBox()
        {
            Matrix4x4 originalMatrix = Gizmos.matrix;
            Gizmos.color = gizmoColor;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
            Gizmos.matrix = originalMatrix;
        }
    }
}