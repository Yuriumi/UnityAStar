using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LearnUnity.AStar
{
    public class UseAStar : MonoBehaviour
    {
        private AStar aStar;

        private Vector2Int startNodePos;
        private Vector2Int targetNodePos;

        public AStarMapData mapData;
        public Tilemap pathMap;
        public Tilemap obsatcleMap;
        public TileBase pathTile;
        public TileBase obstacleTile;

        private Vector3Int mouseCellPos;

        private Stack<AStarStep> aStarSteps;

        private void Awake()
        {
            aStar = GetComponent<AStar>();
            aStarSteps = new Stack<AStarStep>();
        }

        private void Update()
        {
            DrawStartAndTarget();
            DrawObsatcle();
            BuildPath();
            ClearTile();
        }

        private void DrawStartAndTarget()
        {
            if (Input.GetMouseButtonDown(0) && startNodePos == Vector2Int.zero)
            {
                MouseDrawPathTile(pathMap, pathTile);

                startNodePos = (Vector2Int)mouseCellPos;

                Debug.Log("起点已设置！坐标为：" + startNodePos);
            }
            else if (Input.GetMouseButtonDown(0) && startNodePos != Vector2Int.zero && targetNodePos == Vector2Int.zero)
            {
                MouseDrawPathTile(pathMap, pathTile);

                targetNodePos = (Vector2Int)mouseCellPos;

                Debug.Log("终点已设置！！坐标为：" + targetNodePos);
                Debug.Log("起点与终点设置完毕，按下空格开始构建路径!");
            }
        }

        private void MouseDrawPathTile(Tilemap drawMap, TileBase drawTile)
        {
            mouseCellPos = drawMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            TileBase currentCell = drawMap.GetTile(mouseCellPos);

            if (currentCell != null) return;

            drawMap.SetTile(mouseCellPos, drawTile);
        }

        private void DrawObsatcle()
        {
            if (Input.GetMouseButton(1))
            {
                MouseDrawPathTile(obsatcleMap, obstacleTile);
            }
        }

        private void BuildPath()
        {
            if (startNodePos != Vector2Int.zero && targetNodePos != null && Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("开始构建路径!");
                aStar.Build(startNodePos, targetNodePos, aStarSteps);
                DrawPath();
                Debug.Log("路径构建完成!");
            }
        }

        private void DrawPath()
        {
            foreach (AStarStep step in aStarSteps)
            {
                Debug.Log("路径绘制中...");
                pathMap.SetTile((Vector3Int)step.stepPos, pathTile);
            }
        }

        private void ClearTile()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                startNodePos = Vector2Int.zero;
                targetNodePos = Vector2Int.zero;

                for (int x = mapData.originX; x < mapData.gridWidth; x++)
                {
                    for (int y = mapData.originY; y < mapData.gridHeight; y++)
                    {
                        pathMap.SetTile(new Vector3Int(x, y), null);
                        obsatcleMap.SetTile(new Vector3Int(x, y), null);
                        aStarSteps.Clear();
                        Debug.Log("地图已清空，请重新绘制！");
                    }
                }
            }
        }
    }
}