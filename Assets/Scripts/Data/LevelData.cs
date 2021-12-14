using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SongData/LevelData", fileName = "New Level", order = 3)]
public class LevelData : ScriptableObject
{
    public TextAsset levelData;
    public BackgroundData backgrounds;
    //Probably put stuff like loaded assets 
}
