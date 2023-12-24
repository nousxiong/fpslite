using UnityEditor;
using UnityEngine;

namespace Common.Editor
{
    [CustomEditor(typeof(CameraNode.CameraNode))]
    public class CameraNodeEditor : UnityEditor.Editor
    {
        private CameraNode.CameraNode currentNode;

        private void OnSceneGUI()
        {
            Event e = Event.current;
            var cameraNode = (CameraNode.CameraNode)target;
            Debug.Log($"CameraNode {cameraNode.transform.position} {e.type} OnSceneGUI");
            // 处理鼠标点击事件
            if (e.type == EventType.MouseUp && e.button == 0)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                Debug.Log($"MouseUp {ray.origin}");

                if (Physics.Raycast(ray, out var hit))
                {
                    CameraNode.CameraNode hitNode = hit.collider.GetComponent<CameraNode.CameraNode>();
                    if (hitNode != null)
                    {
                        if (currentNode == null)
                        {
                            // 第一次点击
                            currentNode = hitNode;
                            Handles.DrawLine(currentNode.transform.position, ray.origin);
                        }
                        else if (hitNode != currentNode)
                        {
                            // 第二次点击，绘制线并重置
                            Debug.DrawLine(currentNode.transform.position, 
                                hitNode.transform.position, Color.red);
                            currentNode = null;
                        }
                    }
                    else
                    {
                        currentNode = null;
                    }
                }
                else
                {
                    currentNode = null;
                }
                HandleUtility.Repaint();
            }
        }
    }
}