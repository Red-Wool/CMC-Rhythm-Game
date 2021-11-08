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

    private GameObject longNoteMaskPrefab;
    [SerializeField]
    private GameObject[] longNoteMiddlePrefab;
    [SerializeField]
    private GameObject[] longNoteEndPrefab;

    //Private Gameobjects declared early to save memory;
    private Vector3 pos;
    private Vector3 scaleTemp;

    private GameObject arrow;
    private NoteObject nObj;
    private float tempLength;
    private bool longFlag;
    private int lengthVal;

    private GameObject[] arrowButtons;

    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 1.5f;

        //Calculate the speed of which notes scroll down
        beatTempo = bpm * speedMultiplier / 30;

        arrowButtons = GameObject.FindGameObjectsWithTag("Activator");

        //Modify Arrow Buttons to account for speed
        for (int i = 0; i < arrowButtons.Length; i++)
        {
            arrowButtons[i].GetComponent<BoxCollider2D>().size = new Vector2(1f, 2f * (beatTempo / averageBeatNormal));
        }

        totalNotes = 0;

        sl.LoadSong("TestSong", speedMultiplier);

        //Ajust Notes to correct position based off of speedMultiplier 
        /*
        for (int i = 0; i < transform.childCount; i++)
        {
            //Get Object
            arrow = transform.GetChild(i).gameObject;

            //Don't do anything if it is a long note
            if (!arrow.GetComponent<NoteObject>())
            {
                continue;
            }

            //Track Total Notes
            totalNotes++;

            //Set Arrows in sync with speed Multiplier
            pos = arrow.transform.localPosition;
            pos.y *= speedMultiplier;
            arrow.transform.localPosition = pos;

            //Set Up Note
            nObj = arrow.GetComponent<NoteObject>();

            tempLength = nObj.CheckIfLongNote();

            if (nObj && tempLength != 0f)
            {
                lengthVal = (int)tempLength;

                totalNotes += lengthVal;

                tempLength *= speedMultiplier / 2;
                pos.y -= 6f;

                //Instantiate Long Note Middle
                pos.y += tempLength;
                arrow = Instantiate(longNoteMiddlePrefab[(int)nObj.GetNoteColor()], pos, Quaternion.identity, this.transform);

                scaleTemp = Vector3.one;
                scaleTemp.y = tempLength;

                arrow.transform.localScale = scaleTemp;

                arrow.GetComponent<LongNoteObject>().LongNoteSetup(
                    nObj.gameObject,
                    tempLength * 2,
                    lengthVal
                    );

                //Instantiate Long Note Mask (Removed because masks can interfere with other)
                //pos.y -= tempLength;


                

                //Instantiate Long Note End
                pos.y += tempLength;
                Instantiate(longNoteEndPrefab[(int)nObj.GetNoteColor()], pos, Quaternion.identity, this.transform);

                /* Old Long Note System (Scrapped for being broken and bad)
                while (tempLength > 2.5f)
                {
                    if (longFlag)
                    {
                        pos.y += 0.5f;
                        Instantiate(longNoteMiddlePrefab[(int)nObj.noteCol], pos, Quaternion.identity, this.transform);
                        pos.y -= 0.5f;

                        longFlag = false;
                    }
                    pos.y += 2f;
                    tempLength -= 2f;

                    Instantiate(longNoteMiddlePrefab[(int)nObj.noteCol], pos, Quaternion.identity, this.transform);
                }
                pos.y += tempLength;
                Instantiate(longNoteEndPrefab[(int)nObj.noteCol], pos, Quaternion.identity, this.transform);
                
            }
        }
        */
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
