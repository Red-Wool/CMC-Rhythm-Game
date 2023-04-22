using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLineScript : MonoBehaviour
{
    public GameObject end;
    public Vector3 displace;

    public ArrowPathModule[] moveModules;

    LineRenderer lr;
    Vector3[] lines;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();

        lines = new Vector3[100];
        lr.positionCount = 100;

        for (int i = 0; i < moveModules.Length; i++)
        {
            moveModules[i].RequestData();
        }
    }

    public Vector3 GetPos(float time)
    {
        Vector3 total = Vector3.zero;
        for (int i = 0; i < moveModules.Length; i++)
        {
            //Debug.Log(moveModules[i].notePosID + " " + moveModules[i].notePos);
            total += moveModules[i].CalculateNotePosition(time);
        }
        return total;

        //return new Vector3(Mathf.Sin(time * 5f + Time.time) * time, Mathf.Cos(time * 5f + Time.time) * time * 2.5f, Mathf.Cos(time * 5f + Time.time) * time);
        //new Vector3(1/Mathf.Cos(time*.2f + Time.time*2f) * Mathf.Min(time, 1), time);
        //new Vector3(Mathf.Sin(time*51f+Time.time*9f)*time, Mathf.Cos(time*3f+Time.time*4f)*time, Mathf.Sin(Mathf.Cos(time * 5f + Time.time) * time + 23 * Time.time));
        //new Vector3(Mathf.Acos(Mathf.Cos(time + Time.time)) * Mathf.Min(time, 1f), time-4f);
        //new Vector3(Mathf.Sin(time*5f+Time.time)*time, Mathf.Cos(time*5f+Time.time)*time, time); 
        //new Vector3(Mathf.Sin(time+Time.time)*Mathf.Min(time, 1),time);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 1; i < 100; i++)
        {
            lines[i] = GetPos(i*.05f) + displace;
        }
        lr.SetPositions(lines);
        end.transform.position = GetPos(4.95f) + displace;
    }
}
