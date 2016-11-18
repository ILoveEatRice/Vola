namespace AvalonAssets.DataStructure.Graph
{
    public interface IMutableGraph<TNode> : IGraph<TNode>
    {
        bool AddNode(TNode node);
        bool PutEdge(TNode nodeU, TNode nodeV);
        bool RemoveNode(TNode node);
        bool RemoveEdge(TNode nodeU, TNode nodeV);
    }
}