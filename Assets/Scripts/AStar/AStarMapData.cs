using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LearnUnity.AStar
{
    [CreateAssetMenu(menuName = "AStar/MapData", fileName = "MapData")]
    public class AStarMapData : ScriptableObject
    {
        [Header("������Ϣ")]
        public int gridWidth;
        public int gridHeight;
        public int originX;
        public int originY;
    }
}