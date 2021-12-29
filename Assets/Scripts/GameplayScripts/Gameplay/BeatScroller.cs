using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    [Header("Song Speed")]
    public float bpm;
    private float beatTempo;
    private int totalNotes;

    [HideInInspector]
    public bool playing;

    private const float averageBeatNormal = 3f;

    [Header("Instatiate References"), Space(10), SerializeField]
    private SongLoader sl;

    private GameObject[] arrowButtons;

    private SongFileInfo songInfo;

    // Start is called before the first frame update
    void Start()
    {
        totalNotes = 0;

        songInfo = sl.LoadSong("Stream", false);
        totalNotes = songInfo.totalNotes;
        bpm = songInfo.bpm;

        //Calculate the speed of which notes scroll down
        beatTempo = bpm * songInfo.startSpeed / 30;

        arrowButtons = GameObject.FindGameObjectsWithTag("Activator");

        //Modify Arrow Buttons to account for speed
        for (int i = 0; i < arrowButtons.Length; i++)
        {
            arrowButtons[i].GetComponent<BoxCollider2D>().size = new Vector2(1f, 2f * (beatTempo / averageBeatNormal));
        }


        Debug.Log("Loaded!");
    }

    public SongFileInfo StartGame()
    {
        playing = true;

        GameManager.instance.buttonSize = beatTempo / averageBeatNormal;
        return songInfo;
    }

    // Update is called once per frame
    void Update()
    {
        //Scrolls down when Playing
        if (playing)
        {
            transform.position -= Vector3.up * beatTempo * Time.deltaTime;
        }
    }

    public int GetTotalNotes() {return totalNotes;}

    //public GameObject[] GetArrowGameObjects() {return arrow }
}
