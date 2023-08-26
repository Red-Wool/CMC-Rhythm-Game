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

    public void UpdateData()
    {
        ArrowPathFunctions.UpdateModule(notePosID, stats);
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
            case "PopIn":
                return PopInRequest();
            case "QuadraticBezierCurve":
                return QuadraticBezierCurveRequest();
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
            case "PopIn":
                pos = PopIn;
                break;
            case "QuadraticBezierCurve":
                pos = QuadraticBezierCurve;
                break;
        }

        return pos;
    }

    public static void UpdateModule(string id, ArrowPathModuleStat stat)
    {
        switch (id)
        {
            case "PopIn":
                PopInUpdate(stat);
                break;
            case "QuadraticBezierCurve":
                QuadraticBezierCurveUpdate(stat);
                break;

        }
    }

    #region Move Direction
    public static EditorRequest MoveDirectionRequest(){return new EditorRequest() {fieldNum = 3, requestFields = new EditorRequestField[] { new EditorRequestField() { fieldName = "Direction", requestType = RequestType.Vector3 } } };}
    public static Vector3 MoveDirection(float time, ArrowPathModuleStat stat)
    {
        return new Vector3(stat.store[0],stat.store[1],stat.store[2]) * time * stat.speed;
    }
    #endregion

    #region Circle 3D

    public static EditorRequest Circle3DRequest()
    {
        return new EditorRequest()
        {
            fieldNum = 9,
            requestFields = new EditorRequestField[]{
            new EditorRequestField() {fieldName = "Strength", requestType = RequestType.Vector3}, //0-2 X Y Z
            new EditorRequestField() {fieldName = "Angle Rate", requestType = RequestType.Vector2}, //3-4 X Y 
            new EditorRequestField() {fieldName = "Angle Time Rate", requestType = RequestType.Vector2}, //5-6 X Y
            new EditorRequestField() {fieldName = "Angle Displace", requestType = RequestType.Vector2} //7-8 X Y
            }
        };
    }
    public static Vector3 Circle3D(float time, ArrowPathModuleStat stat)
    {
        time *= stat.speed;
        float gameTime = GameManager.instance.GameTime - stat.startTime;
        return new Vector3(
            Mathf.Sin(time*stat.store[3]+gameTime * stat.store[5] + stat.store[7] * Mathf.Deg2Rad) *time*stat.store[0],
            Mathf.Cos(time*stat.store[4]+gameTime * stat.store[6] + stat.store[8] * Mathf.Deg2Rad) *time*stat.store[1],
            time * stat.store[2]); 
    }

    #endregion

    #region Oscillate

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

    #endregion

    #region Zig Zag
    public static Vector3 ZigZag(float time, ArrowPathModuleStat stat)
    {
        time *= stat.speed;
        float gameTime = GameManager.instance.GameTime - stat.startTime;
        return new Vector3(
            stat.store[0] * Mathf.Asin(Mathf.Sin(stat.store[3] * time + stat.store[6] * gameTime)),
            stat.store[1] * Mathf.Asin(Mathf.Sin(stat.store[4] * time + stat.store[7] * gameTime)),
            stat.store[2] * Mathf.Asin(Mathf.Sin(stat.store[5] * time + stat.store[8] * gameTime))) * 0.63662f;
    }

    #endregion

    #region Square Wave

    public static Vector3 SquareWave(float time, ArrowPathModuleStat stat)
    {
        time *= stat.speed;
        float gameTime = GameManager.instance.GameTime - stat.startTime;
        return new Vector3(
            stat.store[0] * Mathf.Round(Mathf.Asin(Mathf.Sin(stat.store[3] * time + stat.store[6] * gameTime))),
            stat.store[1] * Mathf.Round(Mathf.Asin(Mathf.Sin(stat.store[4] * time + stat.store[7] * gameTime))),
            stat.store[2] * Mathf.Round(Mathf.Asin(Mathf.Sin(stat.store[5] * time + stat.store[8] * gameTime)))) * 0.63662f;
    }

    #endregion

    #region Pop In

    public static EditorRequest PopInRequest()
    {
        return new EditorRequest()
        {
            fieldNum = 5,
            requestFields = new EditorRequestField[]{
            new EditorRequestField() {fieldName = "Direction", requestType = RequestType.Vector3}, //0-2 X Y Z
            new EditorRequestField() {fieldName = "Pop Time", requestType = RequestType.Float}, //3 X 
            new EditorRequestField() {fieldName = "Pop Strength", requestType = RequestType.Float}, //4 X
            }
        };
    }

    public static void PopInUpdate(ArrowPathModuleStat stat)
    {
        stat.store[3] /= GameManager.instance.bs.bpm / 30f;
    }

    public static Vector3 PopIn(float time, ArrowPathModuleStat stat)
    {
        time *= stat.speed;
        float gameTime = Mathf.Clamp((time - stat.store[3]) * stat.store[4],0,1);
        gameTime = 3 * Mathf.Pow(gameTime, 2) - 2 * Mathf.Pow(gameTime, 3);
        
        return new Vector3(stat.store[0], stat.store[1], stat.store[2]) * gameTime;
    }

    #endregion

    #region Quadratic Bezier Curve

    public static EditorRequest QuadraticBezierCurveRequest()
    {
        return new EditorRequest()
        {
            fieldNum = 12,
            requestFields = new EditorRequestField[]{
            new EditorRequestField() {fieldName = "Point 1", requestType = RequestType.Vector3}, //0-2 X Y Z
            new EditorRequestField() {fieldName = "Point 2", requestType = RequestType.Vector3}, //3-5 X Y Z
            new EditorRequestField() {fieldName = "Point 3", requestType = RequestType.Vector3}, //6-8 X Y Z
            new EditorRequestField() {fieldName = "Start Time", requestType = RequestType.Float}, //9 X 
            new EditorRequestField() {fieldName = "Duration", requestType = RequestType.Float}, //10 X
            }
        };
    }

    public static void QuadraticBezierCurveUpdate(ArrowPathModuleStat stat)
    {
        float bpm = GameManager.instance.bs.bpm;

        stat.store[9] /= bpm / 30f;
        stat.store[10] /= bpm / 30f;
        stat.store[11] = stat.startTime + stat.store[9];
    }

    public static Vector3 QuadraticBezierCurve(float time, ArrowPathModuleStat stat)
    {
        float t = GameManager.instance.GameTime;

        if (t > stat.store[11] + stat.store[10])
            return Vector3.zero;

        time *= stat.speed;
        time += t;
        time = Mathf.Clamp((time - stat.store[11]) / stat.store[10], 0, 1);

        float subTime = Mathf.Clamp((t - stat.store[11]) / stat.store[10], 0, 1);

        Vector3 a = new Vector3(stat.store[0], stat.store[1], stat.store[2]);
        Vector3 b = new Vector3(stat.store[3], stat.store[4], stat.store[5]);
        Vector3 c = new Vector3(stat.store[6], stat.store[7], stat.store[8]);

        Vector3 ab = Vector3.Lerp(a, b, time);
        Vector3 bc = Vector3.Lerp(b, c, time);

        Vector3 sab = Vector3.Lerp(a, b, subTime);
        Vector3 sbc = Vector3.Lerp(b, c, subTime);

        return Vector3.Lerp(ab,bc, time) - Vector3.Lerp(sab, sbc, subTime);
    }

    #endregion
}
