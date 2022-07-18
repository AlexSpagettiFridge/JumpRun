using System;

namespace JumpRun.Scr.Misc
{
    public static class Util
    {
        public static float CalculateFriction(float from, float to, float friction)
        {
            if (from > to)
            {
                return Math.Max(to, from - friction);
            }
            if (from < to)
            {
                return Math.Min(to, from + friction);
            }
            return to;
        }
    }
}