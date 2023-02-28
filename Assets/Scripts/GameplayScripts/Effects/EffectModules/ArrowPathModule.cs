using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public delegate Vector3 NotePosition(float time, ArrowPathModuleStat stat);

[System.Serializable]
public class ArrowPathModule
{
    public bool isActive;

    public string notePosID;
    public int objID;
    public ArrowPathModuleStat stats;

    
 
    public NotePosition notePos;

    public ArrowPathModule(string posID, int oID, ArrowPathModuleStat stat)
    {
        isActive = true;
        notePosID = posID;
        objID = oID;
        stats = stat;
        notePos = ArrowPathFunctions.GetNotePosition(notePosID);
    }

    public ArrowPathModule(string json)
    {
        ArrowPathModule a = JsonUtility.FromJson<ArrowPathModule>(json);

        isActive = true;
        notePosID = a.notePosID;
        objID = a.objID;
        stats = a.stats;
        notePos = ArrowPathFunctions.GetNotePosition(notePosID);
    }

    public string GetJSON()
    {
        //ArrowPathModule arrowPath = new ArrowPathModule(notePosID, objID, stats.speed, stats.startTime, stats.duration, stats.easeType, stats.store);
        return JsonUtility.ToJson(this); //arrowPath;
    }

    public void RequestData()
    {
        notePos = ArrowPathFunctions.GetNotePosition(notePosID);
    }

    public Vector3 CalculateNotePosition(float time)
    {
        if (notePos == null)
        {
            RequestData();
            //notePos = ArrowPathFunctions.GetNotePosition(notePosID);
            Debug.Log("Unset Arrow Path");
        }
        return isActive ? 


            notePos(time, stats) : Vector3.zero;
    }

}

[System.Serializable]
public class ArrowPathModuleStat
{
    public float speed;

    public float startTime;
    public float duration;
    public Ease easeType;

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
                return Circle3DRequest();
            case "Oscillate":
                return OscillateRequest();
            case "ZigZag":
                return OscillateRequest();
            case "SquareWave":
                return OscillateRequest();
        }

        Debug.Log("Invalid Move Function Editor Request: " + id);
        return null;
    }

    public static NotePosition GetNotePosition(string id)
    {
        //Debug.Log(id);
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

    public static EditorRequest MoveDirectionRequest(){return new EditorRequest() {fieldNum = 3, requestFields = new EditorRequestField[] { new EditorRequestField() { fieldName = "Direction", requestType = RequestType.Vector3 } } };}
    public static Vector3 MoveDirection(float time, ArrowPathModuleStat stat)
    {
        return new Vector3(stat.store[0],stat.store[1],stat.store[2]) * time * stat.speed;
    }

    public static EditorRequest Circle3DRequest()
    {
        return new EditorRequest()
        {
            fieldNum = 7,
            requestFields = new EditorRequestField[]{
            new EditorRequestField() {fieldName = "Strength", requestType = RequestType.Vector3}, //0-2 X Y Z
            new EditorRequestField() {fieldName = "Angle Rate", requestType = RequestType.Vector2}, //3-4 X Y 
            new EditorRequestField() {fieldName = "Angle Time Rate", requestType = RequestType.Vector2}, //5-6 X Y
            }
        };
    }
    public static Vector3 Circle3D(float time, ArrowPathModuleStat stat)
    {
        time *= stat.speed;
        float gameTime = GameManager.instance.GameTime - stat.startTime;
        return new Vector3(
            Mathf.Sin(time*stat.store[3]+gameTime * stat.store[5]) *time*stat.store[0],
            Mathf.Cos(time*stat.store[4]+gameTime * stat.store[6]) *time*stat.store[1],
            time * stat.store[2]); 
    }

    public static EditorRequest OscillateRequest() {
        return new EditorRequest()
        {
            fieldNum = 9,
            requestFields = new EditorRequestField[]{ 
            new EditorRequestField() {fieldName = "Strength", requestType = RequestType.Vector3}, //0-2 X Y Z
            new EditorRequestField() {fieldName = "Position Rate", requestType = RequestType.Vector3}, //3-5 X Y Z
            new EditorRequestField() {fieldName = "Time Rate", requestType = RequestType.Vector3}, //6-8 X Y Z
            } };
    }
    public static Vector3 Oscillate(float time, ArrowPathModuleStat stat)
    {
        time *= stat.speed;
        float gameTime = GameManager.instance.GameTime - stat.startTime;
        return new Vector3(
            stat.store[0] * Mathf.Sin(stat.store[3] * time + stat.store[6] * gameTime),
            stat.store[1] * Mathf.Sin(stat.store[4] * time + stat.store[7] * gameTime),
            stat.store[2] * Mathf.Sin(stat.store[5] * time + stat.store[8] * gameTime));
    }


    public static Vector3 ZigZag(float time, ArrowPathModuleStat stat)
    {
        time *= stat.speed;
        float gameTime = GameManager.instance.GameTime - stat.startTime;
        return new Vector3(
            stat.store[0] * Mathf.Asin(Mathf.Sin(stat.store[3] * time + stat.store[6] * gameTime)),
            stat.store[1] * Mathf.Asin(Mathf.Sin(stat.store[4] * time + stat.store[7] * gameTime)),
            stat.store[2] * Mathf.Asin(Mathf.Sin(stat.store[5] * time + stat.store[8] * gameTime))) * 0.63662f;
    }
    
    public static Vector3 SquareWave(float time, ArrowPathModuleStat stat)
    {
        time *= stat.speed;
        float gameTime = GameManager.instance.GameTime - stat.startTime;
        return new Vector3(
            stat.store[0] * Mathf.Round(Mathf.Asin(Mathf.Sin(stat.store[3] * time + stat.store[6] * gameTime))),
            stat.store[1] * Mathf.Round(Mathf.Asin(Mathf.Sin(stat.store[4] * time + stat.store[7] * gameTime))),
            stat.store[2] * Mathf.Round(Mathf.Asin(Mathf.Sin(stat.store[5] * time + stat.store[8] * gameTime)))) * 0.63662f;
    }
}
