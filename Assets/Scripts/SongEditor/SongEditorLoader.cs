using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongEditorLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject noteHolder;
    [SerializeField]
    private GameObject[] editorNotePrefab;

    /*
    public SongFileInfo LoadSong()
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

        TextAsset textFile;
        string[] text;
    }
    */
}
