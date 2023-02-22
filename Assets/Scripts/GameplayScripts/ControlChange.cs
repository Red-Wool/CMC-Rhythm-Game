using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class ControlChange : MonoBehaviour
{
    public ControlData data;

    public TMP_Text left;
    public TMP_Text up;
    public TMP_Text down;
    public TMP_Text right;

    public GameObject panel;
    private bool changing;
    private int val;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        left.text = data.left.ToString();
        up.text = data.up.ToString();
        down.text = data.down.ToString();
        right.text = data.right.ToString();
        if (changing && Input.anyKeyDown)
        {
            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(kcode))
                {
                    changing = false;
                    panel.SetActive(false);
                    switch (val)
                    {
                        case 0:
                            data.left = kcode;
                            break;
                        case 1:
                            data.up = kcode;
                            break;
                        case 2:
                            data.down = kcode;
                            break;
                        case 3:
                            data.right = kcode;
                            break;
                    }
                }
                    
            }

        }
    }

    public void Replace(int value)
    {
        val = value;
        changing = true;
        panel.SetActive(true);
    }
}
