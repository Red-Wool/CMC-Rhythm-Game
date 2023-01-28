using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongEditorData : ScriptableObject
{
    public EffectTypeSprite[] sprites;

    //Strings to create Buttons
    public string[] moveObjectNames;
    public string[] arrowPathObjectNames;
    public string[] uiObjectNames;
}

[System.Serializable]
public struct EffectTypeSprite
{
    public EffectType type;
    public Sprite sprite;
}

public enum EffectType
{

}
