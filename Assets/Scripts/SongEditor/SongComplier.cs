using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using TMPro;

public class SongComplier : MonoBehaviour
{
    //public string songName;

    [Header("Input Objects"), Space(10), 
     SerializeField] private TMP_InputField songNameInput; public string SongName { get { return songNameInput.text; } set { songNameInput.text = value; } }
    [SerializeField] private TMP_InputField songFileNameInput; public string SongFileName { get { return songFileNameInput.text; } set { songFileNameInput.text = value; } }
    [SerializeField] private TMP_InputField bpmInput; public string BPM { get { return bpmInput.text; } set { bpmInput.text = value; } }
    [SerializeField] private TMP_InputField scrollInput; public string Scroll { get { return scrollInput.text; } set { scrollInput.text = value; } }

    public TMP_Text warningText;

    private const string validNum = "0123456789.";

    public GameObject noteHolder;

    private string data;

    private string noteString;
    private string effectString;

    private EditorNoteObject obj;
    private EditorEffectTriggerObject effectObj;

    public bool saveFlag;
    public bool loadFlag;

    public void SaveSong(string name, GameObject notes)
    {
        
        //Get the file Location
        string path = Application.dataPath + "/AssetBundles/songdata/" + name + ".txt";

        data = "";

        //Set Header Data
        SongFileInfo info = new SongFileInfo();
        info.songName = name;
        info.songFileName = songFileNameInput.text;
        info.version = "V6";
        info.lastEdit = DateTime.Now + "";
        info.bpm = float.Parse(bpmInput.text);
        info.startSpeed = float.Parse(scrollInput.text);

        noteString = "";
        effectString = "";

        //Varible to track total notes
        int totalNotes = 0;
        for (int i = 0; i < notes.transform.childCount; i++)
        {
            obj = notes.transform.GetChild(i).GetComponent<EditorNoteObject>();
            if (obj != null)
            {
                Note noteObj = obj.GetNoteData();
                totalNotes += noteObj.isLongNote ? (int)noteObj.longNoteLen + 1 : 1;

                noteString += "\n" + JsonUtility.ToJson(noteObj);
            }
            else
            {
                effectObj = notes.transform.GetChild(i).GetComponent<EditorEffectTriggerObject>();
                if (effectObj != null)
                {
                    EffectModule noteObj = effectObj.GetData();

                    effectString += "\n" + JsonUtility.ToJson(noteObj);
                }
            }
        }
        data = "\nNote";// + noteString + "\nEffect" + effectString;
        data += noteString;
        data += "\nEffect";
        data += effectString;

        info.totalNotes = totalNotes;
        data = JsonUtility.ToJson(info) + data;

        data += "\nEnd";
        //if (!File.Exists(path))
        //{ 
        Debug.Log("Scaldedel " + path);
        File.WriteAllText(path, data);

        //var importer = UnityEditor.AssetImporter.GetAtPath(path);
        //importer.assetBundleName = "songdata";

        //CreateAssetBundles.BuildAllAssetBundles();
        //}
    }

    public void EditorCompile()
    {
        Debug.Log("Compiling");
        if (noteHolder.transform.childCount > 10 || saveFlag)
        {
            SaveSong(songNameInput.text, noteHolder);
            saveFlag = false;
            warningText.text = "";
        }
        else
        {
            saveFlag = true;
            warningText.text = "Sure you want to save?";
            Debug.Log("Crazy!");
        }
    }

    private void Start()
    {
        saveFlag = false;
        loadFlag = false;

        bpmInput.onValidateInput = (string text, int charIndex, char addedChar) => { return ValidateCharacter(validNum, addedChar); };
        bpmInput.characterLimit = 10;
        scrollInput.onValidateInput = (string text, int charIndex, char addedChar) => { return ValidateCharacter(validNum, addedChar); };
        scrollInput.characterLimit = 10;
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
    public string songFileName;
    public string version;
    public string lastEdit;
    public float bpm;
    public float startSpeed;
    public int totalNotes;
}
