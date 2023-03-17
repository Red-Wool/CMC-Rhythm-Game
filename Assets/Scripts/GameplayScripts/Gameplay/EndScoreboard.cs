using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

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
    private GameObject challenge;

    [SerializeField]
    private Texture[] gradeTextures;

    private int[] hitTypeVal;

    void Start()
    {
        challenge.SetActive(PlayerPrefs.GetInt("Challenge") == 1);
    }

    // Update is called once per frame
    void Update()
    {
        starObjSupport.transform.localScale = starObj.transform.localScale;
        starObjSupport.transform.rotation = starObj.transform.rotation;
    }

    public void ShowScoreboard(int score, int topCombo, int[] hitTypeCount, int totalNotes, int hitNotes)
    {
        //starObj.GetComponent<Animator>().SetTrigger("Trigger");
        starObj.transform.localScale = Vector3.zero;
        starObj.transform.DOScale(100,1.7f).SetEase(Ease.InOutSine);
        starObj.transform.DORotate(Vector3.forward * 360, 1.6f, RotateMode.LocalAxisAdd);

        completeSFX.Play();

        hitTypeVal = hitTypeCount;

        for (int i = 1; i < 8; i++)
        {
            finalScoreboardObjs[i].GetComponent<TextMeshProUGUI>().text += " " + hitTypeVal[i - 1];
        }
        Debug.Log(hitNotes + " " + totalNotes);

        float accuracy = hitNotes / (float)totalNotes;

        finalScoreboardObjs[8].GetComponent<TextMeshProUGUI>().text += " " + (accuracy * 100f).ToString("#.00") + "%";

        finalScoreboardObjs[finalScoreboardObjs.Length - 3].GetComponent<TextMeshProUGUI>().text += " " + score;
        finalScoreboardObjs[finalScoreboardObjs.Length - 2].GetComponent<TextMeshProUGUI>().text += " " + topCombo;

        Texture texture;

        if (accuracy > 0.98f)
        {
            texture = gradeTextures[0];
        }
        else if (accuracy > 0.90f)
        {
            texture = gradeTextures[1];
        }
        else if (accuracy > 0.80f)
        {
            texture = gradeTextures[2];
        }
        else if (accuracy > 0.70f)
        {
            texture = gradeTextures[3];
        }
        else if (accuracy > 0.6f)
        {
            texture = gradeTextures[4];
        }
        else if (accuracy == 0)
        {
            texture = gradeTextures[6];
        }
        else
        {
            texture = gradeTextures[5];
        }

        finalScoreboardObjs[finalScoreboardObjs.Length - 4].GetComponent<RawImage>().texture = texture;

        StartCoroutine("DisplayScoreboard");
    }

    private IEnumerator DisplayScoreboard()
    {
        yield return new WaitForSecondsRealtime(2f);

        for (int i = 0; i < finalScoreboardObjs.Length - 4; i++)
        {
            GetComponent<GameManager>().hitSFX.Play();
            finalScoreboardObjs[i].gameObject.SetActive(true);
            


            yield return new WaitForSecondsRealtime(0.2f);
        }
        yield return new WaitForSecondsRealtime(0.8f);

        for (int i = finalScoreboardObjs.Length - 5; i < finalScoreboardObjs.Length; i++)
        {
            finalScoreboardObjs[i].gameObject.SetActive(true);
        }

        
    } 
}
