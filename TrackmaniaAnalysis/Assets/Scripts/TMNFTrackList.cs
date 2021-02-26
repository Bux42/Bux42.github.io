using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Track
{
    public int TrackId;
    public string TrackName;
    public string TrackAuthor;
    public string WRTime;
    public string WRAuthor;
    public List<string> ReplayFilesPath = new List<string>();
}
[Serializable]
public class TMNFTrackList
{
    public List<Track> Tracks;
}
