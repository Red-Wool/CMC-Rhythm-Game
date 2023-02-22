using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public static ControlManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    private ControlData data;
    public ControlData GetData { get { return data;} private set { data = value; } }
}
