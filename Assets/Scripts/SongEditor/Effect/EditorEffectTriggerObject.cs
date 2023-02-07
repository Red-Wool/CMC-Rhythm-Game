using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EditorEffectTriggerObject : MonoBehaviour
{
    public string objectEffect;
    public Ease easeType;

    public virtual EffectType effectType { get { return EffectType.General; } }

    public void SetUp(EffectStat data)
    {
        transform.position = new Vector3(data.xEditor, data.yTime);
        objectEffect = data.effectObj;
    }

    public EffectStat GetStat()
    {
        return new EffectStat
        {
            effectObj = objectEffect,
            ease = easeType,
            xEditor = transform.position.x,
            yTime = transform.position.y,
            type = effectType
        };
    }
}


