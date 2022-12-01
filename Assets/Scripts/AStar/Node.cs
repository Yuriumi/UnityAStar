using UnityEngine;
using System;

namespace LearnUnity.AStar
{
    public class Node : IComparable<Node>
    {
        public Vector2Int nodePosition;

        public int gCost;
        public int hCost;
        public int fCost => gCost + hCost;

        public bool isObstacle;

        public Node nodeParent;

        public Node(Vector2Int nodePosition)
        {
            this.nodePosition = nodePosition;
            nodeParent = null;
        }

        public int CompareTo(Node other)
        {
            int result = fCost.CompareTo(other.fCost);

            if (result == 0)
            {
                result = hCost.CompareTo(other.hCost);
            }

            return result;
        }
    }
}