using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorEffectTriggerObject : MonoBehaviour
{
    public EffectStat effectInfo;

    public EffectType type { get; protected set; }

    public void SetUp(EffectStat data)
    {
        effectInfo = data;
        transform.position = new Vector3(data.editorPos, data.yVal);
    }

    /*public EffectModule GetData()
    {
        effectInfo.yVal = transform.position.y;
        effectInfo.editorPos = (int)transform.position.x;
        return effectInfo;
    }*/
}

public struct EffectStat
{
    public float editorPos;
    public float yVal;
}
