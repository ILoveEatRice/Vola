using System;
using System.Collections.Generic;
using AvalonAssets.Utility;

namespace AvalonAssets.DataStructure.Graph.Square
{
    public class SquareCoordinate
    {
        #region Direction

        /*
         *    D
         *  C X A
         *    B
         */

        public enum Direction
        {
            A,
            B,
            C,
            D
        }

        #endregion

        private readonly int _x;
        private readonly int _y;


        public SquareCoordinate(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public int X
        {
            get { return _x; }
        }

        public int Y
        {
            get { return _y; }
        }

        public override int GetHashCode()
        {
            return HashUtils.IntegerHash(X, Y);
        }

        public override bool Equals(object obj)
        {
            var square = obj as SquareCoordinate;
            return square != null && Equals(square);
        }

        public bool Equals(SquareCoordinate obj)
        {
            return obj._x == _x && obj._y == _y;
        }

        public SquareCoordinate Neighbor(Direction direction, int radius = 1)
        {
            if (radius == 0)
                return this;
            if (radius < 0)
                throw new ArgumentOutOfRangeException("radius");
            var xOffset = 0;
            var yOffset = 0;
            switch (direction)
            {
                case Direction.A:
                    xOffset = 1;
                    break;
                case Direction.B:
                    yOffset = -1;
                    break;
                case Direction.C:
                    xOffset = -1;
                    break;
                case Direction.D:
                    yOffset = 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            xOffset *= radius;
            yOffset *= radius;
            return new SquareCoordinate(_x + xOffset, _y + yOffset);
        }

        public SquareCoordinate Diagonal(Direction direction)
        {
            int xOffset;
            int yOffset;
            switch (direction)
            {
                case Direction.A:
                    xOffset = 1;
                    yOffset = -1;
                    break;
                case Direction.B:
                    xOffset = -1;
                    yOffset = -1;
                    break;
                case Direction.C:
                    xOffset = -1;
                    yOffset = 1;
                    break;
                case Direction.D:
                    xOffset = 1;
                    yOffset = 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new SquareCoordinate(_x + xOffset, _y + yOffset);
        }

        public int Distance(SquareCoordinate squareCoordinate)
        {
            return Math.Abs(X - squareCoordinate.X) + Math.Abs(Y - squareCoordinate.Y);
        }

        public IEnumerable<SquareCoordinate> Range(int radius)
        {
            if (radius < 0)
                throw new ArgumentOutOfRangeException("radius");
            for (var x = -radius; x <= radius; x++)
                for (var y = -radius; y <= radius; y++)
                    yield return new SquareCoordinate(x, y) + this;
        }

        public static SquareCoordinate operator +(SquareCoordinate left, SquareCoordinate right)
        {
            return new SquareCoordinate(left.X + right.X, left.Y + right.Y);
        }

        public static SquareCoordinate operator -(SquareCoordinate left, SquareCoordinate right)
        {
            return new SquareCoordinate(left.X - right.X, left.Y - right.Y);
        }

        public static SquareCoordinate operator *(SquareCoordinate left, int right)
        {
            return new SquareCoordinate(left.X * right, left.Y * right);
        }

        public static SquareCoordinate operator /(SquareCoordinate left, int right)
        {
            return new SquareCoordinate(left.X / right, left.Y / right);
        }
    }
}