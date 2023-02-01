using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorEffectTriggerObject : MonoBehaviour
{
    public virtual EffectType effectType { get { return EffectType.General; } }

    public void SetUp(EffectStat data)
    {
        transform.position = new Vector3(data.xEditor, data.yTime);
    }

    public EffectStat GetStat()
    {
        return new EffectStat
        {
            xEditor = transform.position.x,
            yTime = transform.position.y,
            type = effectType
        };
    }
}

public struct EffectStat
{
    public float xEditor;
    public float yTime;
    public EffectType type;
}
