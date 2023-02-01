using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorArrowPathEffect : EditorEffectTriggerObject
{
    public ArrowPathModule arrowPath;

    public override EffectType effectType { get { return EffectType.ArrowPath; } }
}
