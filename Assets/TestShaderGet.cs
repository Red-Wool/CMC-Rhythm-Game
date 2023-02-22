using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShaderGet : MonoBehaviour
{
    public string shaderName;
    public ShaderDataObject shaderData;
    public bool update;

    private ShaderApply apply;
    // Start is called before the first frame update
    void Start()
    {
        apply = GetComponent<ShaderApply>();

        shaderData = LoadAssetBundle.GetShaderObject(shaderName);
        Debug.Log(shaderData.shaderName);
        
        apply.baseMaterial = shaderData.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (update)
        {
            update = false;
            shaderData = LoadAssetBundle.GetShaderObject(shaderName);
            Debug.Log(shaderData.shaderName);

            apply.baseMaterial = shaderData.material;
        }
    }
}
