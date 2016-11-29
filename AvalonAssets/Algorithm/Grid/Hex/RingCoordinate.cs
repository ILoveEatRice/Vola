using System;

namespace AvalonAssets.Algorithm.Grid.Hex
{
    public class RingCoordinate
    {
        public readonly HexCoordinate Center;
        public readonly int Index;
        public readonly int Radius;

        public RingCoordinate(HexCoordinate center, int radius, int index)
        {
            if (center == null)
                throw new ArgumentException("center");
            Center = center;
            Radius = radius;
            Index = index;
        }

        public HexCoordinate ToHex()
        {
            return Center.FromRing(Radius, Index);
        }

        public override bool Equals(object obj)
        {
            var coord = obj as HexCoordinate;
            if (coord != null)
                return Equals(coord);
            var ring = obj as RingCoordinate;
            return ring != null && Equals(ring);
        }

        public bool Equals(RingCoordinate obj)
        {
            return ToHex().Equals(obj.ToHex());
        }

        public bool Equals(HexCoordinate obj)
        {
            return ToHex().Equals(obj);
        }

        public override int GetHashCode()
        {
            return ToHex().GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Center: {0}; Radius: {1}; Index: {2}", Center, Radius, Index);
        }
    }
}