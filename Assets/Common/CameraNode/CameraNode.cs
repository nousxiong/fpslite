using System;
using UnityEngine;

namespace Common.CameraNode
{
    public class CameraNode : MonoBehaviour
    {
        public CameraPath Path;
        public Color gizmoColor = Color.grey;
#if UNITY_EDITOR
        public static CameraNode SelectedCameraNode;
#endif
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