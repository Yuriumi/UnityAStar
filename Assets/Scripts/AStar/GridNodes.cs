using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LearnUnity.AStar
{
    public class GridNodes
    {
        public Node[,] nodeGrids;

        public int gridWidth;
        public int gridHeight;

        public GridNodes(int gridWidth, int gridHeight)
        {
            this.gridWidth = gridWidth;
            this.gridHeight = gridHeight;

            nodeGrids = new Node[gridWidth, gridHeight];

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    nodeGrids[x, y] = new Node(new Vector2Int(x, y));
                }
            }
        }

        public Node GetGridNode(int gridX, int gridY)
        {
            if (gridX < 0 || gridY < 0) return null;
            return nodeGrids[gridX, gridY];
        }
    }
}