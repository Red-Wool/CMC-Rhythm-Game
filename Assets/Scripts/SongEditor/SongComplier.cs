using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using TMPro;

public class SongComplier : MonoBehaviour
{
    //public string songName;

    [SerializeField] private TMP_InputField songNameInput; public string SongName { get { return songNameInput.text; } set { songNameInput.text = value; } }
    [SerializeField] private TMP_InputField bpmInput; public string BPM { get { return bpmInput.text; } set { bpmInput.text = value; } }

    private const string validNum = "0123456789.";

    public GameObject noteHolder;

    private string data;

    private EditorNoteObject obj;

    public void SaveSong(string name, GameObject notes)
    {
        
        //Get the file Location
        string path = Application.dataPath + "/SongData/" + name + ".txt";

        data = "\0";

        //Set Header Data
        SongFileInfo info = new SongFileInfo();
        info.songName = name;
        info.version = "V6";
        info.lastEdit = DateTime.Now + "";
        info.bpm = float.Parse(bpmInput.text);

        //Varible to track total notes
        int totalNotes = 0;
        for (int i = 0; i < notes.transform.childCount; i++)
        {
            obj = notes.transform.GetChild(i).GetComponent<EditorNoteObject>();

            Note noteObj = obj.GetNoteData();
            totalNotes += noteObj.isLongNote ? (int)noteObj.longNoteLen + 1 : 1; 

            data += "\n" + JsonUtility.ToJson(noteObj);
        }
        info.totalNotes = totalNotes;
        data = JsonUtility.ToJson(info) + data;

        data += "\nEnd";

        //if (!File.Exists(path))
        //{ 
        Debug.Log("Scaldedel");
        File.WriteAllText(path, data);
        //}
    }

    public void EditorCompile()
    {
        Debug.Log("dad");
        SaveSong(songNameInput.text, noteHolder);
    }

    private void Start()
    {
        bpmInput.onValidateInput = (string text, int charIndex, char addedChar) => { return ValidateCharacter(validNum, addedChar); };
        bpmInput.characterLimit = 10;
    }

    private char ValidateCharacter(string validCharacters, char addedChr)
    {
        //Debug.Log(validCharacters + " " + addedChr + " " + validCharacters.IndexOf(addedChr));
        if (validCharacters.IndexOf(addedChr) != -1)
        {
            return addedChr;
        }
        else
        {
            Debug.Log("Nonvalid!");
            return '\0'; //Null Character
        }

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

public struct SongFileInfo
{
    public string songName;
    public string version;
    public string lastEdit;
    public float bpm;
    public int totalNotes;
}
