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

    public SongFileInfo LoadSong(string name, float spdMult)
    {
        //Remove all the other notes
        GameObject[] childrenList = new GameObject[noteHolder.transform.childCount];

        for (int i = 0; i < noteHolder.transform.childCount; i++)
        {
            childrenList[i] = noteHolder.transform.GetChild(i).gameObject;
        }

        foreach(GameObject i in childrenList)
        {
            Destroy(i);
        }

        string path = Application.dataPath + "/SongData/" + name + ".txt";
        int lineCount = 1;

        if (File.Exists(path))
        {
            StreamReader textFile = new StreamReader(path);

            songInfo = JsonUtility.FromJson<SongFileInfo>(textFile.ReadLine());

            while (!textFile.EndOfStream)
            {
                string inpStr = textFile.ReadLine();
                //Debug.Log(inpStr);
                if (inpStr != "End")
                {
                    noteData = JsonUtility.FromJson<Note>(inpStr);

                    if (spdMult == 0f)
                    {
                        SetUpEditorNote(noteData);
                    }
                    else
                    {
                        SetUpGameNote(noteData, spdMult);
                    }
                }

                lineCount++;
            }

            textFile.Close();

            return songInfo;
        }
        else
        {
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

    public int SetUpGameNote(Note data, float speedMultiplier) //FIX IT to actually work
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
        Debug.Log("loading");
        songInfo = LoadSong(compiler.SongName, 0f);
        compiler.BPM = songInfo.bpm.ToString();

    }
}
