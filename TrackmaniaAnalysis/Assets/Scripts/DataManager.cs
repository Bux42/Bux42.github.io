using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    public static List<string> PreviousScenes = new List<string>();
    public static ConvertedMapTMNF TMNFMap = null;
    internal static List<TMNFSampleData> TMNFMapGhosts = new List<TMNFSampleData>();

    public static int KeyboardLayout = 0;
    public static string[] MoveKeys;

    public static string[] MoveKeysAzerty = new string[] { "z", "s", "q", "d" };
    public static string[] MoveKeysQwerty = new string[] { "w", "s", "a", "d" };

    public static void KeyboardLayoutChanged(int index)
    {
        PlayerPrefs.SetInt("KeyboardLayout", index);
        if (index == 0 || index == 1)
            MoveKeys = MoveKeysQwerty;
        if (index == 2)
            MoveKeys = MoveKeysAzerty;
    }
}
