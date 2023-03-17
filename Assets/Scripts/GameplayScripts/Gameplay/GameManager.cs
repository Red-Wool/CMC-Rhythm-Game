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
    public bool hasPlayed;

    private bool paused;

    public AudioSource hitSFX;

    //Other Component
    [Header("Componenet References"), Space(10)]
    public BeatScroller bs;
    public ComboCircle cc;
    public EndScoreboard ec;
    public CameraEffects ce;
    public ShaderManager sm;

    //UI + Basically UI
    [Header("UI"), Space(10)]
    public UIEffects uiEffects;
    public GameObject pauseMenu;

    public TextMeshProUGUI countdownText;

    public TextMeshProUGUI scoreBoard;
    public TextMeshProUGUI comboBoard;
    public TextMeshProUGUI multiplierText;

    public TextMeshProUGUI endName;

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
    private SongFileInfo songInfo;
    private List<NoteObject>[] arrowList;

    //[HideInInspector]
    //public float buttonSize;

    [HideInInspector] public Camera mainCamera;

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

        arrowList = new List<NoteObject>[4];
        for (int i = 0; i < arrowList.Length; i++)
        {
            arrowList[i] = new List<NoteObject>();
        }

        mainCamera = Camera.main;

        hitTypeCount = new int[7];

        UpdateScoreBoard();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (paused)
            {
                DisablePause();
            }
            else if (!gameEnd)
            {
                paused = true;
                pauseMenu.SetActive(true);
                
                music.Pause();
                Time.timeScale = 0;
                return;
            }
        }

        if (paused)
        {
            return;
        }
        //Press a Key to start Playing
        if (!playing && Input.anyKeyDown)
        {
            StartGame();

            playing = true;
            hasPlayed = false;

            

            songInfo = bs.StartGame();
            songInfo.startDelay /= songInfo.bpm / 30;
            songInfo.endPos /= songInfo.bpm / 30;

            endName.text = PlayerPrefs.GetString("MapName");

            try
            {
                AudioClip song = LoadAssetBundle.GetMusic(songInfo.songFileName); //Resources.Load<AudioClip>("Music/" + songInfo.songFileName);
                if (song == null)
                {
                    Debug.LogError("Invalid Song Path: Music/" + songInfo.songFileName);
                }
                else
                {
                    music.clip = song;
                    //if (songInfo.startDelay == 0f)
                    //{
                    music.Play();
                    StartCoroutine(FixTime());
                    hasPlayed = true;
                    countdownText.text = "";
                    //}
                    
                }

            }
            catch
            {
                //Debug.LogError("Invalid Song Path: Music/" + songInfo.songFileName);
            }

            //music.Play();
        }
        else if (playing && !gameEnd) //When Song ends, go display scoreboar and stuff
        {
            if (gameTime >= songInfo.endPos)
            {
                Debug.Log("The End");
                ec.ShowScoreboard(score, topCombo, hitTypeCount, bs.GetTotalNotes(), hits);

                bs.playing = false;

                gameEnd = true;
            }
            /*else if (!hasPlayed && !music.isPlaying && gameTime >= songInfo.startDelay)
            {
                //gameTime += Time.deltaTime;
                music.Play();
                //music.time = gameTime - songInfo.startDelay;
                //bs.SetY(gameTime);

                hasPlayed = true;
                //gameTime = songInfo.startDelay;
            }*/
            else
            {
                //gameTime = music.time;
                gameTime += Time.deltaTime;
            }
        }

    }

    public void DisablePause()
    {
        paused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        music.UnPause();
    }

    public IEnumerator FixTime()
    {
        yield return new WaitForSeconds(.1f);
        gameTime = music.time;
        while (playing && music.isPlaying)
        {
            gameTime = music.time;
            yield return new WaitForSeconds(1f);
        }
    }

    public void StartGame()
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

        arrowList = new List<NoteObject>[4];
        for (int i = 0; i < arrowList.Length; i++)
        {
            arrowList[i] = new List<NoteObject>();
        }

        hitTypeCount = new int[7];

        UpdateScoreBoard();
    }

    public void SetTime(float time)
    {
        music.time = time;
        gameTime = time;
    }

    #region NoteHitMethods
    //Note Object References this when a note is hit
    public void NoteHit(float value, GameObject note)
    {

        //Evalute how good the hit was
        hitVal = Evalute(value);

        //Setting stuff, bounce animation, and add score
        NoteHitEffects(hitVal);

        //Add Score
        score += baseNoteScore * currentMultiplier;

        NoteWasPressed(note.GetComponent<NoteObject>());

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
    public void NoteMissed(NoteObject note)
    {
        //Setting stuff and remove Combo
        NoteHitEffects(HitText.Miss);

        
        NoteWasPressed(note);

        UpdateScoreBoard();
    }

    //Determine how good or bad a hit is
    public HitText Evalute(float value)
    {
        value /= 0.25f;
        //Divide by 4 to get ms
        if (Mathf.Abs(value) < 0.1f) { hitVal = HitText.Perfect; } //2.7 Frames //Remember to Test Rates //.025
        else if (Mathf.Abs(value) < 0.22f) { hitVal = HitText.Great; } //6 Frames //.055
        else if (Mathf.Abs(value) < 0.42f) { hitVal = HitText.Good; } //10 Frames //.105
        else if (Mathf.Abs(value) < 0.7f) { hitVal = HitText.Meh; } //.175
        else if (value > 0f) { hitVal = HitText.Early; }
        else if (value < 0f) { hitVal = HitText.Late; }

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

            uiEffects.ComboBreakEffect(combo);

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
    #endregion

    #region MultiNoteRecognitionSystem
    public bool NoteCanBePressed(NoteObject note)
    {
        //Debug.Log(note.name);
        arrowList[(int)note.GetNoteColor()].Add(note);
        if (arrowList[(int)note.GetNoteColor()].Count == 1)
        {
            return true;
        }

        return false;
    }

    private void NoteWasPressed(NoteObject note)
    {
        if (note != null)
        {
            arrowList[(int)note.GetNoteColor()].Remove(note);
            EnableOtherNote((int)note.GetNoteColor());
        }
    }

    private void EnableOtherNote(int col)
    {
        if (arrowList[col].Count > 0)
        {
            NoteObject result = arrowList[col][0];
            float counter = result.yVal;
            for (int i = 1; i < arrowList[col].Count; i++)
            {
                if (arrowList[col][i].yVal < counter)
                {
                    result = arrowList[col][i];
                    counter = result.yVal;
                }
            }

            StartCoroutine("WaitOneFrame", result);
            //result.ActivateArrow();
        }
    }

    IEnumerator WaitOneFrame(NoteObject result)
    {
        yield return 0;
        result.ActivateArrow();
    }
    #endregion
    //Manages Text st    
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
