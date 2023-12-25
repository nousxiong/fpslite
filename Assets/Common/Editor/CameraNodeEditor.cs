using UnityEditor;
using UnityEngine;

namespace Common.Editor
{
    [CustomEditor(typeof(CameraNode.CameraNode))]
    public class CameraNodeEditor : UnityEditor.Editor
    {
        private CameraNode.CameraNode startNode;

        private void OnSceneGUI()
        {
            Event e = Event.current;
            var cameraNode = (CameraNode.CameraNode)target;
            Debug.Log($"CameraNode {cameraNode.transform.position} {e.type} OnSceneGUI");
            // if (e.type == EventType.MouseMove)
            {
                if (startNode == null)
                {
                    startNode = cameraNode;
                }
                
                if (startNode == cameraNode)
                {
                    Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                    // Debug.Log($"mouse ray {ray.origin}");
                    Handles.color = Color.yellow;
                    Handles.DrawLine(startNode.transform.position, ray.origin);
                    // HandleUtility.Repaint();
                }
                else if (startNode != null)
                {
                    Handles.DrawLine(startNode.transform.position, cameraNode.transform.position);
                }
            }
            HandleUtility.Repaint();
        }
    }
}