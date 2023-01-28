using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate Vector3 NotePosition(float time, ArrowPathModuleStat stat);

[System.Serializable]
public class ArrowPathModule
{
    public bool isActive;

    public string notePosID;
    public int objID;
    public ArrowPathModuleStat stats;

 
    public NotePosition notePos;

    public void RequestData()
    {
        notePos = ArrowPathFunctions.GetNotePosition(notePosID);
    }

    public Vector3 CalculateNotePosition(float time)
    {
        return (isActive) ? notePos(time, stats) : Vector3.zero;
    }
}

[System.Serializable]
public class ArrowPathModuleStat
{
    public float speed;

    public float startTime;
    public float duration;

    public float[] store;
}

public static class ArrowPathFunctions
{
    public static EditorRequest RequestEditorData(string id)
    {
        switch (id)
        {
            case "MoveDirection":
                return MoveDirectionRequest();
            case "Circle3D":
                return MoveDirectionRequest();
            case "Oscillate":
                return OscillateRequest();
            case "ZigZag":
                return MoveDirectionRequest();
            case "SquareWave":
                return MoveDirectionRequest();
        }

        Debug.Log("Invalid Move Function Editor Request: " + id);
        return null;
    }

    public static NotePosition GetNotePosition(string id)
    {
        Debug.Log(id);
        NotePosition pos = null;

        switch (id)
        {
            case "MoveDirection":
                pos = MoveDirection;
                break;
            case "Circle3D":
                pos = Circle3D;
                break;
            case "Oscillate":
                pos = Oscillate;
                break;
            case "ZigZag":
                pos = ZigZag;
                break;
            case "SquareWave":
                pos = SquareWave;
                break;
        }

        return pos;
    }

    public static EditorRequest MoveDirectionRequest(){return new EditorRequest();}
    public static Vector3 MoveDirection(float time, ArrowPathModuleStat stat)
    {
        return Vector3.up * time * stat.speed;
    }

    public static EditorRequest Circle3DRequest()
    {
        return new EditorRequest()
        {
            requestFields = new EditorReqestField[]{
            new EditorReqestField() {fieldName = "Strength", requestType = RequestType.Vector3}, //0-2 X Y Z
            new EditorReqestField() {fieldName = "Angle Rate", requestType = RequestType.Vector2}, //3-4 X Y 
            new EditorReqestField() {fieldName = "Angle Time Rate", requestType = RequestType.Vector2}, //5-6 X Y
            }
        };
    }
    public static Vector3 Circle3D(float time, ArrowPathModuleStat stat)
    {
        time *= stat.speed;
        return new Vector3(
            Mathf.Sin(time*stat.store[3]+Time.time * stat.store[5]) *time*stat.store[0],
            Mathf.Cos(time*stat.store[4]+Time.time * stat.store[6]) *time*stat.store[1],
            time * stat.store[2]); 
    }

    public static EditorRequest OscillateRequest() {
        return new EditorRequest()
        { requestFields = new EditorReqestField[]{ 
            new EditorReqestField() {fieldName = "Strength", requestType = RequestType.Vector3}, //0-2 X Y Z
            new EditorReqestField() {fieldName = "Position Rate", requestType = RequestType.Vector3}, //3-5 X Y Z
            new EditorReqestField() {fieldName = "Time Rate", requestType = RequestType.Vector3}, //6-8 X Y Z
            } };
    }
    public static Vector3 Oscillate(float time, ArrowPathModuleStat stat)
    {
        time *= stat.speed;
        return new Vector3(
            stat.store[0] * Mathf.Sin(stat.store[3] * time + stat.store[6] * Time.time),
            stat.store[1] * Mathf.Sin(stat.store[4] * time + stat.store[7] * Time.time),
            stat.store[2] * Mathf.Sin(stat.store[5] * time + stat.store[8] * Time.time));
    }


    public static Vector3 ZigZag(float time, ArrowPathModuleStat stat)
    {
        return new Vector3(Mathf.Asin(Mathf.Sin(time * stat.speed + Time.time)), 0, 0) * 0.63662f;
    }
    
    public static Vector3 SquareWave(float time, ArrowPathModuleStat stat)
    {
        time *= stat.speed;
        return new Vector3(Mathf.Round(Mathf.Asin(Mathf.Sin(time + Time.time)) * 0.63662f), 0, 0);
    }
}
