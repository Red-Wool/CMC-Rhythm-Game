using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderTriggerObject : EffectTriggerObject
{
    private ShaderModule shaderModule;
    private ShaderDataObject shaderData;


    public void SetData(ShaderModule module)
    {
        shaderModule = module;
        shaderModule.ease = stat.ease;
        if (module.option == ShaderOption.Change)
        {
            shaderData = LoadAssetBundle.GetShaderObject(shaderModule.shaderDataName);
        }
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (stat.yTime <= GameManager.instance.GameTime)
        {
            //Debug.Log("effect!");
            TriggerEffect();
            gameObject.SetActive(false);

        }
    }

    public override void TriggerEffect()
    {
        GameManager.instance.sm.ChangeShader(shaderData, shaderModule);
    }
}
