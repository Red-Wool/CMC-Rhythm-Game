using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class SongLoader : MonoBehaviour
{
    public GameObject noteHolder;
    //public string songName;

    [SerializeField]
    private GameObject[] editorNotePrefab;
    [SerializeField]
    private GameObject[] notePrefab;
    [SerializeField]
    private GameObject[] longNoteMiddlePrefab;
    [SerializeField]
    private GameObject[] longNoteEndPrefab;
    [SerializeField]
    private SongComplier compiler;

    //Extra Varibles so only instatiated once
    private Note noteData;
    private GameObject gameObj;

    private EditorNoteObject ediObj;

    private NoteObject noteObj;
    private SongFileInfo songInfo;

    private Vector3 pos;
    private Vector3 scaleTemp;
    private int length;

    public SongFileInfo LoadSong(string name, bool isEditor)
    {
        //Varible Declaration
        GameObject[] childrenList = new GameObject[noteHolder.transform.childCount];

        //Remove Existing Notes
        for (int i = 0; i < noteHolder.transform.childCount; i++)
        {
            childrenList[i] = noteHolder.transform.GetChild(i).gameObject;
        }
        foreach(GameObject i in childrenList)
        {
            Destroy(i);
        }

        //Get Path
        string path = Application.dataPath + "/SongData/" + name + ".txt";

        //Check if Path Exists
        if (File.Exists(path))
        {
            //Get Stream reader to read txt file
            StreamReader textFile = new StreamReader(path);

            //Get Song Info
            songInfo = JsonUtility.FromJson<SongFileInfo>(textFile.ReadLine());

            //Set Speed
            float spdMult = songInfo.startSpeed;
            float bpm = songInfo.bpm;

            //Go Through entire file until the end
            while (!textFile.EndOfStream)
            {
                string inpStr = textFile.ReadLine();
                if (inpStr != "End")
                {
                    noteData = JsonUtility.FromJson<Note>(inpStr);

                    //Check if editor edition
                    if (isEditor)
                    {
                        SetUpEditorNote(noteData);
                    }
                    else
                    {
                        SetUpGameNote(noteData, bpm, spdMult);
                    }
                }
            }

            //Close the text file
            textFile.Close();

            return songInfo;
        }
        else
        {
            //Oh crap you gave the wrong path
            Debug.Log("Invalid Path! Like what is this?? " + path);
        }

        return new SongFileInfo();
    }

    public void SetUpEditorNote(Note data)
    {
        gameObj = Instantiate(editorNotePrefab[(int)data.color], noteHolder.transform);

        ediObj = gameObj.GetComponent<EditorNoteObject>();

        ediObj.SetY(data.yVal);
        ediObj.SetLongNote(data.isLongNote, data.longNoteLen);
    }

    public int SetUpGameNote(Note data, float bpm, float speedMultiplier) //FIX IT to actually work
    {

        //Track Total Notes
        //totalNotes++;
        gameObj = Instantiate(notePrefab[(int)data.color], noteHolder.transform);

        //Set Arrows in sync with speed Multiplier
        pos = gameObj.transform.localPosition;
        pos.y = data.yVal * speedMultiplier;
        gameObj.transform.localPosition = pos;

        //Set Up Note
        noteObj = gameObj.GetComponent<NoteObject>();

        noteObj.yVal = data.yVal / (bpm / 30);

        if (data.isLongNote && data.longNoteLen != 0f)
        {
            length = (int)data.longNoteLen;

            //totalNotes += lengthVal;

            data.longNoteLen *= speedMultiplier / 2;
            pos.y -= 6f;

            //Instantiate Long Note Middle
            pos.y += data.longNoteLen;
            gameObj = Instantiate(longNoteMiddlePrefab[(int)data.color], pos, Quaternion.identity, noteHolder.transform);

            scaleTemp = Vector3.one;
            scaleTemp.y = data.longNoteLen;

            gameObj.transform.localScale = scaleTemp;

            gameObj.GetComponent<LongNoteObject>().LongNoteSetup(
                noteObj.gameObject,
                data.longNoteLen * 2,
                length
                );

            pos.y += data.longNoteLen;
            Instantiate(longNoteEndPrefab[(int)data.color], pos, Quaternion.identity, this.transform);

            return length + 1;
        }

        return 1;
    }

    //Method for Song Editor Button
    public void EditorLoad()
    {
        Debug.Log("Loading File!");
        songInfo = LoadSong(compiler.SongName, true);
        compiler.SongFileName = songInfo.songFileName;
        compiler.BPM = songInfo.bpm.ToString();
        compiler.Scroll = songInfo.startSpeed.ToString();
    }
}
