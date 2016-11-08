using System;
using System.Collections.Generic;
using System.Linq;
using AvalonAssets.Algorithm;
using AvalonAssets.DataStructure.Graph;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvalonAssetsTests.Algorithm
{
    [TestClass]
    public class PathFindingTests
    {
        [TestMethod]
        public void DijkstraAlgorithmTest()
        {
            var nodeA = new TestWeightGraphNode(1);
            var nodeB = new TestWeightGraphNode(2);
            var nodeC = new TestWeightGraphNode(0);
            var nodeD = new TestWeightGraphNode(3);
            var nodeE = new TestWeightGraphNode(4);
            var nodeF = new TestWeightGraphNode(5);
            var nodeG = new TestWeightGraphNode(7);
            /**
             * D - E - F - G
             * |       |
             * B - A - C
             */
            nodeA.AddNeighbors(nodeB, nodeC);
            nodeB.AddNeighbors(nodeA, nodeD);
            nodeC.AddNeighbors(nodeA, nodeF);
            nodeD.AddNeighbors(nodeB, nodeE);
            nodeE.AddNeighbors(nodeD, nodeF);
            nodeF.AddNeighbors(nodeE, nodeC, nodeG);
            var path = PathFinding.DijkstraAlgorithm(nodeA, nodeG).Select(n => n.Value).ToList();
            Console.WriteLine(string.Join(",", path));
            var result = new List<int> { nodeA.Value, nodeB.Value,nodeD.Value, nodeE.Value, nodeF.Value, nodeG.Value };
            Console.WriteLine(string.Join(",", result));
            Assert.IsTrue(result.SequenceEqual(path));
        }

        [TestMethod]
        public void AStarAlgorithmTest()
        {
            var nodeA = new TestWeightGraphNode(1);
            var nodeB = new TestWeightGraphNode(2);
            var nodeC = new TestWeightGraphNode(0);
            var nodeD = new TestWeightGraphNode(3);
            var nodeE = new TestWeightGraphNode(4);
            var nodeF = new TestWeightGraphNode(5);
            var nodeG = new TestWeightGraphNode(7);
            /**
             * D - E - F - G
             * |       |
             * B - A - C
             */
            nodeA.AddNeighbors(nodeB, nodeC);
            nodeB.AddNeighbors(nodeA, nodeD);
            nodeC.AddNeighbors(nodeA, nodeF);
            nodeD.AddNeighbors(nodeB, nodeE);
            nodeE.AddNeighbors(nodeD, nodeF);
            nodeF.AddNeighbors(nodeE, nodeC, nodeG);
            PathFinding.Heuristic<TestWeightGraphNode> method = (a, b) =>
            {
                switch (b.Value)
                {
                    case 1:
                        return 1;
                    case 2:
                        return 1;
                    case 3:
                        return 1;
                    case 4:
                        return 1;
                    case 5:
                        return 1;
                    case 6:
                        return 1;
                    case 7:
                        return 1;
                    default:
                        return -1;
                }
            };
            var path = PathFinding.AStarAlgorithm(nodeA, nodeG, method).Select(n => n.Value).ToList();
            Console.WriteLine(string.Join(",", path));
            var result = new List<int> { nodeA.Value, nodeB.Value, nodeD.Value, nodeE.Value, nodeF.Value, nodeG.Value };
            Console.WriteLine(string.Join(",", result));
            Assert.IsTrue(result.SequenceEqual(path));
        }


        [TestMethod]
        public void BreadthFirstSearchTest()
        {
            var nodeA = new TestGraphNode(1);
            var nodeB = new TestGraphNode(2);
            var nodeC = new TestGraphNode(3);
            var nodeD = new TestGraphNode(4);
            var nodeE = new TestGraphNode(5);
            var nodeF = new TestGraphNode(6);
            var nodeG = new TestGraphNode(7);
            /**
             * D - E - F - G
             * |       |
             * B - A - C
             */
            nodeA.AddNeighbors(nodeB, nodeC);
            nodeB.AddNeighbors(nodeA, nodeD);
            nodeC.AddNeighbors(nodeA, nodeF);
            nodeD.AddNeighbors(nodeB, nodeE);
            nodeE.AddNeighbors(nodeD, nodeF);
            nodeF.AddNeighbors(nodeE, nodeC, nodeG);
            var path = PathFinding.BreadthFirstSearch(nodeA, nodeG).Select(n => n.Value).ToList();
            Console.WriteLine(string.Join(",", path));
            var result = new List<int> {nodeA.Value, nodeC.Value, nodeF.Value, nodeG.Value};
            Console.WriteLine(string.Join(",", result));
            Assert.IsTrue(result.SequenceEqual(path));
        }

        [TestMethod]
        public void HeuristicSearchTest()
        {
            var nodeA = new TestGraphNode(1);
            var nodeB = new TestGraphNode(2);
            var nodeC = new TestGraphNode(3);
            var nodeD = new TestGraphNode(4);
            var nodeE = new TestGraphNode(5);
            var nodeF = new TestGraphNode(6);
            var nodeG = new TestGraphNode(7);
            /**
             * D - E - F - G
             * |       |
             * B - A - C
             */
            nodeA.AddNeighbors(nodeB, nodeC);
            nodeB.AddNeighbors(nodeA, nodeD);
            nodeC.AddNeighbors(nodeA, nodeF);
            nodeD.AddNeighbors(nodeB, nodeE);
            nodeE.AddNeighbors(nodeD, nodeF);
            nodeF.AddNeighbors(nodeE, nodeC, nodeG);
            PathFinding.Heuristic<TestGraphNode> method = (a, b) =>
            {
                switch (b.Value)
                {
                    case 1:
                        return 3;
                    case 2:
                        return 4;
                    case 3:
                        return 2;
                    case 4:
                        return 3;
                    case 5:
                        return 2;
                    case 6:
                        return 1;
                    case 7:
                        return 0;
                    default:
                        return -1;
                }
            };
            var path = PathFinding.HeuristicSearch(nodeA, nodeG, method).Select(n => n.Value).ToList();
            Console.WriteLine(string.Join(",", path));
            var result = new List<int> {nodeA.Value, nodeC.Value, nodeF.Value, nodeG.Value};
            Console.WriteLine(string.Join(",", result));
            Assert.IsTrue(result.SequenceEqual(path));
        }

        public class TestGraphNode : IGraphNode<TestGraphNode>
        {
            private readonly HashSet<TestGraphNode> _graphNodes;

            public TestGraphNode(int value)
            {
                _graphNodes = new HashSet<TestGraphNode>();
                Value = value;
            }

            public int Value { get; }

            public IEnumerable<TestGraphNode> GetNeighbors()
            {
                return _graphNodes;
            }

            public void AddNeighbors(params TestGraphNode[] nodes)
            {
                foreach (var node in nodes)
                {
                    _graphNodes.Add(node);
                }
            }
        }
        public class TestWeightGraphNode : IWeightedGraphNode<TestWeightGraphNode>
        {
            private readonly HashSet<TestWeightGraphNode> _graphNodes;

            public TestWeightGraphNode(int value)
            {
                _graphNodes = new HashSet<TestWeightGraphNode>();
                Value = value;
            }

            public int Value { get; }

            public int GetWeight(TestWeightGraphNode neighbor)
            {
                return Math.Abs(Value - neighbor.Value);
            }

            public IEnumerable<TestWeightGraphNode> GetNeighbors()
            {
                return _graphNodes;
            }

            public void AddNeighbors(params TestWeightGraphNode[] nodes)
            {
                foreach (var node in nodes)
                {
                    _graphNodes.Add(node);
                }
            }
        }
    }
}