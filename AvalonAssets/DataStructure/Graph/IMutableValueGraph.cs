namespace AvalonAssets.DataStructure.Graph
{
    public interface IMutableValueGraph<TNode, TValue> : IValueGraph<TNode, TValue>
    {
        bool AddNode(TNode node);
        bool PutEdge(TNode nodeU, TNode nodeV, TValue value);
        bool RemoveNode(TNode node);
        bool RemoveEdge(TNode nodeU, TNode nodeV);
    }
}