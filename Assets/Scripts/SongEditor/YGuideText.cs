using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class YGuideText : MonoBehaviour
{
    private TextMeshProUGUI textArea;

    // Start is called before the first frame update
    void Start()
    {
        textArea = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        textArea.text = transform.position.y.ToString();
    }
}
