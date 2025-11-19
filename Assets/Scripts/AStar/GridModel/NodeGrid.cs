using System.Collections.Generic;
using UnityEngine;

namespace AStar.GridModel
{
    public class NodeGrid
    {
        private Node[,] _grid;
        
        public Node this[int x, int y] => _grid[x, y];

        public int Width => _grid?.GetLength(0) ?? 0;
        public int Height => _grid?.GetLength(1) ?? 0;
        public int Count => _grid?.Length ?? 0;

        public Node StartNode;
        public Node EndNode;

        public void ClearGrid()
        {
            _grid = null;
        }

        public void NewNodeGrid(int width, int height)
        {
            _grid = new Node[width, height];
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    _grid[x, y] = new Node(new Vector2Int(x, y));
                    
                    if (x == 0 && y == 0)
                    {
                        StartNode = _grid[x, y];
                        StartNode.Start = true;
                    }
                    
                    if (x == width - 1 && y == height - 1)
                    {
                        EndNode = _grid[x, y];
                        EndNode.End = true;
                    }
                }
            }
        }
        
        public void SetStart(Vector2Int point)
        {
            if (_grid[point.x, point.y].Start) return;
            
            foreach (var node in _grid)
            {
                if (node.Start)
                {
                    node.Start = false;
                    break;
                }
            }
            _grid[point.x, point.y].Start = true;
            _grid[point.x, point.y].Walkable = true;
            
            StartNode = _grid[point.x, point.y];
        }

        public void SetEnd(Vector2Int point)
        {
            if (_grid[point.x, point.y].End) return;
            
            foreach (var node in _grid)
            {
                if (node.End)
                {
                    node.End = false;
                    break;
                }
            }
            _grid[point.x, point.y].End = true;
            _grid[point.x, point.y].Walkable = true;
            
            EndNode = _grid[point.x, point.y];
        }
        
        public void SetWalkable(Vector2Int point)
        {
            _grid[point.x, point.y].Walkable = true;
        }
        
        public void SetUnwalkable(Vector2Int point)
        {
            if (_grid[point.x, point.y].Start || _grid[point.x, point.y].End) return;
            
            _grid[point.x, point.y].Walkable = false;
        }

        public IEnumerable<Node> GetNodes()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    yield return _grid[x, y];
                }
            }
        }

        public void ClearPathFlags()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    _grid[x, y].OpenSet = false;
                    _grid[x, y].ClosedSet = false;
                    _grid[x, y].FinalPath = false;
                }
            }
        }
    }
}