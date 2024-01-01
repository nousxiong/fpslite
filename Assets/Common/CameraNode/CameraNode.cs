using UnityEngine;

namespace Common.CameraNode
{
    public class CameraNode : MonoBehaviour
    {
        /// <summary>
        /// 路径宽度
        /// </summary>
        public float pathWidth = 1f;
        [Header("Tail size, default no tail")]
        public float tailLength;
#if UNITY_EDITOR
        public Color gizmoColor = Color.magenta;
        public Color selectedColor = Color.yellow;
        Vector3 pathCenter = new Vector3(0f, 0f, 0f);
        Vector3 pathEdge = new Vector3(0f, 0f, 0f);
        Vector3 pathEnd = new Vector3(0f, 0f, 0f);
        Vector3 pathSize = new Vector3(0f, 0f, 0f);
        Vector3 arrowLeft = new Vector3(0f, 0f, 0f);
        Vector3 arrowEdge = new Vector3(0f, 0f, 0f);
        Vector3 arrowRight = new Vector3(0f, 0f, 0f);
        public bool selected;
#endif
        Color GetColor()
        {
            return selected ? selectedColor : gizmoColor;
        }
        
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
            var distance = Vector3.Distance(position, parentPosition);
            var distanceWithTail = tailLength + distance;
            Matrix4x4 originalMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;
            
            // draw link
            Gizmos.color = GetColor();
            pathEdge.z = distance;
            pathEnd.z = -tailLength;
            Gizmos.DrawLine(pathEnd, pathEdge);
            
            // draw path & arrow
            Gizmos.color = GetColor();
            pathCenter.z = distanceWithTail / 2f - tailLength;
            pathSize.x = pathWidth;
            pathSize.z = distanceWithTail;
            Gizmos.DrawWireCube(pathCenter, pathSize);
            arrowLeft.x = -pathWidth / 2f;
            arrowLeft.z = distanceWithTail / 3f - tailLength;
            arrowRight.x = pathWidth / 2;
            arrowRight.z = distanceWithTail / 3f - tailLength;
            arrowEdge.z = distanceWithTail / 3f * 2f - tailLength;
            Gizmos.DrawLine(arrowLeft, arrowEdge);
            Gizmos.DrawLine(arrowRight, arrowEdge);
            
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