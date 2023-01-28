using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SongEditorLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject noteHolder;
    [SerializeField]
    private GameObject[] editorNotePrefab;
    [SerializeField]
    private GameObject effectPrefab;

    SongFileInfo songInfo;
    MoveModule moveData;
    Note noteData;

    GameObject gameObj;
    EditorNoteObject ediObj;

    public SongFileInfo LoadSong(string songName)
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

        string[] text;

        text = File.ReadAllLines(Application.dataPath + "/AssetBundles/songdata/" + songName + ".txt");
        

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
                        moveData = JsonUtility.FromJson<MoveModule>(inpStr);
                        
                        // SET UP THIS
                        SetUpEffectEditor(new EffectStat());
                    }
                    else
                    {
                        noteData = JsonUtility.FromJson<Note>(inpStr);

                        //Check if editor edition
                        SetUpEditorNote(noteData);
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

    public void SetUpEffectEditor(EffectStat data)
    {
        gameObj = Instantiate(effectPrefab, noteHolder.transform);
        gameObj.GetComponent<EditorEffectTriggerObject>().SetUp(data);
    }

    public void SetUpEditorNote(Note data)
    {
        gameObj = Instantiate(editorNotePrefab[(int)data.color], noteHolder.transform);

        ediObj = gameObj.GetComponent<EditorNoteObject>();

        ediObj.SetY(data.yVal);
        ediObj.SetLongNote(data.isLongNote, data.longNoteLen);
    }


}
