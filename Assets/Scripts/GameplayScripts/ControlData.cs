using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Control Data")]
public class ControlData : ScriptableObject
{
    public KeyCode left;
    public KeyCode up;
    public KeyCode down;
    public KeyCode right;

    public KeyCode leftAlt;
    public KeyCode upAlt;
    public KeyCode downAlt;
    public KeyCode rightAlt;

    public KeyCode GetMainKey(NoteColor col)
    {
        switch (col)
        {
            case NoteColor.Red:
                return left;
            case NoteColor.Blue:
                return up;
            case NoteColor.Green:
                return down;
            case NoteColor.Yellow:
                return right;
        }
        return left;
    }

    public KeyCode GetAltKey(NoteColor col)
    {
        switch (col)
        {
            case NoteColor.Red:
                return leftAlt;
            case NoteColor.Blue:
                return upAlt;
            case NoteColor.Green:
                return downAlt;
            case NoteColor.Yellow:
                return rightAlt;
        }
        return left;
    }
}
