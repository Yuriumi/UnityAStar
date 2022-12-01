using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

namespace LearnUnity.AStar
{
    public class AStar : MonoBehaviour
    {
        private GridNodes gridNodes;
        public AStarMapData mapData;
        public Tilemap obstacleMap;

        private int gridWidth;
        private int gridHeight;
        private int originX;
        private int originY;

        private Node startNdoe;
        private Node targetNdoe;

        private List<Node> openNodeList;
        private HashSet<Node> closeNodeList;

        public void Build(Vector2Int startNodePos, Vector2Int targetNodePos, Stack<AStarStep> aStarSteps)
        {
            if (GenerateGrid(startNodePos, targetNodePos))
            {
                FindShortPath();
                UpdatePathOnAStarStepPath(aStarSteps);
            }
        }

        /// <summary>
        /// 生成寻路网格
        /// </summary>
        /// <param name="startNodePos"> 起点坐标</param>
        /// <param name="targetNodePos"> 终点坐标</param>
        /// <returns></returns>
        private bool GenerateGrid(Vector2Int startNodePos, Vector2Int targetNodePos)
        {
            if (mapData == null) return false;

            Debug.Log("开始生成网格！");

            openNodeList = new List<Node>();
            closeNodeList = new HashSet<Node>();

            gridWidth = mapData.gridWidth;
            gridHeight = mapData.gridHeight;
            originX = mapData.originX;
            originY = mapData.originY;

            gridNodes = new GridNodes(gridWidth, gridHeight);

            startNdoe = gridNodes.GetGridNode(startNodePos.x - originX, startNodePos.y - originY);
            targetNdoe = gridNodes.GetGridNode(targetNodePos.x - originX, targetNodePos.y - originY);

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    Debug.Log("开始查找障碍物");
                    TileBase obstacle = obstacleMap.GetTile(new Vector3Int(x + originX, y + originY));

                    if (obstacle != null)
                    {
                        Node currentNode = gridNodes.GetGridNode(x, y);
                        currentNode.isObstacle = true;
                        Debug.Log("查找到障碍物!");
                    }
                }
            }

            Debug.Log("网格生成完毕！");
            return true;
        }

        private void FindShortPath()
        {
            openNodeList.Add(startNdoe);
            Node closeNode;

            while (openNodeList.Count > 0)
            {
                openNodeList.Sort();

                closeNode = openNodeList[0];
                openNodeList.RemoveAt(0);
                closeNodeList.Add(closeNode);

                if (closeNode == targetNdoe)
                {
                    break;
                }

                EvaluateNeighbourNodes(closeNode);
            }
        }

        private void EvaluateNeighbourNodes(Node currentNode)
        {
            Vector2Int currentNodePos = currentNode.nodePosition;
            Node validNeighbourNode;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;

                    validNeighbourNode = GetvalidNeighbourNode(currentNodePos.x + x, currentNodePos.y + y);

                    if (validNeighbourNode != null)
                    {
                        if (!openNodeList.Contains(validNeighbourNode))
                        {
                            validNeighbourNode.gCost = currentNode.gCost + GetNodeDistance(currentNode, validNeighbourNode);
                            validNeighbourNode.hCost = GetNodeDistance(validNeighbourNode, targetNdoe);
                            validNeighbourNode.nodeParent = currentNode;

                            openNodeList.Add(validNeighbourNode);
                        }
                    }
                }
            }
        }

        private Node GetvalidNeighbourNode(int x, int y)
        {
            if (x >= gridWidth || y >= gridHeight || x < 0 || y < 0) return null;

            Node neighbourNode = gridNodes.GetGridNode(x, y);

            if (neighbourNode.isObstacle || closeNodeList.Contains(neighbourNode)) return null;

            return neighbourNode;
        }

        private int GetNodeDistance(Node nodeA, Node nodeB)
        {
            int xDistacne = Mathf.Abs(nodeA.nodePosition.x - nodeB.nodePosition.x);
            int yDistacne = Mathf.Abs(nodeA.nodePosition.y - nodeB.nodePosition.y);

            if (xDistacne > yDistacne)
            {
                return 14 * yDistacne + 10 * (xDistacne - yDistacne);
            }
            return 14 * xDistacne + 10 * (yDistacne - xDistacne);
        }

        private void UpdatePathOnAStarStepPath(Stack<AStarStep> aStarSteps)
        {
            Node nextNode = targetNdoe;

            while (nextNode != null)
            {
                AStarStep newStep = new AStarStep();
                newStep.stepPos = new Vector2Int(nextNode.nodePosition.x + originX, nextNode.nodePosition.y + originY);

                Debug.Log(newStep.stepPos);

                aStarSteps.Push(newStep);
                nextNode = nextNode.nodeParent;
            }
            Debug.Log("路径成功生成！");
        }
    }
}