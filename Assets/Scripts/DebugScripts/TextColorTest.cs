using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextColor : MonoBehaviour
{
    public string text;

    public TextMeshProUGUI textMesh;
    // Start is called before the first frame update
    void Start()
    {
        //textMesh.GetComponent<TextMeshProUGUI>();

        textMesh.richText = true;
        textMesh.text = text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
