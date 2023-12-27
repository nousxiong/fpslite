﻿using UnityEditor;
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
            var currentNode = (CameraNode.CameraNode)target;
            Debug.Log($"{currentNode.transform.position} {e.type} OnSceneGUI");

            if (e.type == EventType.MouseDown)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    var hitNode = hit.collider.GetComponent<CameraNode.CameraNode>();
                    if (hitNode != null)
                    {
                        if (hitNode == currentNode)
                        {
                            startNode = startNode == currentNode ? null : currentNode;
                        }
                        else if (startNode != null)
                        {
                            // TODO 创建CameraPath
                            Debug.Log($"add new {hitNode.transform.position}");
                            startNode.Connect(hitNode, out _);
                            startNode = null;
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

            if (startNode == currentNode)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                Handles.color = Color.yellow;
                Handles.DrawLine(currentNode.transform.position, ray.origin);
            }
            
            HandleUtility.Repaint();
        }
    }
}