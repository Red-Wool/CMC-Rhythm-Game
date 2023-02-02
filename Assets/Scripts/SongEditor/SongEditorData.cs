using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SongEditorData : ScriptableObject
{
    public EffectTypeSprite[] effectTypeSprites;

    public string[] arrowPathOptions;

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
    General,
    Move,
    ArrowPath,
    UI,
    Shader
}
