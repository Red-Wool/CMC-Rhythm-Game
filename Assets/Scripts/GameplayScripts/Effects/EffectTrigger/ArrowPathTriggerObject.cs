using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPathTriggerObject : EffectTriggerObject
{
    private ArrowPathModule arrowPathModule;

    public void SetData(ArrowPathModule m)
    {
        arrowPathModule = new ArrowPathModule(m.notePosID, m.objID, m.stats);
        arrowPathModule.stats.duration /= GameManager.instance.bs.bpm / 60f;
        arrowPathModule.stats.easeType = stat.ease;
        arrowPathModule.RequestData();
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
