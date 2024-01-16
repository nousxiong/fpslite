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
            return angle switch
            {
                >= 360f => angle.InvertAngle(),
                <= -360f => angle.InvertAngle(),
                _ => angle
            };
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
    }
}
