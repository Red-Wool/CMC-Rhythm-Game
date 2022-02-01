using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ArrowSpawner : EditorWindow
{
    //Varible Declaration

    float yVal = 0;

    GameObject parent;

    GameObject leftArrowObj;
    GameObject upArrowObj;
    GameObject downArrowObj;
    GameObject rightArrowObj;

    //int arrowSel = 0;
    //string[] arrowStrings = { "Left/Red", "Up/Blue", "Down/Green", "Right/Yellow" };

    GameObject storedObj;
    Vector3 pos;

    [MenuItem("Tools/Arrow Spawner")] //Method toBe Called when going to the tools and stuff
    public static void ShowWindow()
    {
        GetWindow(typeof(ArrowSpawner)); //GetWindow is inherited from EditorWindow class;
    }

    private void OnGUI()
    {
        GUILayout.Label("Arrow Spawning Prefab Information", EditorStyles.boldLabel);
        //GUILayout.Label("Arrow Spawning Prefab Information", EditorStyles.whiteLabel);
        //EditorGUILayout.Popup(1, new string[] {"pee", "poo", "pop" }, EditorStyles.boldLabel);

        parent = EditorGUILayout.ObjectField("Note Container", parent, typeof(GameObject), true) as GameObject;

        GUILayout.Space(15);

        leftArrowObj = EditorGUILayout.ObjectField("Left Arrow Object", leftArrowObj, typeof(GameObject), false) as GameObject;
        upArrowObj = EditorGUILayout.ObjectField("Up Arrow Object", upArrowObj, typeof(GameObject), false) as GameObject;
        downArrowObj = EditorGUILayout.ObjectField("Down Arrow Object", downArrowObj, typeof(GameObject), false) as GameObject;
        rightArrowObj = EditorGUILayout.ObjectField("Right Arrow Object", rightArrowObj, typeof(GameObject), false) as GameObject;

        GUILayout.Space(15);

        GUILayout.Label("Spawn Notes", EditorStyles.boldLabel);

        yVal = EditorGUILayout.FloatField("Y Coordinate", yVal);

        //arrowSel = GUILayout.SelectionGrid(arrowSel, arrowStrings, 4);
        GUILayout.BeginHorizontal();

            if (GUILayout.Button("Spawn Left Arrow"))
            {
                SpawnArrow(0);
            }
            if (GUILayout.Button("Spawn Up Arrow"))
            {
                SpawnArrow(1);
            }
            if (GUILayout.Button("Spawn Down Arrow"))
            {
                SpawnArrow(2);
            }
            if (GUILayout.Button("Spawn Right Arrow"))
            {
                SpawnArrow(3);
            }

        GUILayout.EndHorizontal();

    }


    private void SpawnArrow(int arrowNum)
    {
        if (leftArrowObj == null || upArrowObj == null || downArrowObj == null || rightArrowObj == null)
        {
            Debug.LogError("Need to put in a Prefab for the arrows");

            return;
        }


        switch (arrowNum)
        {
            case 0:
                storedObj = leftArrowObj;
                break;
            case 1:
                storedObj = upArrowObj;
                break;
            case 2:
                storedObj = downArrowObj;
                break;
            case 3:
                storedObj = rightArrowObj;
                break;
        }

        storedObj = Instantiate(storedObj, parent.transform);

        pos = storedObj.transform.localPosition;
        pos.y = yVal;
        storedObj.transform.localPosition = pos;
    }
}
