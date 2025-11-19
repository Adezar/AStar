using UnityEngine;

namespace AStar.GridModel
{
    public class Node
    {
        public Vector2Int Position;
        
        public bool Walkable = true;
        public bool Start;
        public bool End;
        
        public float GCost;
        public float HCost;
        public float FCost => GCost + HCost;
        
        public Node Parent { get; set; }
        public bool OpenSet { get; set; }
        
        public bool FinalPath { get; set; }
        public bool ClosedSet { get; set; }

        public Node(Vector2Int position)
        {
            Position = position;
        }
    }
}