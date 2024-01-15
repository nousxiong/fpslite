namespace Common.Utility
{
    public static class FloatExtensions
    {
        public static float InvertAngle(this float angle)
        {
            return -(360f - angle);
        }
        
        public static float InvertIfNegative(this float angle)
        {
            return angle < 0f ? angle.InvertAngle() : angle;
        }
        
        public static float InvertIfPositive(this float angle)
        {
            return angle >= 0f ? angle.InvertAngle() : angle;
        }
    }
}
