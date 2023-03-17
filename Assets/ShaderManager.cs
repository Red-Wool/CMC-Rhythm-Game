using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Camera))]
public class ShaderManager : MonoBehaviour
{
    public Material baseMaterial;
    public bool isActive;

    public void ChangeShader(ShaderDataObject data, ShaderModule module)
    {
        
        if (module.option != ShaderOption.Change)
        {
            //Debug.Log(module.option);
            isActive = module.option == ShaderOption.On;
        }
        else if (baseMaterial.name != data.material.name)
        {

            baseMaterial = data.material;
            for (int i = 0; i < module.store.Length; i++)
            {
                baseMaterial.SetFloat(data.request.requestFields[i].fieldName, module.store[i]);
                    //int j = i;
                    //float begin = baseMaterial.GetFloat(data.request.requestFields[j].fieldName);//Shader.PropertyToID(data.request.requestFields[i].fieldName);
                    //DOTween.To(() => begin, x => begin = x, module.store[j], module.duration).SetEase(module.ease).OnUpdate(() => ));
            }
            
        }
        else
        {
            float duration = module.duration / (GameManager.instance.bs.bpm / 30f);
            for (int i = 0; i < module.store.Length; i++)
            {
                int j = i;
                float begin = baseMaterial.GetFloat(data.request.requestFields[j].fieldName);//Shader.PropertyToID(data.request.requestFields[i].fieldName);
                DOTween.To(() => begin, x => begin = x, module.store[j], duration).SetEase(module.ease).OnUpdate(() => baseMaterial.SetFloat(data.request.requestFields[j].fieldName, begin));
            }
        }
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (isActive)
        {
            Graphics.Blit(src, dst, baseMaterial);
        }
        else
        {
            Graphics.Blit(src, dst);
        }
        
    }
}