using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class NoteClass : MonoBehaviour
{
    [SerializeField]
    protected KeyCode keyPress;
    [SerializeField]
    protected KeyCode altKeyPress;
    [SerializeField]
    protected NoteColor noteCol;

    public KeyCode GetKeyPress() { return keyPress; }
    public KeyCode GetAltKeyPress() { return altKeyPress; }
    public NoteColor GetNoteColor() { return noteCol; }

    public void SetY(float yVal)
    {
        transform.position = transform.position + (Vector3.up * (yVal - transform.position.y)); 
    }

    public abstract NoteType GetNoteType { get; }

}
