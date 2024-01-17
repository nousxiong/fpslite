using UnityEngine;

namespace Common.Utility
{
    public static class FloatExtensions
    {
        public static float InvertAngle(this float angle)
        {
            // return -(360f - angle);
            var absFloat = 360f - Mathf.Abs(angle);
            if (angle >= 0f)
            {
                absFloat = -absFloat;
            }
            return absFloat;
        }
        
        public static float ClampAngleIn360(this float angle)
        {
            return angle switch
            {
                >= 360f => angle - 360f,
                <= -360f => angle + 360f,
                _ => angle
            };
        }
        
        public static float InvertIfNegtive(this float angle)
        {
            return angle < 0f ? angle.InvertAngle() : angle;
        }
        
        public static float InvertIf180(this float angle)
        {
            return angle switch
            {
                >= 180f => angle.InvertAngle(),
                <= -180f => angle.InvertAngle(),
                _ => angle
            };
        }
        
        public static float InvertBySign(this float angle, float signAngle)
        {
            return angle switch
            {
                >= 180f when signAngle < 0f => angle.InvertAngle(),
                < -180f when signAngle >= 0f => angle.InvertAngle(),
                _ => angle
            };
        }

        public static bool IsInvertBySign(this float angle, float signAngle)
        {
            return angle switch
            {
                >= 180f when signAngle < 0f => true,
                < -180f when signAngle >= 0f => true,
                _ => false
            };
        }
    }
}
