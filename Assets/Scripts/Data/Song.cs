using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SongData/Song", fileName = "New Song", order = 2)]
public class Song : ScriptableObject
{
    public string songName;
    public string artist;
    public string description;
    public Texture songArt;
    public Texture songFrameArt;
    public LevelData levelData;
}
