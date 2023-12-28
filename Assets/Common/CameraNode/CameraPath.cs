using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Common.CameraNode
{
    public class CameraPath : MonoBehaviour
    {
        [NonSerialized]
        public CameraNode Begin;
        [NonSerialized]
        public CameraNode End;
        public float width = 1f;
        public Color gizmoColor = Color.magenta;
        Vector3 size = new Vector3(0f, 0f, 0f);

        public bool IsEndpoint(CameraNode node)
        {
            if (node == null) return false;
            return Begin == node || End == node;
        }

        public void UpdateTransform()
        {
            Vector3 positionEnd = End.transform.position;
            Vector3 positionBegin = Begin.transform.position;
            Vector3 dir = positionEnd - positionBegin;
            
            Transform transformPath = transform;
            transformPath.rotation = Quaternion.LookRotation(dir.normalized);
            transformPath.position = (positionEnd + positionBegin) / 2;
        }

        void OnDrawGizmos()
        {
            if (Begin == null || End == null)
            {
                return;
            }

            UpdateTransform();
            
            Gizmos.color = gizmoColor;
            Gizmos.DrawLine(Begin.transform.position, End.transform.position);
            size.x = width;
            size.z = Vector3.Distance(Begin.transform.position, End.transform.position);
            // 保存原始Gizmos矩阵
            Matrix4x4 originalMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, size);
            Gizmos.matrix = originalMatrix;
            // Gizmos.DrawWireCube(transform.position, size);
        }
    }
}