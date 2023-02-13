using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPathTriggerObject : EffectTriggerObject
{
    private ArrowPathModule arrowPathModule;

    public void SetData(ArrowPathModule m)
    {
        arrowPathModule = m;
        arrowPathModule.stats.easeType = stat.ease;
        m.RequestData();
    }

    protected override void Update()
    {
        //base.Update();
        if (stat.yTime <= GameManager.instance.GameTime)
        {
            //Debug.Log("effect!");
            TriggerEffect();
            gameObject.SetActive(false);

        }
    }

    public override void TriggerEffect()
    {
        //Debug.Log("Great!");
        GameManager.instance.bs.ActivateArrowPathEffect(stat, arrowPathModule);
    }
}
