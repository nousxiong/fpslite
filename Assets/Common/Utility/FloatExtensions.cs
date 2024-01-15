namespace Common.Utility
{
    public static class FloatExtensions
    {
        public static float InvertAngle(this float angle)
        {
            return -(360f - angle);
        }
        
        public static float InvertIf360(this float angle)
        {
            if (angle >= 360f)
            {
                return angle.InvertAngle();
            }
            if (angle <= -360f)
            {
                return angle.InvertAngle();
            }
            return angle;
        }
        
        public static float InvertIf180(this float angle)
        {
            if (angle >= 180f)
            {
                return angle.InvertAngle();
            }
            if (angle <= -180f)
            {
                return angle.InvertAngle();
            }
            return angle;
        }
    }
}
