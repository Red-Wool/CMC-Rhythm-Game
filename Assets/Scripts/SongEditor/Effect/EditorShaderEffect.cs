using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorShaderEffect : EditorEffectTriggerObject
{
    public ShaderModule shader;

    public override EffectType effectType { get { return EffectType.Shader; } }
}
