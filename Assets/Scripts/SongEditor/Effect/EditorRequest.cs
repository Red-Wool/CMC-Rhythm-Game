using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorRequest
{
    public int fieldNum;
    public EditorRequestField[] requestFields;
}

public class EditorRequestField
{
    public string fieldName;
    public RequestType requestType;
}

public enum RequestType
{
    Float,
    Int,
    Vector3,
    Vector2,
    String
}
