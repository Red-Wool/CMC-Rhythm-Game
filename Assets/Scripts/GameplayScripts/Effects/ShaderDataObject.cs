using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ShaderDataObject : ScriptableObject
{
    public string shaderName;
    public Material material;
    public EditorRequest request;
}
