using System;
using System.Collections.Generic;
using AvalonAssets.DataStructure.Graph;
using AvalonAssets.DataStructure.Queue;

namespace AvalonAssets.Algorithm
{
    /// <summary>
    ///     Path finding algorithm implementaion.
    /// </summary>
    public static class PathFinding
    {
        /// <summary>
        ///     Returns the distance between <paramref name="node" /> and <paramref name="goal" />.
        /// </summary>
        /// <typeparam name="T">Graph node.</typeparam>
        /// <param name="goal">Goal node.</param>
        /// <param name="node">Current node.</param>
        /// <returns>Distance between <paramref name="node" /> and <paramref name="goal" /></returns>
        public delegate int Heuristic<in T>(T goal, T node);

        /// <summary>
        ///     Creates path from visit history.
        /// </summary>
        /// <typeparam name="T">Graph node.</typeparam>
        /// <param name="from">History of visit.</param>
        /// <param name="start">Start of the path.</param>
        /// <param name="goal">End of the path.</param>
        /// <returns>
        ///     Path from <paramref name="start" /> to <paramref cref="goal" />.
        ///     Empty if no path can be found.
        /// </returns>
        private static IEnumerable<T> CreatePath<T>(IDictionary<T, T> from, T start, T goal)
        {
            if (!from.ContainsKey(goal))
                return new List<T>();
            var current = goal;
            var path = new List<T> {current};
            while (!current.Equals(start))
            {
                current = from[current];
                path.Add(current);
            }
            path.Reverse();
            return path;
        }

        #region Weight

        /// <summary>
        ///     Using Dijkstra's Algorithm to find path with given <see cref="IWeightGraph{T}" />
        ///     from <paramref name="start" /> to <paramref cref="goal" />.
        /// </summary>
        /// <typeparam name="T">Graph node.</typeparam>
        /// <param name="graph">Weighted graph</param>
        /// <param name="start">Start of the path.</param>
        /// <param name="goal">End of the path.</param>
        /// <returns>
        ///     Path from <paramref name="start" /> to <paramref cref="goal" />.
        ///     Empty if no path can be found.
        /// </returns>
        public static IEnumerable<T> DijkstraAlgorithm<T>(IWeightGraph<T> graph, T start, T goal)
        {
            return DijkstraAlgorithm(start, goal, graph.Neighbors, graph.GetWeight);
        }

        /// <summary>
        ///     Using Dijkstra's Algorithm to find path with given <see cref="IWeightedGraphNode{T}" />
        ///     from <paramref name="start" /> to <paramref cref="goal" />.
        /// </summary>
        /// <param name="start">Start of the path.</param>
        /// <param name="goal">End of the path.</param>
        /// <returns>
        ///     Path from <paramref name="start" /> to <paramref cref="goal" />.
        ///     Empty if no path can be found.
        /// </returns>
        public static IEnumerable<T> DijkstraAlgorithm<T>(T start, T goal) where T : IWeightedGraphNode<T>
        {
            return DijkstraAlgorithm(start, goal, n => n.GetNeighbors(), (f, t) => f.GetWeight(t));
        }

        private static IEnumerable<T> DijkstraAlgorithm<T>(T start, T goal, Func<T, IEnumerable<T>> neighbors,
            Func<T, T, int> weight)
        {
            var frontiers = new PriorityQueue<T>();
            frontiers.Enqueue(0, start);
            var from = new Dictionary<T, T> {{start, default(T)}};
            var weights = new Dictionary<T, int> {{start, 0}};
            while (!frontiers.IsEmpty())
            {
                var frontier = frontiers.Dequeue();
                if (start.Equals(goal))
                    break;
                foreach (var neighbor in neighbors(frontier))
                {
                    var newCost = weights[frontier] + weight(frontier, neighbor);
                    if (weights.ContainsKey(neighbor) && newCost >= weights[neighbor]) continue;
                    weights[neighbor] = newCost;
                    frontiers.Enqueue(newCost, neighbor);
                    from[neighbor] = frontier;
                }
            }
            return CreatePath(from, start, goal);
        }

