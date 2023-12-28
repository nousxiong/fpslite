using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Common.CameraNode
{
    public class CameraNode : MonoBehaviour
    {
        HashSet<CameraPath> Paths = new HashSet<CameraPath>();
        public Color gizmoColor = Color.grey;
#if UNITY_EDITOR
        public GameObject pathPrefab;
#endif
        public bool Connect(CameraNode other, out GameObject path, GameObject pathParent = null)
        {
            path = null;
            if (IsConnected(other))
            {
                return false;
            }
            // path = new GameObject("P_CameraPath");
            path = Object.Instantiate(pathPrefab);
            var cameraPath = path.GetComponent<CameraPath>();
            cameraPath.Begin = this;
            cameraPath.End = other;
            if (pathParent != null)
            {
                path.transform.SetParent(pathParent.transform);
            }
            cameraPath.UpdateTransform();
            return Paths.Add(cameraPath);
        }
        
        bool IsConnected(CameraNode other)
        {
            return other == this || Paths.FirstOrDefault(path => path.IsEndpoint(other)) != null;
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