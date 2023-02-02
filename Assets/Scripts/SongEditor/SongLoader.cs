using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;
using TMPro;

public class SongLoader : MonoBehaviour
{
    public GameObject noteHolder;

    [SerializeField]
    private GameObject[] notePrefab;
    [SerializeField]
    private GameObject[] longNoteMiddlePrefab;
    [SerializeField]
    private GameObject[] longNoteEndPrefab;

    [SerializeField] private GameObject effectTriggerPrefab;

    //Extra Varibles so only instatiated once
    private Note noteData;
    private MoveModule effectData;
    private GameObject gameObj;

    private NoteButton noteButton;
    private NoteObject noteObj;
    private LongNoteObject longNoteObj;
    private LongNoteEndObject longNoteEndObj;
    private SongFileInfo songInfo;

    private Vector3 pos;
    private Vector3 scaleTemp;
    private int length;
    private float temp;

    public string[] invalidStrings = {"End", "Effect", "Note"};

    public SongFileInfo LoadSong(string name)
    {

        //Get Path
        //string path = Application.dataPath + "/SongData/" + name + ".txt";
        TextAsset textFile;
        string[] text;

        textFile = LoadAssetBundle.GetSongData(name);
        text = Regex.Split(textFile.text, "\n");

        //Check if Path Exists
        if (text.Length != 0)
        {
            //Get Stream reader to read txt file
            //StringReader textFile = new StringReader(;
            

            //Get Song Info
            songInfo = JsonUtility.FromJson<SongFileInfo>(text[0]);

            //Set Speed
            float spdMult = songInfo.startSpeed;
            float bpm = songInfo.bpm;
            float delay = songInfo.startDelay / (bpm / 30);

            //songInfo.startDelay /= bpm / 30;
            //songInfo.endPos /= bpm / 30;

            
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

                //Debug.Log(inpStr);

                if (inpStr != "Note" && inpStr != "Effect" && inpStr != "End")
                {
                    if (effect)
                    {
                        effectData = JsonUtility.FromJson<MoveModule>(inpStr);

                        /*if (isEditor)
                        {
                            SetUpEffectEditor(effectData);
                        }
                        else
                        {
                            SetUpEffect(effectData, bpm);
                        }*/
                    }
                    else
                    {
                        noteData = JsonUtility.FromJson<Note>(inpStr);

                        //Check if editor edition
                        SetUpGameNote(noteData, bpm, spdMult, delay);
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
        //Why is it here twice
        Debug.Log("Invalid Path! Like what is this? " + name);
        return new SongFileInfo();
    }

    public int SetUpGameNote(Note data, float bpm, float speedMultiplier, float delay)
    {

        //Track Total Notes
        //totalNotes++;
        Transform parent = GameManager.instance.bs.ArrowLines((int)data.color).transform;
        noteButton = GameManager.instance.bs.ArrowButtons((int)data.color).GetComponent<NoteButton>();
        gameObj = Instantiate(notePrefab[(int)data.color], parent);

        //Set Arrows in sync with speed Multiplier
        pos = gameObj.transform.localPosition;
        pos.y = (data.yVal) * speedMultiplier;
        gameObj.transform.localPosition = pos;

        //Set Up Note
        noteObj = gameObj.GetComponent<NoteObject>();

        noteObj.SetUpNote(noteButton);
        noteObj.yVal = (data.yVal) / (bpm / 30f);

        noteObj.count = data.temp;

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

            longNoteObj = gameObj.GetComponent<LongNoteObject>();
            longNoteObj.SetUpNote(noteButton);
            longNoteObj.LongNoteSetup(
                noteObj,
                GameManager.instance.bs.ArrowButtons((int)data.color),
                data.longNoteLen * 2,
                length,
                temp / (bpm / 30)
                );

            pos.y += data.longNoteLen;
            gameObj = Instantiate(longNoteEndPrefab[(int)data.color], pos, parent.transform.rotation, parent);

            longNoteEndObj = gameObj.GetComponent<LongNoteEndObject>();
            longNoteEndObj.SetUpNote(noteButton);
            longNoteEndObj.yVal = data.yVal / (bpm / 30);

            gameObj.transform.localPosition = pos; 

            return length + 1;
        }

        return 1;
    }

    public void SetUpEffect(EffectStat data, float bpm)
    {
        gameObj = Instantiate(effectTriggerPrefab);
        gameObj.GetComponent<EffectTriggerObject>().SetupEffect(data, bpm);
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