        /// <summary>
        ///     Using A* Algorithm to find path with given <see cref="IWeightGraph{T}" />
        ///     from <paramref name="start" /> to <paramref cref="goal" />.
        /// </summary>
        /// <typeparam name="T">Graph node.</typeparam>
        /// <param name="graph">Weighted graph</param>
        /// <param name="start">Start of the path.</param>
        /// <param name="goal">End of the path.</param>
        /// <param name="heuristic">Heuristic function.</param>
        /// <returns>
        ///     Path from <paramref name="start" /> to <paramref cref="goal" />.
        ///     Empty if no path can be found.
        /// </returns>
        /// <seealso cref="Heuristic{T}" />
        public static IEnumerable<T> AStarAlgorithm<T>(IWeightGraph<T> graph, T start, T goal, Heuristic<T> heuristic)
        {
            return AStarAlgorithm(start, goal, graph.Neighbors, graph.GetWeight, heuristic);
        }

        /// <summary>
        ///     Using A* Algorithm to find path with given <see cref="IWeightedGraphNode{T}" />
        ///     from <paramref name="start" /> to <paramref cref="goal" />.
        /// </summary>
        /// <param name="start">Start of the path.</param>
        /// <param name="goal">End of the path.</param>
        /// <param name="heuristic">Heuristic function.</param>
        /// <returns>
        ///     Path from <paramref name="start" /> to <paramref cref="goal" />.
        ///     Empty if no path can be found.
        /// </returns>
        /// <seealso cref="Heuristic{T}" />
        public static IEnumerable<T> AStarAlgorithm<T>(T start, T goal, Heuristic<T> heuristic)
            where T : IWeightedGraphNode<T>
        {
            return AStarAlgorithm(start, goal, n => n.GetNeighbors(), (f, t) => f.GetWeight(t), heuristic);
        }

        private static IEnumerable<T> AStarAlgorithm<T>(T start, T goal, Func<T, IEnumerable<T>> neighbors,
            Func<T, T, int> weight, Heuristic<T> heuristic)
        {
            var frontiers = new PriorityQueue<T>();
            frontiers.Enqueue(0, start);
            var from = new Dictionary<T, T> {{start, default(T)}};
            var weights = new Dictionary<T, int> {{start, 0}};
            while (!frontiers.IsEmpty())
            {
                var frontier = frontiers.Dequeue();
                if (start.Equals(goal))
                    break;
                foreach (var neighbor in neighbors(frontier))
                {
                    var newCost = weights[frontier] + weight(frontier, neighbor);
                    if (weights.ContainsKey(neighbor) && newCost >= weights[neighbor]) continue;
                    weights[neighbor] = newCost;
                    frontiers.Enqueue(newCost + heuristic(goal, neighbor), neighbor);
                    from[neighbor] = frontier;
                }
            }
            return CreatePath(from, start, goal);
        }

        #endregion

        #region Non-weight

        /// <summary>
        ///     Using Breadth First Search to find path with given <see cref="IGraphNode{T}" />
        ///     from <paramref name="start" /> to <paramref cref="goal" />.
        /// </summary>
        /// <param name="start">Start of the path.</param>
        /// <param name="goal">End of the path.</param>
        /// <returns>
        ///     Path from <paramref name="start" /> to <paramref cref="goal" />.
        ///     Empty if no path can be found.
        /// </returns>
        public static IEnumerable<T> BreadthFirstSearch<T>(T start, T goal) where T : IGraphNode<T>
        {
            return BreadthFirstSearch(start, goal, n => n.GetNeighbors());
        }

