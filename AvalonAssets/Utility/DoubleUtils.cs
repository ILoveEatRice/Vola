using System;

namespace AvalonAssets.Utility
{
    public static class DoubleUtils
    {
        /// <summary>
        ///     Return true if two double are about equal.
        ///     Not using Double.Epsilon, read the reference for the reason.
        ///     Reference: http://stackoverflow.com/a/2411661/3673259
        /// </summary>
        /// <param name="left">Self.</param>
        /// <param name="right">Double to be compare.</param>
        /// <returns>Is two double about equal.</returns>
        public static bool AboutEqual(this double left, double right)
        {
            return Math.Abs(left - right) <= Math.Max(Math.Abs(left), Math.Abs(right))*1E-15;
        }

        public static bool AboutGreaterThanOrEqual(this double left, double right)
        {
            return left > right || AboutEqual(left, right);
        }

        public static bool AboutLesserThanOrEqual(this double left, double right)
        {
            return left < right || AboutEqual(left, right);
        }

        public static double ToRadians(this double value)
        {
            return Math.PI/180*value;
        }

        public static double NormalizeAngle(double angle)
        {
            while (angle >= 360)
                angle -= 360;
            while (angle < 0)
                angle += 360;
            return angle;
        }
    }
}