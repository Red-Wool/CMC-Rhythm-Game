﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTriggerObject : MonoBehaviour
{
    private EffectModule effectType;

    private float yVal;

    public void SetupEffect(EffectModule effect, float val)
    {
        effectType = effect;
        yVal = val;
    }

    // Update is called once per frame
    void Update()
    {
        if (yVal <= GameManager.instance.GameTime)
        {
            GameManager.instance.bs.ActivateEffect(effectType);
            gameObject.SetActive(false);
        }
    }
}
