using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorEffectTriggerObject : MonoBehaviour
{
    public EffectModule effectInfo;

    public void SetUp(EffectModule data)
    {
        effectInfo = data;
        transform.position = new Vector3(data.xSpot, data.yVal);
    }

    public EffectModule GetData()
    {
        effectInfo.yVal = transform.position.y;
        effectInfo.xSpot = (int)transform.position.x;
        return effectInfo;
    }
}
