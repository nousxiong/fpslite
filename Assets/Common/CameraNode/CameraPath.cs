using System;
using UnityEngine;

namespace Common.CameraNode
{
    public class CameraPath : MonoBehaviour
    {
        [NonSerialized]
        public CameraNode Begin;
        [NonSerialized]
        public CameraNode End;

        void OnDrawGizmos()
        {
            if (Begin == null || End == null)
            {
                return;
            }
            
            Gizmos.DrawLine(Begin.transform.position, End.transform.position);
        }
    }
}