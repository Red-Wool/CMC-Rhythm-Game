using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
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

    [SerializeField] private GameObject effectTriggerPrefab;
    [SerializeField] private GameObject effectTriggerEditorPrefab;

    [SerializeField]
    private SongComplier compiler;

    //Extra Varibles so only instatiated once
    private Note noteData;
    private EffectModule effectData;
    private GameObject gameObj;

    private EditorNoteObject ediObj;

    private NoteObject noteObj;
    private SongFileInfo songInfo;

    private Vector3 pos;
    private Vector3 scaleTemp;
    private int length;
    private float temp;

    public string[] invalidStrings = {"End", "Effect", "Note"};

    public SongFileInfo LoadSong(string name, bool isEditor)
    {
        //Varible Declaration
        if (isEditor)
        {
            GameObject[] childrenList = new GameObject[noteHolder.transform.childCount];

            //Remove Existing Notes
            for (int i = 0; i < noteHolder.transform.childCount; i++)
            {
                childrenList[i] = noteHolder.transform.GetChild(i).gameObject;
            }
            foreach (GameObject i in childrenList)
            {
                Destroy(i);
            }
        }


        //Get Path
        //string path = Application.dataPath + "/SongData/" + name + ".txt";
        TextAsset textFile = LoadAssetBundle.GetSongData(name);

        //Check if Path Exists
        if (textFile != null)
        {
            //Get Stream reader to read txt file
            //StringReader textFile = new StringReader(;
            string[] text = Regex.Split(textFile.text, "\n");

            //Get Song Info
            songInfo = JsonUtility.FromJson<SongFileInfo>(text[0]);

            //Set Speed
            float spdMult = songInfo.startSpeed;
            float bpm = songInfo.bpm;

            bool effect = false;

            //Go Through entire file until the end
            for (int i = 1; i < text.Length; i++)
            {
                string inpStr = text[i].Trim();
                inpStr.Replace("\n", "");
                if (inpStr == "Effect")
                {
                    effect = true;
                }

                if (inpStr != "Note" && inpStr != "Effect" && inpStr != "End")
                {
                    if (effect)
                    {
                        effectData = JsonUtility.FromJson<EffectModule>(inpStr);

                        if (isEditor)
                        {
                            SetUpEffectEditor(effectData);
                        }
                        else
                        {
                            SetUpEffect(effectData, bpm);
                        }
                    }
                    else
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
            }

            //Close the text file
            //textFile.Close();

            return songInfo;
        }
        else
        {
            //Oh crap you gave the wrong path
            Debug.Log("Invalid Path! Like what is this? " + name);
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
        Transform parent = GameManager.instance.bs.ArrowLines((int)data.color).transform;
        gameObj = Instantiate(notePrefab[(int)data.color], parent);

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
            temp = data.longNoteLen;

            data.yVal += data.longNoteLen;

            //totalNotes += lengthVal;

            data.longNoteLen *= speedMultiplier / 2;
            //pos.y -= 6f;

            //Instantiate Long Note Middle
            pos.y += data.longNoteLen;
            gameObj = Instantiate(longNoteMiddlePrefab[(int)data.color], pos, parent.transform.rotation, parent); //arrowButton.transform.position + Vector3.up * ((1f - percent) * tempLength / 2)
            gameObj.transform.localPosition = pos;

            scaleTemp = Vector3.one;
            scaleTemp.y = data.longNoteLen;

            gameObj.transform.localScale = scaleTemp;

            gameObj.GetComponent<LongNoteObject>().LongNoteSetup(
                noteObj,
                GameManager.instance.bs.ArrowButtons((int)data.color),
                data.longNoteLen * 2,
                length,
                temp / (bpm / 30)
                );

            pos.y += data.longNoteLen;
            gameObj = Instantiate(longNoteEndPrefab[(int)data.color], pos, parent.transform.rotation, parent);
            gameObj.GetComponent<LongNoteEndObject>().yVal = data.yVal / (bpm / 30);
            gameObj.transform.localPosition = pos; 

            return length + 1;
        }

        return 1;
    }

    public void SetUpEffectEditor(EffectModule data)
    {
        gameObj = Instantiate(effectTriggerEditorPrefab, noteHolder.transform);
        gameObj.GetComponent<EditorEffectTriggerObject>().SetUp(data);
    }

    public void SetUpEffect(EffectModule data, float bpm)
    {
        gameObj = Instantiate(effectTriggerPrefab);
        gameObj.GetComponent<EffectTriggerObject>().SetupEffect(data, bpm);
    }

    //Method for Song Editor Button
    public void EditorLoad()
    {
        Debug.Log("Loading File!");
        if (noteHolder.transform.childCount < 10 || compiler.loadFlag)
        {
            songInfo = LoadSong(compiler.SongName, true);
            compiler.SongFileName = songInfo.songFileName;
            compiler.BPM = songInfo.bpm.ToString();
            compiler.Scroll = songInfo.startSpeed.ToString();
            compiler.loadFlag = false;
            compiler.warningText.text = "";
        }
        else
        {
            compiler.loadFlag = true;
            compiler.warningText.text = "Sure you want to Load?";
        }
        
    }

    private bool WordCheck(string word)
    {
        for (int i = 0; i < invalidStrings.Length; i++)
        {
            //Debug.Log(" ?= " + invalidStrings[i]); //invalidStrings[i]);
            if (invalidStrings[i].Trim() == word.Trim())
            {
                return false;
            }
        }
        return true;
    }
}
