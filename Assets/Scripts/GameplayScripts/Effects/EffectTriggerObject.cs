using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTriggerObject : MonoBehaviour
{
    [SerializeField] private EffectType effectType;

    [SerializeField] private EffectStat stat;

    public void SetupEffect(EffectStat effectStat, float bpm)
    {
        //BIG NEED FOR FIX UP
        //effectType = effect;
        //yVal = effect.yVal / (bpm / 30);
    }

    // Update is called once per frame
    void Update()
    {
        if (stat.yVal <= GameManager.instance.GameTime)
        {
            //ALSO NEEDS FIX
            //GameManager.instance.bs.ActivateEffect(effectType);
            //Debug.Log("effect!");

            gameObject.SetActive(false);
            
        }
    }
}
