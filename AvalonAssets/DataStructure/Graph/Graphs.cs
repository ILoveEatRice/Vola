using System;
using System.Collections.Generic;
using System.Linq;

namespace AvalonAssets.DataStructure.Graph
{
    public static class Graphs
    {
        public static IMutableValueGraph<TNode, TValue> UndriectedValue<TNode, TValue>(bool allowSelfLoops = false)
        {
            return new UndirectedValueGraph<TNode, TValue>(allowSelfLoops);
        }

        private class UndirectedValueGraph<TNode, TValue> : AbstractValueGraph<TNode, TValue>,
            IMutableValueGraph<TNode, TValue>
        {
            private IDictionary<TNode, GraphConnection<TNode, TValue>> _connections;
            public UndirectedValueGraph(bool allowSelfLoops) : base(allowSelfLoops, false)
            {
                _connections = new Dictionary<TNode, GraphConnection<TNode, TValue>>();
            }

            public override IEnumerable<EndPointPair<TNode>> Edges
            {
                get { throw new NotImplementedException(); }
            }

            public override IEnumerable<TNode> Nodes
            {
                get { throw new NotImplementedException(); }
            }

            public override IEnumerable<TNode> Neighbors(TNode node)
            {
                return _connections.Keys;
            }

            public override IEnumerable<TNode> Predecessors(TNode node)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<TNode> Successors(TNode node)
            {
                throw new NotImplementedException();
            }

            public override TValue EdgeValueOrDefault(TNode nodeU, TNode nodeV, TValue defaultValue)
            {
                throw new NotImplementedException();
            }

            public bool AddNode(TNode node)
            {
                throw new NotImplementedException();
            }

            public bool PutEdge(TNode nodeU, TNode nodeV, TValue value)
            {
                throw new NotImplementedException();
            }

            public bool RemoveNode(TNode node)
            {
                throw new NotImplementedException();
            }

            public bool RemoveEdge(TNode nodeU, TNode nodeV)
            {
                throw new NotImplementedException();
            }
        }
    }
}