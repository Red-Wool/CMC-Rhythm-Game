using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SongLoader : MonoBehaviour
{
    public GameObject noteHolder;
    public string name;

    [SerializeField]
    private GameObject[] editorNotePrefab;
    [SerializeField]
    private GameObject[] notePrefab;
    [SerializeField]
    private GameObject[] longNoteMiddlePrefab;
    [SerializeField]
    private GameObject[] longNoteEndPrefab;

    //Extra Varibles so only instatiated once
    private Note noteData;
    private GameObject gameObj;

    private EditorNoteObject ediObj;

    private NoteObject noteObj;

    private Vector3 pos;
    private Vector3 scaleTemp;
    private int length;

    public void LoadSong (string name)
    {
        LoadSong(name, 0f);
    }

    public int LoadSong(string name, float spdMult)
    {
        string path = Application.dataPath + "/SongData/" + name + ".txt";
        int lineCount = 1;

        int totalNotes = 0;

        if (File.Exists(path))
        {
            StreamReader textFile = new StreamReader(path);

            while (!textFile.EndOfStream)
            {
                string inpStr = textFile.ReadLine();
                if (lineCount > 4 && inpStr != "End")
                {
                    noteData = JsonUtility.FromJson<Note>(inpStr);

                    if (spdMult == 0f)
                    {
                        SetUpEditorNote(noteData);
                    }
                    else
                    {
                        totalNotes += SetUpGameNote(noteData, spdMult);
                    }
                }

                lineCount++;
            }

            textFile.Close();

            return totalNotes;
        }
        else
        {
            Debug.Log("Invalid Path! Like what is this?? " + path);
        }

        return 0;
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

    public void Test()
    {
        Debug.Log("loading");
        LoadSong(name);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
