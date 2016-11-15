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

        /// <summary>
        ///     Return true if two double are about equal or greater than.
        /// </summary>
        /// <param name="left">Self.</param>
        /// <param name="right">Double to be compare.</param>
        /// <returns>Is two double about equal or greater than.</returns>
        /// <seealso cref="AboutEqual" />
        public static bool AboutGreaterThanOrEqual(this double left, double right)
        {
            return left > right || AboutEqual(left, right);
        }

        /// <summary>
        ///     Return true if two double are about equal or lesser than.
        /// </summary>
        /// <param name="left">Self.</param>
        /// <param name="right">Double to be compare.</param>
        /// <returns>Is two double about equal or lesser than.</returns>
        /// <seealso cref="AboutEqual" />
        public static bool AboutLesserThanOrEqual(this double left, double right)
        {
            return left < right || AboutEqual(left, right);
        }

        /// <summary>
        ///     Converts from degree to Radians.
        /// </summary>
        /// <param name="value">Double in degree.</param>
        /// <returns>Double in Radians.</returns>
        public static double ToRadians(this double value)
        {
            return Math.PI/180*value;
        }

        /// <summary>
        ///     Returns angle where 0 &lt;= x &lt; 360.
        /// </summary>
        /// <param name="angle">Double in degree.</param>
        /// <returns>Normalized angle.</returns>
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