using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShaderModule
{
    public string shaderDataName;
    public float[] store;

    public float speed;
    public float duration;

    public DG.Tweening.Ease ease;
    public ShaderOption option;
}

public enum ShaderOption
{
    Change,
    On,
    Off
}