using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTriggerObject : MonoBehaviour
{
    [SerializeField] protected EffectType effectType;

    [SerializeField] protected EffectStat stat;

    public void SetupEffect(EffectStat effectStat, float bpm)
    {
        stat = effectStat;
        //BIG NEED FOR FIX UP
        //effectType = effect;
        //yVal = effect.yVal / (bpm / 30);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (stat.yTime <= GameManager.instance.GameTime)
        {
            Debug.Log("effect!");
            TriggerEffect();
            gameObject.SetActive(false);
            
        }
    }

    public virtual void TriggerEffect()
    {

    }
}

[System.Serializable]
public struct EffectStat
{
    public string effectObj;
    public float xEditor;
    public float yTime;
    public EffectType type;
}
