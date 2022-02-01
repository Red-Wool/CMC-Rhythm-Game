using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

//No Longer in use
//Was just used to get data from original Purple song
/*public class ArrowArchive : EditorWindow
{

    string songName;
    GameObject parent;
    //SongComplier sc;

    [MenuItem("Tools/Arrow Archiver")] //Method toBe Called when going to the tools and stuff
    public static void ShowWindow()
    {
        GetWindow(typeof(ArrowArchive)); //GetWindow is inherited from EditorWindow class;
    }

    private void OnGUI()
    {
        songName = EditorGUILayout.TextField("Song Name", songName);

        parent = EditorGUILayout.ObjectField("Song Data Parent", parent, typeof(GameObject), true) as GameObject;
        //sc = EditorGUILayout.ObjectField("Song Compiler", sc, typeof(SongComplier), false) as SongComplier;

        if (GUILayout.Button("Archive"))
        {
            ArchiveArrows();
        }
    }

    private void ArchiveArrows()
    {
        //Get the file Location
        string path = Application.dataPath + "/SongData/" + songName + ".txt";

        string data = name + "\nLast Edited: " + DateTime.Now + "\nVersion\nV6";
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            NoteObject obj = parent.transform.GetChild(i).GetComponent<NoteObject>();



            data += "\n" + JsonUtility.ToJson(obj.GetNoteData());

            //data += "\n" + obj.GetNoteColor() + " " + notes.transform.GetChild(i).transform.position.y + " " + obj.GetIfLongNote() + " " + obj.GetLength();
        }
        data += "\nEnd";

        //if (!File.Exists(path))
        //{ 
        Debug.Log("Scaldedel");
        File.WriteAllText(path, data);
        //}
    }
}*/
