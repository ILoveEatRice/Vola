using System;
using System.Collections.Generic;
using System.Linq;
using AvalonAssets.Utility;

namespace AvalonAssets.DataStructure.Graph.Hex
{
    public class HexCoordinate
    {
        #region Direction

        /// <summary>
        ///     <para>
        ///         <see cref="A" /> represent the direction of positive Q. The order of direction is clockwise.
        ///     </para>
        /// </summary>
        /*
         *   E F
         *  D X A
         *   C B 
         */
        public enum Direction
        {
            A,
            B,
            C,
            D,
            E,
            F
        }

        #endregion

        private readonly int _q;
        private readonly int _r;

        private HexCoordinate(int x, int y, int z)
        {
            if (x + y + z != 0)
                throw new ArgumentException("Invalid coordinates.");
            _q = x;
            _r = z;
        }

        private HexCoordinate(int q, int r)
        {
            _q = q;
            _r = r;
        }

        /* Cube coordinate
         *     (0, 1, -1) (1, 0, -1)
         * (-1, 1, 0) (X, Y, Z) (1, -1, 0)
         *     (-1, 0, 1) (0, -1, 1)
         */

        /// <summary>
        ///     Cube coordinate X
        /// </summary>
        public int X
        {
            get { return _q; }
        }

        /// <summary>
        ///     Cube coordinate Y
        /// </summary>
        public int Y
        {
            get { return -_q - _r; }
        }

        /// <summary>
        ///     Cube coordinate Z
        /// </summary>
        public int Z
        {
            get { return _r; }
        }

        /* Axial coordinate
         *    (0, -1) (1, -1)
         * (-1, 0) (Q, R) (1, 0)
         *    (-1, 1) (0, 1)
         */

        /// <summary>
        ///     Axial coordinate Q
        /// </summary>
        public int Q
        {
            get { return _q; }
        }

        /// <summary>
        ///     Axial coordinate R
        /// </summary>
        public int R
        {
            get { return _r; }
        }

        /// <summary>
        ///     Create from cube coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public static HexCoordinate FromCube(int x, int y, int z)
        {
            return new HexCoordinate(x, y, z);
        }

        /// <summary>
        ///     Create from axial coordinates.
        /// </summary>
        /// <param name="q"></param>
        /// <param name="r"></param>
        public static HexCoordinate FromAxial(int q, int r)
        {
            return new HexCoordinate(q, r);
        }

        // Reference: http://stackoverflow.com/a/13871379/3673259
        //            http://szudzik.com/ElegantPairing.pdf
        public override int GetHashCode()
        {
            var a = (ulong) (X >= 0 ? 2*(long) X : -2*(long) X - 1);
            var b = (ulong) (Y >= 0 ? 2*(long) Y : -2*(long) Y - 1);
            var c = (long) ((a >= b ? a*a + a + b : a + b*b)/2);
            return (int) (X < 0 && Y < 0 || X >= 0 && Y >= 0 ? c : -c - 1);
        }

        public override bool Equals(object obj)
        {
            var coord = obj as HexCoordinate;
            return coord != null && Equals(coord);
        }

        public bool Equals(HexCoordinate obj)
        {
            return obj.Q == Q && obj.R == R;
        }

