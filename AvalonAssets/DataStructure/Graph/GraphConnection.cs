using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace AvalonAssets.DataStructure.Graph
{
    internal class GraphConnection<TNode, TValue>
    {
        private readonly IDictionary<TNode, NodeValue<TValue>> _neighbors;

        internal GraphConnection()
        {
            _neighbors = new Dictionary<TNode, NodeValue<TValue>>();
        }

        internal IEnumerable<TNode> Predecessors()
        {
            return _neighbors.Where(n => (n.Value.Type & ConnectionType.Predecessor) == ConnectionType.Predecessor)
                .Select(n => n.Key);
        }


        internal bool IsPredecessors(TNode node)
        {
            CheckNodeExist(node);
            return Contains(_neighbors[node].Type, ConnectionType.Predecessor);
        }

        internal IEnumerable<TNode> Successors()
        {
            return _neighbors.Where(n => (n.Value.Type & ConnectionType.Successor) == ConnectionType.Predecessor)
                .Select(n => n.Key);
        }

        internal bool IsSuccessors(TNode node)
        {
            CheckNodeExist(node);
            return Contains(_neighbors[node].Type, ConnectionType.Successor);
        }

        internal IEnumerable<TNode> Neighbors()
        {
            return _neighbors.Keys;
        }

        internal TValue Value(TNode node)
        {
            CheckNodeExist(node);
            return _neighbors[node].Value;
        }

        internal TValue RemovePredecessor(TNode node)
        {
            return RemoveConnection(node, ConnectionType.Predecessor);
        }

        internal TValue RemoveSuccessor(TNode node)
        {
            return RemoveConnection(node, ConnectionType.Successor);
        }

        [AssertionMethod]
        private void CheckNodeExist(TNode node)
        {
            if (!_neighbors.ContainsKey(node))
                throw new ArgumentException("The node does not exists.");
        }

        private TValue RemoveConnection(TNode node, ConnectionType type)
        {
            CheckNodeExist(node);
            var nodeValue = _neighbors[node];
            if (Contains(nodeValue.Type, type))
                throw new InvalidOperationException(string.Format("The node is not a {0}.", type));
            var newType = nodeValue.Type & ~type;
            if (newType == ConnectionType.None)
                _neighbors.Remove(node);
            else
                _neighbors[node] = new NodeValue<TValue>(newType, nodeValue.Value);
            return nodeValue.Value;
        }

        internal void AddPredecessor(TNode node, TValue value)
        {
            AddConnection(node, value, ConnectionType.Predecessor);
        }

        internal void AddSuccessor(TNode node, TValue value)
        {
            AddConnection(node, value, ConnectionType.Successor);
        }

        private void AddConnection(TNode node, TValue value, ConnectionType type)
        {
            if (_neighbors.ContainsKey(node))
            {
                var nodeValue = _neighbors[node];
                _neighbors[node] = new NodeValue<TValue>(nodeValue.Type & type, value);
            }
            else
                _neighbors[node] = new NodeValue<TValue>(type, value);
        }

        private static bool Contains(ConnectionType type, ConnectionType checkType)
        {
            return (type & checkType) != checkType;
        }

        [Flags]
        private enum ConnectionType : ushort
        {
            None = 0,
            Predecessor = 1 << 0,
            Successor = 1 << 1
        }

        private class NodeValue<TNodeValue>
        {
            public readonly ConnectionType Type;
            public readonly TNodeValue Value;

            public NodeValue(ConnectionType type, TNodeValue value)
            {
                Type = type;
                Value = value;
            }
        }
    }
}