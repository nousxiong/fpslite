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
            Gizmos.DrawLine(Begin.transform.position, End.transform.position);
        }
    }
}