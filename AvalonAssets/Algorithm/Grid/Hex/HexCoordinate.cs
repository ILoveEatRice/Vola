using System;
using AvalonAssets.Utility;

namespace AvalonAssets.Algorithm.Grid.Hex
{
    internal class HexCoordinate : IHexCoordinate
    {
        private readonly int _q;
        private readonly int _r;
        internal HexCoordinate(int x, int y, int z)
        {

            if (x + y + z != 0)
                throw new ArgumentException("Invalid coordinates.");
            _q = x;
            _r = z;
        }
        internal HexCoordinate(int q, int r)
        {
            _q = q;
            _r = r;
        }
        public int X
        {
            get { return _q; }
        }
        
        public int Y
        {
            get { return -_q - _r; }
        }
        
        public int Z
        {
            get { return _r; }
        }
        
        public int Q
        {
            get { return _q; }
        }
        
        public int R
        {
            get { return _r; }
        }


        public override int GetHashCode()
        {
            return HashUtils.IntegerHash(X, Y);
        }

        public override bool Equals(object obj)
        {
            var coord = obj as IHexCoordinate;
            if (coord != null)
                return Equals(coord);
            var ring = obj as IRingHexCoordinate;
            return ring != null && Equals(ring);
        }

        public bool Equals(IHexCoordinate obj)
        {
            return obj.Q == Q && obj.R == R;
        }

        public bool Equals(IRingHexCoordinate obj)
        {
            return Equals(obj.ConvertTo());
        }


        public override string ToString()
        {
            return string.Format("X, Y, Z: {0}, {1}, {2}", X, Y, Z);
        }
    }
}