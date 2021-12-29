using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEffects : MonoBehaviour
{
    //Varible Declaration
    [Header("UI Bounce")]
    public GameObject bouncingGameObj;

    [Space(10)]
    public AnimationCurve gameObjBounceHit;
    public AnimationCurve gameObjBounceMiss;

    [Space(10)]
    public TextMeshProUGUI scoreBoard;
    public TextMeshProUGUI comboBoard;

    public AnimationCurve scoreBoardBounceAmount;
    private float baseValScore;
    private float baseValCombo;

    //True for hit, false for Miss, huh
    private bool hitOrMiss;

    private float bounceTimer;

    [Header("UI Wobble"), Space(10)]
    public GameObject textObj;
    private TMP_Text textMesh;

    [Header("Combo Break"), Space(10)]
    public GameObject comboBreakPopupParent;
    public GameObject comboBreakPopupPrefab;
    [Space(10)]
    public Vector2 cbpRandomIntesity;

    private List<GameObject> comboBreakPopupPool = new List<GameObject>();

    [Header("Smooth Score Increase"), Space(10)]


    //[Range(0f, 1f)]
    //public float smoothSpeed;
    public AnimationCurve smoothSpeed;

    private float smoothScoreTimer;
    private int startScore;
    private int smoothScore;
    private int targetScore;


    //Extra Varibles declared only once to save memory
    private Mesh mesh;
    private Vector3[] vertices;

    private Vector3 bounceVal;
    private Vector3 bounceValScore;
    private Vector3 offset;

    private GameObject cbpObj;

    // Start is called before the first frame update
    void Start()
    {
        //eee
        bounceTimer = 5f;

        //Get the TMP_Text component to edit the mesh and crap
        textMesh = textObj.GetComponent<TMP_Text>();

        //Add objects to pool to ruduce lag/garbo collect crash
        for (int i = 0; i < 10; i++)
        {
            CreateComboBreakEffect();
        }

        //Set Varibles
        smoothScore = 0;
        targetScore = 0;

        baseValScore = scoreBoard.gameObject.transform.position.y;
        baseValCombo = comboBoard.gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        //Button button;
        //bool isGood = button.interactable;

        //Hit Text Update Effects + Scoreboards
        if (bounceTimer < 1f)
        {
            //HitTextStuff
            bounceTimer += Time.deltaTime;

            if (hitOrMiss)
            {
                bounceVal.y = gameObjBounceHit.Evaluate(bounceTimer);
            }
            else
            {
                bounceVal.x = gameObjBounceMiss.Evaluate(bounceTimer);
            }
            bouncingGameObj.transform.localScale = bounceVal;

            //ScoreboardSTUFF

            //bounceValScore = scoreBoard.transform.position;
            //bounceValScore.y = baseValScore + scoreBoardBounceAmount.Evaluate(bounceTimer);
            //scoreBoard.transform.position = bounceValScore;

            bounceValScore = comboBoard.transform.position;
            bounceValScore.y = baseValCombo + scoreBoardBounceAmount.Evaluate(bounceTimer);
            comboBoard.transform.position = bounceValScore;


        }

        //Text UI Effects
        textMesh.ForceMeshUpdate();
        mesh = textMesh.mesh;
        vertices = mesh.vertices;
        
        for (int i = 0; i < textMesh.textInfo.characterCount; i++)
        {
            TMP_CharacterInfo c = textMesh.textInfo.characterInfo[i];

            int index = c.vertexIndex;

            offset = WobbleOffset(vertices[i], Time.time + i);

            for (int j = 0; j < 4; j++)
            {
                vertices[index + j] = vertices[index + j] + offset;
            }

            
        }

        mesh.vertices = vertices;

        textMesh.canvasRenderer.SetMesh(mesh);

        //Smooth Score Increase
        if (smoothScore != targetScore)
        {
            smoothScoreTimer += Time.deltaTime;

            smoothScore = (int)Mathf.SmoothStep(startScore, targetScore, smoothSpeed.Evaluate(smoothScoreTimer));

            //Mathf.l

            scoreBoard.text = "Score: " + smoothScore;
        }

        //boun
    }

    //To Start Animation and stuff for the Hit Text
    public void BounceGameObj (GameObject gameObj, bool hitOrMiss)
    {
        bouncingGameObj = gameObj;
        bounceVal = Vector3.one;

        this.hitOrMiss = hitOrMiss;

        bounceTimer = 0f;
    }

    //Method for UI Text Effectd
    public Vector2 WobbleOffset(Vector3 pos, float time)
    {
        //return new Vector2(0, Mathf.Sin(pos.y + time) * 30);
        float power = ((Mathf.Cos((2f / 3f) * time * 1) + 1) / 4f) + 0.5f;
        return new Vector2(Mathf.Cos(time * 1f) * 1f * power, Mathf.Sin(time * 1f) * 1f * power);    //Mathf.Clamp(Mathf.Cos(time * 5f)*10, -6f, 6f), Mathf.Clamp(Mathf.Sin(time * 5f)*10, -6f, 6f));
    }

    //To Start Animation for breaking a combo
    public void ComboBreakEffect(Vector3 pos, int combo)
    {
        //Debug.Log("Failing ez 0 to deathg neabgr");
        cbpObj = null;

        for (int i = 0; i < comboBreakPopupPool.Count; i++)
        {
            if (!comboBreakPopupPool[i].activeInHierarchy)
            { 
                cbpObj = comboBreakPopupPool[i];

                break;
            }
        }

        if (cbpObj == null)
        {
            CreateComboBreakEffect();
        }

        cbpObj.transform.position = comboBreakPopupParent.transform.position;

        cbpObj.GetComponent<TextMeshProUGUI>().text = combo.ToString();


        cbpObj.GetComponent<ComboBreakParticle>().Spawn(new Vector2(
            Random.Range(-cbpRandomIntesity.x, cbpRandomIntesity.x),
            cbpRandomIntesity.y));
    }

    //Add a combo Break Effect to the pool
    public void CreateComboBreakEffect()
    {
        cbpObj = Instantiate(comboBreakPopupPrefab, comboBreakPopupParent.transform) as GameObject;

        cbpObj.SetActive(false);

        comboBreakPopupPool.Add(cbpObj);
    }

    //Increment smoothly to a number for the score
    public void SmoothNumberIncrease(int score)
    {
        startScore = smoothScore;

        if (smoothScore == targetScore)
        {
            smoothScoreTimer = 0f;
        }

        //if (interrupt)
        //{
        smoothScoreTimer = Mathf.Max(Mathf.Min(smoothScoreTimer, 0.8f) - 0.3f, 0f);
        //}

        targetScore = score;
    }

}
