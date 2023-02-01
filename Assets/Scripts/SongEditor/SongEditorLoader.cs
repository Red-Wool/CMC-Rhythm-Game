using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SongEditorLoader : MonoBehaviour
{
    [SerializeField] private SongComplier compiler;
    [SerializeField] private GameObject noteHolder;
    [SerializeField] private GameObject[] editorNotePrefab;
    [SerializeField] private GameObject[] editorEffectPrefab;

    SongFileInfo songInfo;
    GameObject effectObject;
    MoveModule moveData;
    ArrowPathModule arrowPathData;
    Note noteData;

    EffectStat effectStat;

    GameObject gameObj;
    EditorNoteObject ediObj;

    public void EditorLoad()
    {
        Debug.Log("Loading File!");
        if (noteHolder.transform.childCount < 10 || compiler.loadFlag)
        {
            songInfo = LoadSong(compiler.SongName);
            compiler.SongFileName = songInfo.songFileName;
            compiler.BPM = songInfo.bpm.ToString();
            compiler.Scroll = songInfo.startSpeed.ToString();
            compiler.Delay = songInfo.startDelay.ToString();
            compiler.End = songInfo.endPos.ToString();
            compiler.loadFlag = false;
            compiler.warningText.text = "";
            Debug.Log("Loading files complete!");
        }
        else
        {
            compiler.loadFlag = true;
            compiler.warningText.text = "Sure you want to Load?";
        }

    }

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
                        effectStat = JsonUtility.FromJson<EffectStat>(inpStr);
                        i++;

                        effectObject = SetUpEffectEditor((int)effectStat.type, effectStat);
                        switch (effectStat.type)
                        {
                            case EffectType.Move:
                                moveData = JsonUtility.FromJson<MoveModule>(inpStr);
                                effectObject.GetComponent<EditorMoveEffect>().move = moveData;
                                break;
                            case EffectType.ArrowPath:
                                arrowPathData = JsonUtility.FromJson<ArrowPathModule>(inpStr);
                                effectObject.GetComponent<EditorArrowPathEffect>().arrowPath = arrowPathData;
                                break;
                        }
                        
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

    public GameObject SetUpEffectEditor(int id, EffectStat data)
    {
        gameObj = Instantiate(editorEffectPrefab[id], noteHolder.transform);
        gameObj.GetComponent<EditorEffectTriggerObject>().SetUp(data);
        return gameObj;
    }

    public void SetUpEditorNote(Note data)
    {
        gameObj = Instantiate(editorNotePrefab[(int)data.color], noteHolder.transform);

        ediObj = gameObj.GetComponent<EditorNoteObject>();

        ediObj.SetY(data.yVal);
        ediObj.SetLongNote(data.isLongNote, data.longNoteLen);
    }


}
