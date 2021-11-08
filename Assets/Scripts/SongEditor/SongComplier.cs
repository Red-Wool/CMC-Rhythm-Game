using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

public class SongComplier : MonoBehaviour
{
    public string name;
    public GameObject noteHolder;

    private string data;

    private EditorNoteObject obj;

    public void SaveSong(string name, GameObject notes)
    {
        
        //Get the file Location
        string path = Application.dataPath + "/SongData/" + name + ".txt";

        data = name + "\nLast Edited: " + DateTime.Now + "\nVersion\nV6";
        for (int i = 0; i < notes.transform.childCount; i++)
        {
            obj = notes.transform.GetChild(i).GetComponent<EditorNoteObject>();



            data += "\n" + JsonUtility.ToJson(obj.GetNoteData());

            //data += "\n" + obj.GetNoteColor() + " " + notes.transform.GetChild(i).transform.position.y + " " + obj.GetIfLongNote() + " " + obj.GetLength();
        }
        data += "\nEnd";

        //if (!File.Exists(path))
        //{ 
        Debug.Log("Scaldedel");
            File.WriteAllText(path, data);
        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        //SaveSong(name, noteHolder);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Test()
    {
        Debug.Log("dad");
        SaveSong(name, noteHolder);
    }
}

[System.Serializable]
public struct Note
{
    public NoteColor color;
    public float yVal;
    public bool isLongNote;
    public float longNoteLen;
}
