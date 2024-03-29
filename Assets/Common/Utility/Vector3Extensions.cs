﻿using UnityEngine;

namespace Common.Utility
{
    public static class Vector3Extensions
    {
        public static Vector3 Abs(this Vector3 vector)
        {
            return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
        }

        public static Vector3 ProjectionXZ(this Vector3 vector)
        {
            return new Vector3(vector.x, 0, vector.z);
        }
        
        public static void ProjectionXZ(this Vector3 vector, out Vector3 projection)
        {
            projection = vector;
            projection.y = 0f;
        }
    }
}
