using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Manages Hits, Playing music, and Telling other componenets what to do
public class GameManager : MonoBehaviour
{
    //Reference for everyone
    [HideInInspector]
    public static GameManager instance;

    //Music
    [Header("Music")]
    public AudioSource music;

    public bool playing;

    public AudioSource hitSFX;

    //Other Component
    [Header("Componenet References"), Space(10)]
    public BeatScroller bs;
    public ComboCircle cc;
    public EndScoreboard ec;

    //UI + Basically UI
    [Header("UI"), Space(10)]
    public UIEffects uiEffects;

    public TextMeshProUGUI scoreBoard;
    public TextMeshProUGUI comboBoard;
    public TextMeshProUGUI multiplierText;

    public SpriteRenderer hitTextDisplay;

    public List<Sprite> hitTextSprites;

    //Game Stuff: Score, Combo, Multiplier, Hit Types and Counter
    [Header("Game Statistics"), Space(10)]
    public int score;

    public int combo;

    public int currentMultiplier = 1;

    public int[] noteScoring = {200, 150, 100, 75, 25, 25, 0 };
    private int[] comboMultiplierInterval = {0, 6, 18, 36};

    private int[] hitTypeCount = new int[7];

    private int hits;

    private int topCombo;

    private float gameTime; public float GameTime { get { return gameTime; } }

    //[HideInInspector]
    public float buttonSize;

    private bool gameEnd;

    //Extra Varibles declared only once to save memory
    private HitText hitVal;

    private int baseNoteScore;

    // Start is called before the first frame update
    void Start()
    {
        //Singleton of this for Notes to send data to
        instance = this;

        hits = 0;

        score = 0;
        combo = 0;

        topCombo = 0;

        gameEnd = false;

        hitVal = HitText.Miss;
        hitTextDisplay.sprite = null;

        hitTypeCount = new int[7];

        UpdateScoreBoard();
    }

    // Update is called once per frame
    void Update()
    {
        //Press a Key to start Playing
        if (!playing && Input.anyKeyDown)
        {
            playing = true;

            SongFileInfo songInfo = bs.StartGame();
            try
            {
                AudioClip song = Resources.Load<AudioClip>("Music/" + songInfo.songFileName);
                if (song == null)
                {
                    Debug.LogError("Invalid Song Path: Music/" + songInfo.songFileName);
                }
                else
                {
                    music.clip = song;
                    //music.Play();
                }

            }
            catch
            {
                Debug.LogError("Invalid Song Path: Music/" + songInfo.songFileName);
            }

            music.Play();
        }
        else if (playing && !gameEnd) //When Song ends, go display scoreboar and stuff
        {
            if (!music.isPlaying)
            {
                Debug.Log("The End");
                ec.ShowScoreboard(score, topCombo, hitTypeCount, bs.GetTotalNotes(), hits);

                gameEnd = true;
            }
            else
            {
                gameTime += Time.deltaTime;
            }
        }

    }

    //Note Object References this when a note is hit
    public void NoteHit(float value, GameObject note)
    {
        //Debug.Log("Hit");

        value /= buttonSize;

        //Evalute how good the hit was, set points, combo, and art
        hitVal = Evalute(value);

        //Setting stuff, bounce animation, and add score
        NoteHitEffects(hitVal);

        //uiEffects.BounceGameObj(hitTextDisplay.gameObject, //Just know that it says its bad if it is early or late
        //    (hitVal == HitText.Early || hitVal == HitText.Late) ? false : true);

        score += baseNoteScore * currentMultiplier;

        //Hit Stuff ######add back pls 9/2/21
        ParticleManager.instance.PlayParticle(note.transform.position, 
            note.GetComponent<NoteClass>().GetNoteColor(),
            note.GetComponent<NoteClass>().GetNoteType,
            hitVal);

        hitSFX.Play();

        UpdateScoreBoard();
    }

    public void LongNoteHit(bool hitOrMiss, int hitsMissed)
    {
        if (hitOrMiss)
        {
            hitVal = HitText.Perfect;

            score += 50 * currentMultiplier;
        }
        else
        {
            hitVal = HitText.Miss;

            //Debug.Log(hitsMissed);

            //Has -1 to account for NoteHitEffects Adding 1
            hitTypeCount[6] += hitsMissed - 1;
            //Debug.Log(hitTypeCount[6]);
        }

        NoteHitEffects(hitVal);

        UpdateScoreBoard();
        //Debug.Log("LongHit! " + hitOrMiss);
    }

    //Note Object references this when It is missed
    public void NoteMissed()
    {
        //Debug.Log("Miss");

        //Setting stuff and remove Combo
        NoteHitEffects(HitText.Miss);

        UpdateScoreBoard();
    }

    //Manages Text stuff
    public void UpdateScoreBoard()
    {
        //Sets Combo
        for (int i = comboMultiplierInterval.Length - 1; i >= 0; i--)
        {
            if (combo >= comboMultiplierInterval[i])
            {
                currentMultiplier = (int)Mathf.Pow(2, i);

                break;
            }
        }

        //Updates Scoreboard + ComboBoard, go for a perfect?
        uiEffects.SmoothNumberIncrease(score);
        comboBoard.text = "Combo: " + combo;

        multiplierText.text = "x" + currentMultiplier;

        cc.UpdateComboCircle(combo, comboMultiplierInterval);
    }

    //Determine how good or bad a hit is
    public HitText Evalute(float value)
    {
        if (Mathf.Abs(value) < 0.09f) {hitVal = HitText.Perfect;} //Remember to Test Rates
        else if (Mathf.Abs(value) < 0.20f) {hitVal = HitText.Great;}
        else if (Mathf.Abs(value) < 0.35f) {hitVal = HitText.Good;}
        else if (Mathf.Abs(value) < 0.65f) {hitVal = HitText.Meh;}
        else if (value > 0f) {hitVal = HitText.Early;}
        else if (value < 0f) {hitVal = HitText.Late;}

        baseNoteScore = noteScoring[(int)hitVal];

        return hitVal;
    }

    //Manages the effects of hitting a note
    public void NoteHitEffects(HitText hitValue)
    {
        hitTextDisplay.sprite = hitTextSprites[(int)hitValue];

        if ((int)hitValue >= 4)
        {
            uiEffects.BounceGameObj(hitTextDisplay.gameObject, false);

            uiEffects.ComboBreakEffect(uiEffects.transform.position, combo);

            ParticleManager.instance.PlayBreak(combo > comboMultiplierInterval[1], combo);

            combo = 0;
        }
        else
        {
            uiEffects.BounceGameObj(hitTextDisplay.gameObject, true);

            hits++;
            //Continue the combo
            combo++;

            if (combo > topCombo)
            {
                topCombo = combo;
            }
        }

        hitTypeCount[(int)hitValue]++;
    }
}

//Catogorize the types of Hits
[System.Serializable]
public enum HitText
{
    Perfect = 0,
    Great = 1,
    Good = 2,
    Meh = 3,
    Late = 4,
    Early = 5,
    Miss = 6
}

public enum NoteType
{
    Normal = 0,
    Long = 1,
    LongEnd = 2,
}
