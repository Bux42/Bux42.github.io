using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class ControlEntry
    {
        public int Time;
        public byte Index;
        public bool Enabled;
    }
    [Serializable]
    public class Sample2
    {
        public string Timestamp;
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 PitchYawRoll;
        public float Speed;
        public Vector3 Velocity;
    }
    [Serializable]
    class TMNFSampleData
    {
        public string PlayerName;
        public string RaceTime;
        public float MaxSpeed;
        public List<string> ControlNames = new List<string>();
        public List<ControlEntry> ControlEntries = new List<ControlEntry>();
        public List<Sample2> Samples = new List<Sample2>();
        public LineRenderer LineRenderer;
        public Color Color;
    }
}