        /// <summary>
        ///     Using Breadth First Search to find path with given <see cref="IGraph{T}" />
        ///     from <paramref name="start" /> to <paramref cref="goal" />.
        /// </summary>
        /// <typeparam name="T">Graph node.</typeparam>
        /// <param name="graph">Weighted graph</param>
        /// <param name="start">Start of the path.</param>
        /// <param name="goal">End of the path.</param>
        /// <returns>
        ///     Path from <paramref name="start" /> to <paramref cref="goal" />.
        ///     Empty if no path can be found.
        /// </returns>
        public static IEnumerable<T> BreadthFirstSearch<T>(IGraph<T> graph, T start, T goal)
        {
            return BreadthFirstSearch(start, goal, graph.Neighbors);
        }

        private static IEnumerable<T> BreadthFirstSearch<T>(T start, T goal, Func<T, IEnumerable<T>> neighbors)
        {
            var frontiers = new Queue<T>();
            frontiers.Enqueue(start);
            var from = new Dictionary<T, T>();
            from.Add(start, default(T));
            while (frontiers.Count > 0)
            {
                var frontier = frontiers.Dequeue();
                if (start.Equals(goal))
                    break;
                foreach (var neighbor in neighbors(frontier))
                {
                    if (from.ContainsKey(neighbor)) continue;
                    frontiers.Enqueue(neighbor);
                    from.Add(neighbor, frontier);
                }
            }
            return CreatePath(from, start, goal);
        }

        /// <summary>
        ///     Using Heuristic Search to find path with given <see cref="IGraphNode{T}" />
        ///     from <paramref name="start" /> to <paramref cref="goal" />.
        /// </summary>
        /// <param name="start">Start of the path.</param>
        /// <param name="goal">End of the path.</param>
        /// <param name="heuristic">Heuristic function.</param>
        /// <returns>
        ///     Path from <paramref name="start" /> to <paramref cref="goal" />.
        ///     Empty if no path can be found.
        /// </returns>
        /// <seealso cref="Heuristic{T}" />
        public static IEnumerable<T> HeuristicSearch<T>(T start, T goal, Heuristic<T> heuristic) where T : IGraphNode<T>
        {
            return HeuristicSearch(start, goal, n => n.GetNeighbors(), heuristic);
        }

        /// <summary>
        ///     Using Heuristic Search to find path with given <see cref="IGraph{T}" />
        ///     from <paramref name="start" /> to <paramref cref="goal" />.
        /// </summary>
        /// <typeparam name="T">Graph node.</typeparam>
        /// <param name="graph">Weighted graph</param>
        /// <param name="start">Start of the path.</param>
        /// <param name="goal">End of the path.</param>
        /// <param name="heuristic">Heuristic function.</param>
        /// <returns>
        ///     Path from <paramref name="start" /> to <paramref cref="goal" />.
        ///     Empty if no path can be found.
        /// </returns>
        /// <seealso cref="Heuristic{T}" />
        public static IEnumerable<T> HeuristicSearch<T>(IGraph<T> graph, T start, T goal, Heuristic<T> heuristic)
        {
            return HeuristicSearch(start, goal, graph.Neighbors, heuristic);
        }

        private static IEnumerable<T> HeuristicSearch<T>(T start, T goal, Func<T, IEnumerable<T>> neighbors,
            Heuristic<T> heuristic)
        {
            var frontiers = new PriorityQueue<T>();
            frontiers.Enqueue(0, start);
            var from = new Dictionary<T, T> {{start, default(T)}};
            while (!frontiers.IsEmpty())
            {
                var frontier = frontiers.Dequeue();
                if (start.Equals(goal))
                    break;
                foreach (var neighbor in neighbors(frontier))
                {
                    if (from.ContainsKey(neighbor)) continue;
                    frontiers.Enqueue(heuristic(goal, neighbor), neighbor);
                    from.Add(neighbor, frontier);
                }
            }
            return CreatePath(from, start, goal);
        }

        #endregion
    }
}