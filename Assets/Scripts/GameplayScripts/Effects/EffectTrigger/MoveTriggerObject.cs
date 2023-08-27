using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTriggerObject : EffectTriggerObject
{

    private MoveModule moveModule;

    public void SetData(MoveModule m)
    {
        moveModule = m;
        moveModule.easeType = stat.ease;
    }

    protected override void Update()
    {
        //base.Update();
        if (stat.yTime <= GameManager.instance.GameTime)
        {
            //Debug.Log("effect! " + moveModule.effectType);
            TriggerEffect();
            gameObject.SetActive(false);

        }
    }

    public override void TriggerEffect()
    {
        GameManager.instance.bs.ActivateMoveEffect(stat, moveModule);
    }
}
