namespace AvalonAssets.DataStructure.Graph.Hex
{
    public struct RingCoordinate
    {
        public readonly HexCoordinate Center;
        public readonly int Radius;
        public readonly int Index;

        public RingCoordinate(HexCoordinate center, int radius, int index)
        {
            Center = center;
            Radius = radius;
            Index = index;
        }

        public HexCoordinate ToHex()
        {
            return Center.FromRing(Radius, Index);
        }
    }
}