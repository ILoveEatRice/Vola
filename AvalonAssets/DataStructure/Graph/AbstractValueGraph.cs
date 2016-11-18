using System;

namespace AvalonAssets.DataStructure.Graph
{
    public abstract class AbstractValueGraph<TNode, TValue> : AbstractGraph<TNode>, IValueGraph<TNode, TValue>
    {
        protected AbstractValueGraph(bool allowSelfLoops, bool isDirected) : base(allowSelfLoops, isDirected)
        {
        }

        public virtual TValue EdgeValue(TNode nodeU, TNode nodeV)
        {
            var value = EdgeValueOrDefault(nodeU, nodeV, default(TValue));
            if (!Equals(value, default(TValue))) return value;
            throw new ArgumentException();
        }

        public abstract TValue EdgeValueOrDefault(TNode nodeU, TNode nodeV, TValue defaultValue);
    }
}