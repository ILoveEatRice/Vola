namespace AvalonAssets.DataStructure.Graph
{
    public interface IValueGraph <TNode, TValue> : IGraph<TNode>
    {
        TValue EdgeValue(TNode nodeU, TNode nodeV);
        TValue EdgeValueOrDefault(TNode nodeU, TNode nodeV, TValue defaultValue);
    }
}