using System.Collections.Generic;
using System.Linq;

namespace AvalonAssets.DataStructure.Graph
{
    public abstract class AbstractGraph<TNode> : IGraph<TNode>
    {
        private readonly bool _allowSelfLoops, _isDirected;

        protected AbstractGraph(bool allowSelfLoops, bool isDirected)
        {
            _allowSelfLoops = allowSelfLoops;
            _isDirected = isDirected;
        }

        public bool AllowSelfLoops
        {
            get { return _allowSelfLoops; }
        }

        public abstract IEnumerable<EndPointPair<TNode>> Edges { get; }

        public bool IsDirected
        {
            get { return _isDirected; }
        }

        public abstract IEnumerable<TNode> Nodes { get; }
        public abstract IEnumerable<TNode> Neighbors(TNode node);

        public virtual int Degree(TNode node)
        {
            if (IsDirected)
            {
                return Predecessors(node).Count() + Successors(node).Count();
            }
            var neighbors = Neighbors(node).ToList();
            var selfCount = AllowSelfLoops && neighbors.Contains(node) ? 1 : 0;
            return neighbors.Count + selfCount;
        }

        public virtual int InDegree(TNode node)
        {
            return IsDirected ? Predecessors(node).Count() : Degree(node);
        }

        public virtual int OutDegree(TNode node)
        {
            return IsDirected ? Successors(node).Count() : Degree(node);
        }

        public abstract IEnumerable<TNode> Predecessors(TNode node);

        public abstract IEnumerable<TNode> Successors(TNode node);

        public override string ToString()
        {
            return string.Format("Graph[IsDirected:{0}, AllowSelfLoops:{1}, Nodes:{2}, Edges:{3}]",
                IsDirected, AllowSelfLoops, Nodes, Edges);
        }
    }
}