using System.Collections.Generic;

namespace AvalonAssets.DataStructure.Graph
{
    public class OrderEndPointPair<TNode> : EndPointPair<TNode>
    {
        public OrderEndPointPair(TNode source, TNode target) : base(source, target)
        {
        }

        public TNode Source
        {
            get { return U; }
        }

        public TNode Target
        {
            get { return N; }
        }

        public override bool Equals(object obj)
        {
            var pair = obj as OrderEndPointPair<TNode>;
            return pair != null && Equals(Source, pair.Source) && Equals(Target, pair.Target);
        }

        public override int GetHashCode()
        {
            {
                var hashN = EqualityComparer<TNode>.Default.GetHashCode(N);
                var hashU = EqualityComparer<TNode>.Default.GetHashCode(U);
                return (hashN*397) ^ hashU;
            }
        }

        public override string ToString()
        {
            return string.Format("OrderEndPointPair[Source:{0}, Target:{1}]", Source, Target);
        }
    }
}