using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    [Header("Song Speed")]
    public float bpm;
    private float beatTempo;

    public float speedMultiplier;

    private int totalNotes;

    [HideInInspector]
    public bool playing;

    private const float averageBeatNormal = 3f;

    [Header("Instatiate References"), Space(10), SerializeField]
    private SongLoader sl;

    private GameObject[] arrowButtons;

    // Start is called before the first frame update
    void Start()
    {
        totalNotes = 0;

        SongFileInfo songInfo = sl.LoadSong("Stream", speedMultiplier);
        totalNotes = songInfo.totalNotes;
        bpm = songInfo.bpm;

        //Calculate the speed of which notes scroll down
        beatTempo = bpm * speedMultiplier / 30;

        arrowButtons = GameObject.FindGameObjectsWithTag("Activator");

        //Modify Arrow Buttons to account for speed
        for (int i = 0; i < arrowButtons.Length; i++)
        {
            arrowButtons[i].GetComponent<BoxCollider2D>().size = new Vector2(1f, 2f * (beatTempo / averageBeatNormal));
        }


        Debug.Log("Loaded!");
    }

    public void StartGame()
    {
        playing = true;

        GameManager.instance.buttonSize = beatTempo / averageBeatNormal;
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
