using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndScoreboard : MonoBehaviour
{
    [SerializeField]
    private AudioSource completeSFX;
    [SerializeField]
    private AudioSource hitSFX;

    [SerializeField]
    private GameObject[] finalScoreboardObjs;
    [SerializeField]
    private GameObject starObj;
    [SerializeField]
    private GameObject starObjSupport;

    [SerializeField]
    private Texture[] gradeTextures;

    private int[] hitTypeVal;

    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        starObjSupport.transform.localScale = starObj.transform.localScale;
        starObjSupport.transform.rotation = starObj.transform.rotation;
    }

    public void ShowScoreboard(int score, int topCombo, int[] hitTypeCount, int totalNotes, int hitNotes)
    {
        starObj.GetComponent<Animator>().SetTrigger("Trigger");

        completeSFX.Play();

        hitTypeVal = hitTypeCount;

        for (int i = 1; i < 8; i++)
        {
            finalScoreboardObjs[i].GetComponent<TextMeshProUGUI>().text += " " + hitTypeVal[i - 1];
        }
        Debug.Log(hitNotes + " " + totalNotes);

        float accuracy = hitNotes / (float)totalNotes;

        finalScoreboardObjs[8].GetComponent<TextMeshProUGUI>().text += " " + (accuracy * 100f).ToString("#.00") + "%";

        finalScoreboardObjs[finalScoreboardObjs.Length - 2].GetComponent<TextMeshProUGUI>().text += " " + score;
        finalScoreboardObjs[finalScoreboardObjs.Length - 1].GetComponent<TextMeshProUGUI>().text += " " + topCombo;

        Texture texture;

        if (accuracy > 0.98f)
        {
            texture = gradeTextures[0];
        }
        else if (accuracy > 0.90f)
        {
            texture = gradeTextures[1];
        }
        else if (accuracy > 0.85f)
        {
            texture = gradeTextures[2];
        }
        else if (accuracy > 0.75f)
        {
            texture = gradeTextures[3];
        }
        else if (accuracy > 0.6f)
        {
            texture = gradeTextures[4];
        }
        else
        {
            texture = gradeTextures[5];
        }

        finalScoreboardObjs[finalScoreboardObjs.Length - 3].GetComponent<RawImage>().texture = texture;

        StartCoroutine("DisplayScoreboard");
    }

    private IEnumerator DisplayScoreboard()
    {
        yield return new WaitForSecondsRealtime(2f);

        for (int i = 0; i < finalScoreboardObjs.Length - 3; i++)
        {
            GetComponent<GameManager>().hitSFX.Play();
            finalScoreboardObjs[i].gameObject.SetActive(true);
            


            yield return new WaitForSecondsRealtime(0.2f);
        }
        yield return new WaitForSecondsRealtime(0.8f);

        for (int i = finalScoreboardObjs.Length - 3; i < finalScoreboardObjs.Length; i++)
        {
            finalScoreboardObjs[i].gameObject.SetActive(true);
        }

        
    } 
}
