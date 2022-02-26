using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ShaderApply : MonoBehaviour
{
    public Material baseMaterial;
    public bool isActive;

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
