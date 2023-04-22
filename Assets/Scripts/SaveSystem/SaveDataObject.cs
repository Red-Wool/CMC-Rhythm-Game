using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveDataObject
{


    public Dictionary<LevelType, List<LevelScoreData>> scoreData;
}

[System.Serializable]
public class LevelType
{
    public string levelName;
    public float speed;

    public bool Equal(string l, float s)
    {
        return levelName == l && speed == s;
    }
}

[System.Serializable]
public class LevelScoreData
{
    public int score;
    public int[] hitCategories;

    public ScoreGrade grade;
    public bool beatDev;
}

[System.Serializable]
public enum ScoreGrade
{
    Perfect,
    S,
    A,
    B,
    C,
    D,
    F,
    Cang,
    Paused,
}