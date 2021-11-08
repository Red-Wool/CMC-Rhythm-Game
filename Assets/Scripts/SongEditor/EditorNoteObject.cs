using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorNoteObject : NoteClass
{
    [SerializeField]
    private bool isLongNote;
    [SerializeField]
    private float length;
    [SerializeField]
    private GameObject longNoteMiddle;
    [SerializeField]
    private GameObject longNoteEnd;


    public override NoteType GetNoteType { get { return NoteType.Normal; } }
    public bool GetIfLongNote() { return isLongNote; }
    public float GetLength() { return length; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLongNote(bool flag, float len)
    {
        isLongNote = flag;

        longNoteMiddle.SetActive(flag);
        longNoteEnd.SetActive(flag);
        
        if (flag)
        {
            length = len;

            longNoteMiddle.transform.localPosition = Vector3.up * len / 2;
            longNoteMiddle.transform.localScale = Vector3.one + Vector3.up * (len / 2 - 1);

            longNoteEnd.transform.localPosition = Vector3.up * len;

        }
        else
        {
            length = 0;
        }
    }

    public Note GetNoteData()
    {
        Note note;

        note.color = GetNoteColor();
        note.yVal = transform.position.y;
        note.isLongNote = isLongNote;
        note.longNoteLen = length;

        return note;
    }
}
