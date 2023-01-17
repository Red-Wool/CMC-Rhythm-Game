using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate Vector3 NotePosition(float time, MoveModuleStat stat);

[System.Serializable]
public class MoveModule
{
    public bool isActive;

    public string notePosID;
    public MoveModuleStat stats;

 
    public NotePosition notePos;

    public void RequestData()
    {
        notePos = MoveFunctions.GetNotePosition(notePosID);
    }

    public Vector3 CalculateNotePosition(float time)
    {
        return (isActive) ? notePos(time, stats) : Vector3.zero;
    }
}

[System.Serializable]
public class MoveModuleStat
{
    public float speed;

    public float startTime;
    public float duration;

    public float[] store;
}

public static class MoveFunctions
{
    public static NotePosition GetNotePosition(string id)
    {
        NotePosition pos = null;

        switch (id)
        {
            case "MoveDirection":
                pos = MoveDirection;
                break;
            case "Circle3D":
                pos = Circle3D;
                break;
        }

        return pos;
    }

    public static EditorRequest MoveDirectionRequest(){return new EditorRequest();}
    public static Vector3 MoveDirection(float time, MoveModuleStat stat)
    {
        Debug.Log("Work");
        return Vector3.up * time * stat.speed;
    }

    public static Vector3 Circle3D(float time, MoveModuleStat stat)
    {
        return new Vector3(Mathf.Sin((time*5f+Time.time)*stat.speed)*time, Mathf.Cos((time*5f+Time.time)*stat.speed)*time, time); 
    }
}
