using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    [Header("Song Speed"), SerializeField]
    private string songName;
    public float bpm;
    private float beatTempo;
    private int totalNotes;

    [HideInInspector]
    public bool playing;

    private const float averageBeatNormal = 3f;

    [Header("Instatiate References"), Space(10), SerializeField]
    private SongLoader sl;

    [SerializeField] private GameObject arrowLineParent;
    [SerializeField] private GameObject[] arrowButtons; public GameObject ArrowButtons(int i) { if (arrowButtons == null) arrowButtons = GameObject.FindGameObjectsWithTag("Activator"); return arrowButtons[i]; }
    [SerializeField] private GameObject[] arrowLines; public GameObject ArrowLines(int i) { return arrowLines[i]; }
    [SerializeField] private GameObject flashScreen;


    private SongFileInfo songInfo;

    private Vector3 tempPos;

    // Start is called before the first frame update
    void Start()
    {

        totalNotes = 0;

        songInfo = sl.LoadSong(PlayerPrefs.GetString("CurrentMap"), false);
        totalNotes = songInfo.totalNotes;
        bpm = songInfo.bpm;

        //Calculate the speed of which notes scroll down
        beatTempo = bpm * songInfo.startSpeed / 30;

        //arrowButtons = GameObject.FindGameObjectsWithTag("Activator");

        Debug.Log("Loaded!");
    }

    public SongFileInfo StartGame()
    {
        /*totalNotes = 0;

        songInfo = sl.LoadSong(songName, false);
        totalNotes = songInfo.totalNotes;
        bpm = songInfo.bpm;

        //Calculate the speed of which notes scroll down
        beatTempo = bpm * songInfo.startSpeed / 30;

        arrowButtons = GameObject.FindGameObjectsWithTag("Activator");*/

        playing = true;

        //GameManager.instance.buttonSize = beatTempo / averageBeatNormal;
        return songInfo;
    }

    // Update is called once per frame
    void Update()
    {
        //Scrolls down when Playing
        /*if (playing)
        {
            tempPos = Vector3.up * beatTempo * Time.deltaTime;
            foreach (GameObject i in arrowLines)
            {
                i.transform.localPosition -= tempPos;
            }
        }*/
    }

    public void SetY(float y)
    {
        foreach (GameObject i in arrowLines)
        {
            i.transform.localPosition = Vector3.up * y * beatTempo;
        }
    }

    public int GetTotalNotes() {return totalNotes;}

    public void ActivateMoveEffect(MoveModule effect)
    {
        switch (effect.objID.Trim())
        {
            case "Main":
                effect.Activate(arrowLineParent);
                break;
            case "RedLine":
                effect.Activate(arrowButtons[0]);
                break;
            case "BlueLine":
                effect.Activate(arrowButtons[1]);
                break;
            case "GreenLine":
                effect.Activate(arrowButtons[2]);
                break;
            case "YellowLine":
                effect.Activate(arrowButtons[3]);
                break;
            case "FlashScreen":
                effect.Activate(flashScreen);
                break;
        }
    }


}
