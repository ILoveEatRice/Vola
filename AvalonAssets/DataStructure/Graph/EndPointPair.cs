using System;
using System.Collections;
using System.Collections.Generic;

namespace AvalonAssets.DataStructure.Graph
{
    public class EndPointPair<TNode> : IEnumerable<TNode>
    {
        protected readonly TNode N;
        protected readonly TNode U;

        public EndPointPair(TNode u, TNode n)
        {
            U = u;
            N = n;
        }

        public virtual IEnumerator<TNode> GetEnumerator()
        {
            yield return U;
            yield return N;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(TNode node)
        {
            return Equals(N, node) || Equals(U, node);
        }

        public TNode Neighbor(TNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node");
            if (node.Equals(U))
                return N;
            if (node.Equals(N))
                return U;
            throw new InvalidOperationException("node does not exist in this edge.");
        }

        public override bool Equals(object obj)
        {
            var pair = obj as EndPointPair<TNode>;
            return pair != null && Contains(pair.U) && Contains(pair.N) && pair.Contains(U) && pair.Contains(N);
        }

        protected bool Equals(EndPointPair<TNode> other)
        {
            return EqualityComparer<TNode>.Default.Equals(N, other.N) &&
                   EqualityComparer<TNode>.Default.Equals(U, other.U);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashN = EqualityComparer<TNode>.Default.GetHashCode(N);
                var hashU = EqualityComparer<TNode>.Default.GetHashCode(U);
                return hashN*hashU + hashN + hashU;
            }
        }

        public override string ToString()
        {
            return string.Format("EndPointPair[{0}, {1}]", U, N);
        }
    }
}