using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LearnUnity.AStar
{
    public class TestAStar : MonoBehaviour
    {
        public Tilemap pathMap;
        public TileBase pathTile;

        public Vector2Int startPos;
        public Vector2Int targetPos;

        private AStar aStar;

        public bool displayStartAndTarget;
        public bool displayPath;
        private Stack<AStarStep> steps;

        private void Awake()
        {
            aStar = GetComponent<AStar>();
            steps = new Stack<AStarStep>();
        }

        private void Update()
        {
            ShowPathOnGridMap();
        }

        private void ShowPathOnGridMap()
        {
            if (displayStartAndTarget)
            {
                pathMap.SetTile((Vector3Int)startPos, pathTile);
                pathMap.SetTile((Vector3Int)targetPos, pathTile);
            }
            else
            {
                pathMap.SetTile((Vector3Int)startPos, null);
                pathMap.SetTile((Vector3Int)targetPos, null);
            }

            if (displayPath)
            {
                aStar.Build(startPos, targetPos, steps);

                foreach (var step in steps)
                {
                    pathMap.SetTile((Vector3Int)step.stepPos, pathTile);
                }
            }
            else
            {
                foreach (var step in steps)
                {
                    pathMap.SetTile((Vector3Int)step.stepPos, null);
                }
                steps.Clear();
            }
        }
    }
}