        /// <summary>
        ///     Create a new <see cref="HexCoordinate" /> from current position with given <paramref name="direction" />.
        /// </summary>
        /// <param name="direction">Direction of the new <see cref="HexCoordinate" /> from current position.</param>
        /// <param name="radius"></param>
        /// <returns>New <see cref="HexCoordinate" /></returns>
        /// <seealso cref="Direction" />
        public HexCoordinate Neighbor(Direction direction, int radius = 1)
        {
            var qOffset = 0;
            var rOffset = 0;
            switch (direction)
            {
                case Direction.A:
                    qOffset = 1;
                    break;
                case Direction.B:
                    rOffset = 1;
                    break;
                case Direction.C:
                    qOffset = -1;
                    rOffset = 1;
                    break;
                case Direction.D:
                    qOffset = -1;
                    break;
                case Direction.E:
                    rOffset = -1;
                    break;
                case Direction.F:
                    qOffset = 1;
                    rOffset = -1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            qOffset *= radius;
            rOffset *= radius;
            return new HexCoordinate(_q + qOffset, _r + rOffset);
        }

        /// <summary>
        ///     Create a new <see cref="HexCoordinate" /> from current position with given <paramref name="direction" />.
        ///     Diagonal of <see cref="Direction.A" /> is the other neighbor of <see cref="Direction.A" /> and
        ///     <see cref="Direction.B" />.
        /// </summary>
        /// <param name="direction">Direction of the new <see cref="HexCoordinate" /> from current position.</param>
        /// <returns>New <see cref="HexCoordinate" /></returns>
        /// <seealso cref="Direction" />
        public HexCoordinate Diagonal(Direction direction)
        {
            int qOffset;
            int rOffset;
            switch (direction)
            {
                case Direction.A:
                    qOffset = 1;
                    rOffset = 1;
                    break;
                case Direction.B:
                    qOffset = -1;
                    rOffset = 2;
                    break;
                case Direction.C:
                    qOffset = -2;
                    rOffset = 1;
                    break;
                case Direction.D:
                    qOffset = -1;
                    rOffset = -1;
                    break;
                case Direction.E:
                    qOffset = 1;
                    rOffset = -2;
                    break;
                case Direction.F:
                    qOffset = 2;
                    rOffset = -1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new HexCoordinate(_q + qOffset, _r + rOffset);
        }


        // http://gamedev.stackexchange.com/a/131567/92601
        public HexCoordinate FromRing(int radius, int index)
        {
            var ringSize = 6*radius;
            var tweakedIndex = (index + (radius + 1)/2 - 1)%ringSize;
            var directionIndex = tweakedIndex/radius;
            var direction = EnumUtils.Values<Direction>().Shift(-2).ToList();
            // Compute the ring index of the corner tile at the end of this spoke:
            var cornerIndex = directionIndex*radius;

            // Compute how much further we still need to go:
            var excess = tweakedIndex - cornerIndex;
            return this + Distance(direction[directionIndex], radius) +
                   Distance(direction[(directionIndex + 2)%6], excess);
        }

        /// <summary>
        ///     Convert a <see cref="HexCoordinate" /> to relative <see cref="RingCoordinate" />.
        /// </summary>
        /// <param name="coordinate"><see cref="HexCoordinate" /> to be converted.</param>
        /// <returns>Relative <see cref="RingCoordinate" /></returns>
        public RingCoordinate RingOf(HexCoordinate coordinate)
        {
            var different = coordinate - this;
            var xOffset = Math.Abs(different.X);
            var yOffset = Math.Abs(different.Y);
            var zOffset = Math.Abs(different.Z);

            int directionIndex;

            if (yOffset >= xOffset)
                if (zOffset >= yOffset)
                    directionIndex = different.Z > 0 ? 3 : 0;
                else
                    directionIndex = different.Y > 0 ? 5 : 2;
            else if (xOffset >= zOffset)
                directionIndex = different.X > 0 ? 1 : 4;
            else
                directionIndex = different.Z > 0 ? 3 : 0;

            var radius = (xOffset + yOffset + zOffset)/2;
            // Direction start from E
            var direction = EnumUtils.Values<Direction>().Shift(-2).ToList();
            // Find the counter-clockwise corner tile.
            var corner = Distance(direction[directionIndex], radius);

            different = coordinate - corner;
            xOffset = Math.Abs(different.X);
            yOffset = Math.Abs(different.Y);
            zOffset = Math.Abs(different.Z);
            var excess = (xOffset + yOffset + zOffset)/2;

            // Our tile is at the corner's index, plus this excess.
            var index = radius*directionIndex + excess;
            var ringSize = 6*radius;
            index = (index + ringSize - (radius + 1)/2)%ringSize + 1;
            return new RingCoordinate(this, radius, index);
        }

        /// <summary>
        ///     Returns distance between two <see cref="HexCoordinate" />.
        /// </summary>
        /// <param name="hexCoordinate"></param>
        /// <returns>Distance.</returns>
        public int Distance(HexCoordinate hexCoordinate)
        {
            return (Math.Abs(X - hexCoordinate.X) + Math.Abs(Y - hexCoordinate.Y) + Math.Abs(Z - hexCoordinate.Z))/2;
        }

        /// <summary>
        ///     Returns all <see cref="HexCoordinate" /> within <paramref name="radius" />.
        /// </summary>
        /// <param name="radius"></param>
        /// <returns></returns>
        public IEnumerable<HexCoordinate> Range(int radius)
        {
            if (radius < 0)
                throw new ArgumentOutOfRangeException();
            for (var dx = -radius; dx <= radius; dx++)
                for (var dy = Math.Max(-radius, -dx - radius); dy <= Math.Min(radius, -dx + radius); dy++)
                    yield return new HexCoordinate(dx, dy, -dx - dy);
        }

        /// <summary>
        ///     Returns true if there is nothing blocking between two <see cref="HexCoordinate" />.
        /// </summary>
        /// <param name="position"><see cref="HexCoordinate" /> to be checked.</param>
        /// <param name="isBlock"></param>
        /// <returns></returns>
        public bool Visible(HexCoordinate position, Func<HexCoordinate, bool> isBlock)
        {
            return !Line(position).Any(isBlock);
        }

        /// <summary>
        ///     Returns a <see cref="HexCoordinate" /> with given <paramref name="direction" /> and <paramref name="distance" />.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static HexCoordinate Distance(Direction direction, int distance)
        {
            return new HexCoordinate(0, 0).Neighbor(direction, distance);
        }

        /// <summary>
        ///     Returns all the <see cref="HexCoordinate" /> that can be reached by given <paramref name="steps" />.
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="isBlock"></param>
        /// <returns></returns>
        public IEnumerable<HexCoordinate> Reachable(int steps, Func<HexCoordinate, bool> isBlock)
        {
            if (steps < 0)
                throw new ArgumentOutOfRangeException();
            var visited = new HashSet<HexCoordinate> {this};
            var fringes = new List<HexCoordinate> {this};
            for (var i = 1; i <= steps; i++)
            {
                var newFringes = new List<HexCoordinate>();
                foreach (var coordinate in fringes)
                    foreach (var direction in EnumUtils.Values<Direction>())
                    {
                        var neighbor = coordinate.Neighbor(direction);
                        if (visited.Contains(neighbor) || isBlock(neighbor)) continue;
                        visited.Add(neighbor);
                        newFringes.Add(neighbor);
                        yield return neighbor;
                    }
                fringes = newFringes;
            }
        }

        /// <summary>
        ///     Returns a ring of <see cref="HexCoordinate" /> with given <paramref name="radius" />.
        /// </summary>
        /// <param name="radius"></param>
        /// <returns></returns>
        public IEnumerable<HexCoordinate> Ring(int radius)
        {
            if (radius < 0)
                throw new ArgumentOutOfRangeException();
            if (radius == 0)
            {
                yield return this;
                yield break;
            }
            var coordinate = this + Neighbor(Direction.A)*radius;
            var directions = EnumUtils.Values<Direction>().ToList();
            foreach (var direction in directions)
                for (var i = 0; i < radius; i++)
                {
                    yield return coordinate;
                    coordinate = coordinate.Neighbor(direction);
                }
        }

        /// <summary>
        ///     Returns a spiral ring of <see cref="HexCoordinate" /> with given <paramref name="radius" />.
        /// </summary>
        /// <param name="radius"></param>
        /// <returns></returns>
        public IEnumerable<HexCoordinate> Spiral(int radius)
        {
            if (radius < 0)
                throw new ArgumentOutOfRangeException();
            yield return this;
            for (var i = 1; i <= radius; i++)
                foreach (var coordinate in Ring(i))
                    yield return coordinate;
        }

        /// <summary>
        ///     Returns a approximate line between two <see cref="HexCoordinate" />.
        /// </summary>
        /// <param name="dest"></param>
        /// <returns></returns>
        public IEnumerable<HexCoordinate> Line(HexCoordinate dest)
        {
            var distance = Distance(dest);
            for (var i = 0; i <= distance; i++)
            {
                yield return Round(LinearInterpolation(X, dest.X, distance, i),
                    LinearInterpolation(Y, dest.Y, distance, i),
                    LinearInterpolation(Z, dest.Z, distance, i));
            }
        }

        private static double LinearInterpolation(int pA, int pB, int n, int i)
        {
            return pA + (pB - pA)*1.0/n*i;
        }

        private static HexCoordinate Round(double x, double y, double z)
        {
            var rX = (int) Math.Round(x);
            var rY = (int) Math.Round(y);
            var rZ = (int) Math.Round(z);

            var xDiff = Math.Abs(rX - x);
            var yDiff = Math.Abs(rY - y);
            var zDiff = Math.Abs(rZ - z);

            if (xDiff > yDiff && xDiff > yDiff)
                rX = -rY - rZ;
            else if (yDiff > zDiff)
                rY = -rX - rZ;
            else
                rZ = -rX - rY;
            return new HexCoordinate(rX, rY, rZ);
        }

        /// <summary>
        ///     Field Of View
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="isBlock"></param>
        /// <returns></returns>
        public IEnumerable<HexCoordinate> FieldOfView(int radius, Func<HexCoordinate, bool> isBlock)
        {
            if (radius < 0)
                throw new ArgumentOutOfRangeException();
            if (isBlock(this))
                yield break;
            yield return this;
            if (radius == 0)
                yield break;
            var shadowCast = new ShadowCast();
            for (var ringIndex = 1; ringIndex <= radius; ringIndex++)
            {
                /*
                var isEven = ringIndex%2 == 0;
                var slide = (double) 360/(6*ringIndex);
                var ring = Ring(ringIndex, Direction.F).Shift(-(ringIndex/2)).ToList();
                for (var hexIndex = 0; hexIndex < ringIndex*6; hexIndex++)
                {
                    var minAngle = hexIndex*slide;
                    if (isEven)
                        minAngle -= slide/2;
                    var maxAngle = minAngle + slide;
                    var center = (maxAngle + minAngle)/2.0;
                    if (shadowCast.Hide(center))
                        continue;
                    yield return ring[hexIndex];
                    if (isBlock(ring[hexIndex]))
                        shadowCast.AddShadow(minAngle, maxAngle);
                }*/
                var isEven = ringIndex%2 == 0;
                var slide = (double) 360/(6*ringIndex);
                for (var hexIndex = 0; hexIndex < ringIndex*6; hexIndex++)
                {
                    var coordinate = FromRing(ringIndex, hexIndex);
                    var minAngle = hexIndex*slide;
                    if (isEven)
                        minAngle -= slide/2;
                    var maxAngle = minAngle + slide;
                    var center = (maxAngle + minAngle)/2.0;
                    if (shadowCast.Hide(center))
                        continue;
                    yield return coordinate;
                    if (isBlock(coordinate))
                        shadowCast.AddShadow(minAngle, maxAngle);
                }
            }
        }

        /// <summary>
        ///     Returns all neighbors <see cref="HexCoordinate" />.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HexCoordinate> AllNeighbors()
        {
            return EnumUtils.Values<Direction>().Select(Neighbor);
        }

        /// <summary>
        ///     Use this as a center to rotate other <see cref="HexCoordinate" />.
        /// </summary>
        /// <param name="position"><see cref="HexCoordinate" /> to be rotated.</param>
        /// <param name="times">Number of 60°. Positive for clockwise.</param>
        /// <returns>Rotated <see cref="HexCoordinate" />.</returns>
        public HexCoordinate Rotate(HexCoordinate position, int times)
        {
            var vectorPx = position.X - X;
            var vectorPy = position.Y - Y;
            var vectorPz = position.Z - Z;
            var isOdd = times%2 == 1;
            times %= 3;
            var isPositive = times > 0;
            times = Math.Abs(times);
            for (var i = 0; i < times; i++)
            {
                int temp;
                if (isPositive)
                {
                    temp = vectorPx;
                    vectorPx = vectorPz;
                    vectorPz = vectorPy;
                    vectorPy = temp;
                }
                else
                {
                    temp = vectorPx;
                    vectorPx = vectorPy;
                    vectorPy = vectorPz;
                    vectorPz = temp;
                }
            }
            if (!isOdd) return new HexCoordinate(vectorPx + X, vectorPy + Y, vectorPz + Z);
            vectorPx = -vectorPx;
            vectorPy = -vectorPy;
            vectorPz = -vectorPz;
            return new HexCoordinate(vectorPx + X, vectorPy + Y, vectorPz + Z);
        }

        public static HexCoordinate operator +(HexCoordinate left, HexCoordinate right)
        {
            return new HexCoordinate(left.Q + right.Q, left.R + right.R);
        }

        public static HexCoordinate operator -(HexCoordinate left, HexCoordinate right)
        {
            return new HexCoordinate(left.Q - right.Q, left.R - right.R);
        }

        public static HexCoordinate operator *(HexCoordinate left, int right)
        {
            return new HexCoordinate(left.Q*right, left.R*right);
        }

        public static HexCoordinate operator /(HexCoordinate left, int right)
        {
            return new HexCoordinate(left.Q/right, left.R/right);
        }
    }
}