using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorMoveEffect : EditorEffectTriggerObject
{
    public MoveModule move;

    public override EffectType effectType { get { return EffectType.Move; } }
}
