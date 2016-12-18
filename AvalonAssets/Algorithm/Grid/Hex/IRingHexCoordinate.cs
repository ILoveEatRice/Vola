using AvalonAssets.Utility;

namespace AvalonAssets.Algorithm.Grid.Hex
{
    /// <summary>
    ///     Relative coordinate of <see cref="IHexCoordinate" />.
    /// </summary>
    public interface IRingHexCoordinate : IRingCoordinate, IConvertible<IHexCoordinate>
    {
    }
}