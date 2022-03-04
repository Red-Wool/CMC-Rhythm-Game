using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTriggerObject : MonoBehaviour
{
    [SerializeField] private EffectModule effectType;

    [SerializeField] private float yVal;

    public void SetupEffect(EffectModule effect, float bpm)
    {
        effectType = effect;
        yVal = effect.yVal / (bpm / 30);
    }

    // Update is called once per frame
    void Update()
    {
        if (yVal <= GameManager.instance.GameTime)
        {
            GameManager.instance.bs.ActivateEffect(effectType);
            //Debug.Log("effect!");

            gameObject.SetActive(false);
            
        }
    }
}
