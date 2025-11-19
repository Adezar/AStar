using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Mathf;
using AStar.GridModel;

namespace AStar
{
    public class AStarAlgorithm
    {
        private NodeGrid _nodeGrid;

        private List<Node> _openSet = new List<Node>();
        private HashSet<Node> _closedSet = new HashSet<Node>();
        
        public AStarAlgorithm(NodeGrid nodeGrid)
        {
            _nodeGrid = nodeGrid;
        }
        
        public async UniTask FindPath(bool useEightDirections, CancellationToken ctsToken, Action findPathFinished)
        {
            var startNode = _nodeGrid.StartNode;
            var endNode = _nodeGrid.EndNode;
            
            _openSet = new List<Node>();
            _closedSet = new HashSet<Node>();

            foreach (var node in _nodeGrid.GetNodes())
            {
                node.GCost = float.MaxValue;
                node.Parent = null;
                node.FinalPath = false;
                node.OpenSet = false;
                node.ClosedSet = false;
            }

            if (useEightDirections)
            {
                startNode.HCost = HeuristicEuclid(startNode.Position, endNode.Position);
            }
            else
            {
                startNode.HCost = HeuristicManhattan(startNode.Position, endNode.Position);
            }

            startNode.GCost = 0;

            EnqueueOpenSet(startNode);

            while (_openSet.Count > 0)
            {
                var currentNode = DequeueOpenSet();

                if (currentNode.Position == endNode.Position)
                {
                    currentNode.FinalPath = true;

                    var tempParent = currentNode.Parent;
                    tempParent.FinalPath = true;

                    while (tempParent.Parent != null)
                    {
                        tempParent = tempParent.Parent;
                        tempParent.FinalPath = true;
                    }
                    
                    Debug.Log("<color=green>We are arrived!</color>");
                    findPathFinished?.Invoke();
                    return;
                }

                currentNode.ClosedSet = true;
                _closedSet.Add(currentNode);

                var neighbors = GetNeighbors(currentNode, useEightDirections);
                for (int i = 0; i < neighbors.Count; i++)
                {
                    if (!neighbors[i].Walkable || _closedSet.Contains(neighbors[i])) continue;
                    
                    float tentativeG = currentNode.GCost + 1;

                    if (tentativeG < neighbors[i].GCost)
                    {
                        neighbors[i].Parent = currentNode;
                        neighbors[i].GCost = tentativeG;
                        
                        
                        if (useEightDirections)
                        {
                            neighbors[i].HCost = HeuristicEuclid(neighbors[i].Position, endNode.Position);
                        }
                        else
                        {
                            neighbors[i].HCost = HeuristicManhattan(neighbors[i].Position, endNode.Position);
                        }
                        
                        EnqueueOpenSet(neighbors[i]);
                        
                        await UniTask.Delay(100, cancellationToken: ctsToken);
                    }
                }
            }
            findPathFinished?.Invoke();
            Debug.Log("<color=red>Path not found!</color>");
        }

        private List<Node> GetNeighbors(Node node, bool eightDirections)
        {
            var neighbors = new List<Node>();
            
            var dirs = eightDirections ? new Vector2Int[]
                                                {
                                                    new(1,0), new(-1,0), new(0,1), new(0,-1),
                                                    new(1,1), new(-1,-1), new(1,-1), new(-1,1)
                                                } : new Vector2Int[]
                                                {
                                                    new(1,0), new(-1,0), new(0,1), new(0,-1)
                                                };

            foreach (var dir in dirs)
            {
                var nx = node.Position.x + dir.x;
                var ny = node.Position.y + dir.y;

                if (nx >= 0 && ny >= 0 && nx < _nodeGrid.Width && ny < _nodeGrid.Height)
                    neighbors.Add(_nodeGrid[nx, ny]);
            }

            return neighbors;
        }

        private Node DequeueOpenSet()
        {
            if (_openSet.Count == 0) return null;

            var bestNode = _openSet[0];
            
            for (int i = 0; i < _openSet.Count; i++)
            {
                if (bestNode.FCost > _openSet[i].FCost)
                {
                    bestNode = _openSet[i];
                    break;
                }
            }
            _openSet.Remove(bestNode);
            return bestNode;
        }

        private void EnqueueOpenSet(Node node)
        {
            node.OpenSet = true;
            _openSet.Add(node);
        }

        private int HeuristicManhattan(Vector2Int startPoint, Vector2Int endPoint)
        {
            return Abs(endPoint.x - startPoint.x) + Abs(endPoint.y - startPoint.y);
        }
        private float HeuristicEuclid(Vector2Int startPoint, Vector2Int endPoint)
        {
            return Sqrt((startPoint.x - endPoint.x) * (startPoint.x - endPoint.x) + (startPoint.y - endPoint.y) * (startPoint.y - endPoint.y));
        }
        
    }
}