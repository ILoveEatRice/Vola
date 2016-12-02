using System;
using System.Linq;
using AvalonAssets.Utility;

namespace AvalonAssets.Algorithm.Grid.Hex
{
    internal class RingHexCoordinate : IRingHexCoordinate
    {
        private readonly IHexCoordinate _center;
        private readonly int _index;
        private readonly int _radius;

        public RingHexCoordinate(IHexCoordinate center, int radius, int index)
        {
            if (center == null)
                throw new ArgumentNullException("center");
            if (radius < 0)
                throw new ArgumentOutOfRangeException("radius");
            if (index < 0)
                throw new ArgumentOutOfRangeException("index");
            _radius = radius;
            _index = index;
            _center = center;
        }

        public IHexCoordinate ConvertTo()
        {
            if (Radius == 0)
                return _center;
            var ringSize = 6*Radius;
            var tweakedIndex = (Index + (Radius + 1)/2)%ringSize;
            var directionIndex = tweakedIndex/Radius;
            var direction = EnumUtils.Values<HexDirection>().Shift(-2).ToList();
            // Compute the ring index of the corner tile at the end of this spoke:
            var cornerIndex = directionIndex*Radius;
            // Compute how much further we still need to go:
            var excess = tweakedIndex - cornerIndex;
            return _center.Add(direction[directionIndex].Distance(Radius))
                .Add(direction[(directionIndex + 2)%6].Distance(excess));
        }

        public int Index
        {
            get { return _index; }
        }

        public int Radius
        {
            get { return _radius; }
        }

        public override bool Equals(object obj)
        {
            var coord = obj as IHexCoordinate;
            if (coord != null)
                return Equals(coord);
            var ring = obj as IRingHexCoordinate;
            return ring != null && Equals(ring);
        }

        public bool Equals(IRingHexCoordinate obj)
        {
            return Equals(ConvertTo(), obj.ConvertTo());
        }

        public bool Equals(IHexCoordinate obj)
        {
            return Equals(ConvertTo(), obj);
        }

        public override int GetHashCode()
        {
            return ConvertTo().GetHashCode();
        }
    }
}
