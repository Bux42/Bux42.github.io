using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class MapBlockTMNF
    {
        public string Name;
        public int Direction;
        public Vector3 Coord;
        public int Flags;
        public GameObject Text3D;
    }
    [Serializable]
    public class ConvertedMapTMNF
    {
        public string MapId;
        public List<MapBlockTMNF> Blocks;
    }
}
