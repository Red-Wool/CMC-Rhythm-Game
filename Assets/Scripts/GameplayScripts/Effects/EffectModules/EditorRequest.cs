using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorRequest
{
    public EditorReqestField[] reqestFields;
}

public class EditorReqestField
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
