using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AvalonAssets.Algorithm.Grid.Hex;
using AvalonAssets.DataStructure.Graph;

namespace AvalonAssets.DataStructure.Grid
{
    public class HexGrid<T> : IGraph<HexCoordinate>, IEnumerable<KeyValuePair<HexCoordinate, T>>
    {
        private readonly Dictionary<HexCoordinate, T> _grid;

        public HexGrid()
        {
            _grid = new Dictionary<HexCoordinate, T>();
        }

        public T this[HexCoordinate coordinate]
        {
            get { return _grid[coordinate]; }
            set { _grid[coordinate] = value; }
        }

        public int Count
        {
            get { return _grid.Count; }
        }

        public Dictionary<HexCoordinate, T>.KeyCollection Keys
        {
            get { return _grid.Keys; }
        }

        public Dictionary<HexCoordinate, T>.ValueCollection Values
        {
            get { return _grid.Values; }
        }

        public void Add(HexCoordinate coordinate, T value)
        {
            _grid.Add(coordinate, value);
        }

        public bool TryGetValue(HexCoordinate coordinate, out T value)
        {
            return _grid.TryGetValue(coordinate, out value);
        }

        public bool ContainsKey(HexCoordinate coordinate)
        {
            return _grid.ContainsKey(coordinate);
        }

        public bool ContainsValue(T value)
        {
            return _grid.ContainsValue(value);
        }

        #region IGraph

        bool IGraph<HexCoordinate>.AllowSelfLoops
        {
            get { return true; }
        }

        IEnumerable<EndPointPair<HexCoordinate>> IGraph<HexCoordinate>.Edges
        {
            get
            {
                var returnSet = new HashSet<EndPointPair<HexCoordinate>>();
                foreach (var node in _grid.Keys)
                {
                    var negihbors = node.AllNeighbors().Where(n => _grid.ContainsKey(n)).ToList();
                    foreach (var negihbor in negihbors)
                    {
                        var pair = EndPointPair<HexCoordinate>.Undirected(node, negihbor);
                        if (returnSet.Contains(pair)) continue;
                        returnSet.Add(pair);
                        yield return pair;
                    }
                }
            }
        }

        bool IGraph<HexCoordinate>.IsDirected
        {
            get { return false; }
        }

        IEnumerable<HexCoordinate> IGraph<HexCoordinate>.Nodes
        {
            get { return _grid.Keys; }
        }

        IEnumerable<HexCoordinate> IGraph<HexCoordinate>.Neighbors(HexCoordinate node)
        {
            return node.AllNeighbors().Where(n => _grid.ContainsKey(n));
        }

        int IGraph<HexCoordinate>.Degree(HexCoordinate node)
        {
            return node.AllNeighbors().Count(n => _grid.ContainsKey(n));
        }

        int IGraph<HexCoordinate>.InDegree(HexCoordinate node)
        {
            return node.AllNeighbors().Count(n => _grid.ContainsKey(n));
        }

        int IGraph<HexCoordinate>.OutDegree(HexCoordinate node)
        {
            return node.AllNeighbors().Count(n => _grid.ContainsKey(n));
        }

        IEnumerable<HexCoordinate> IGraph<HexCoordinate>.Predecessors(HexCoordinate node)
        {
            return node.AllNeighbors().Where(n => _grid.ContainsKey(n));
        }

        IEnumerable<HexCoordinate> IGraph<HexCoordinate>.Successors(HexCoordinate node)
        {
            return node.AllNeighbors().Where(n => _grid.ContainsKey(n));
        }

        #endregion

        #region IEnumerable

        public IEnumerator<KeyValuePair<HexCoordinate, T>> GetEnumerator()
        {
            return _grid.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}