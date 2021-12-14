using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SongData/Album", fileName = "New Album",order = 1)]
public class Album : ScriptableObject
{
    public string albumName;
    public Texture coverArt;
    public Song[] songList;
}
