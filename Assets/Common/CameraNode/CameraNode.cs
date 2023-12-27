using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Common.CameraNode
{
    public class CameraNode : MonoBehaviour
    {
        List<CameraPath> Paths = new List<CameraPath>();
        public Color gizmoColor = Color.grey;
#if UNITY_EDITOR
        public static CameraNode SelectedCameraNode;
#endif
        public bool Connect(CameraNode other, out GameObject path, GameObject pathParent = null)
        {
            path = null;
            if (IsConnected(other))
            {
                return false;
            }
            path = new GameObject("P_CameraPath");
            var cameraPath = path.GetComponent<CameraPath>();
            cameraPath.Begin = this;
            cameraPath.End = other;
            if (pathParent != null)
            {
                path.transform.SetParent(pathParent.transform);
            }
            cameraPath.UpdateTransform();
            Paths.Add(cameraPath);
            return true;
        }
        
        bool IsConnected(CameraNode other)
        {
            return other == this || Paths.Find(path => path.IsEndpoint(other)) != null;
        }
        
        void OnDrawGizmos()
        {
            DrawBox();
        }

        void DrawBox()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            // 保存原始Gizmos矩阵
            Matrix4x4 originalMatrix = Gizmos.matrix;
            Gizmos.color = gizmoColor;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
            Gizmos.matrix = originalMatrix;
        }
    }
}