using UnityEngine;

namespace Common.Utility
{
    public static class Vector3Utility
    {
        /// <summary>
        /// 获取2个Vector3之间的锐角夹角
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static float AcuteAngle(Vector3 from, Vector3 to)
        {
            var angle = Vector3.Angle(from, to);
            if (angle > 90f)
            {
                angle = 180f - angle;
            }
            return angle;
        }
    }
}
