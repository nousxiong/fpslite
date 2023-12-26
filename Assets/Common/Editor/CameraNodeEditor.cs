using UnityEditor;
using UnityEngine;

namespace Common.Editor
{
    [CustomEditor(typeof(CameraNode.CameraNode))]
    public class CameraNodeEditor : UnityEditor.Editor
    {
        static CameraNode.CameraNode startNode;

        void OnSceneGUI()
        {
            Event e = Event.current;
            var cameraNode = (CameraNode.CameraNode)target;
            Debug.Log($"{cameraNode.transform.position} {e.type} OnSceneGUI");

            if (e.type == EventType.MouseDown)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    var hitNode = hit.collider.GetComponent<CameraNode.CameraNode>();
                    if (hitNode != null)
                    {
                        if (hitNode == cameraNode)
                        {
                            startNode = startNode == cameraNode ? null : cameraNode;
                        }
                    }
                    else
                    {
                        startNode = null;
                    }
                }
                else
                {
                    startNode = null;
                }
            }

            if (startNode == cameraNode)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                Handles.color = Color.yellow;
                Handles.DrawLine(cameraNode.transform.position, ray.origin);
            }
            
            HandleUtility.Repaint();
        }
    }
}