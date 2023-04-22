using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class EndScoreboard : MonoBehaviour
{
    [SerializeField] private SaveDataScriptableObject saveData;

    [SerializeField]
    private AudioSource completeSFX;
    [SerializeField]
    private AudioSource hitSFX;

    [SerializeField]
    private GameObject[] finalScoreboardObjs;
    [SerializeField]
    private StarTransition starObj;
    [SerializeField]
    private GameObject starObjSupport;
    [SerializeField]
    private GameObject challenge;
    [SerializeField]
    private GameObject speed;
    [SerializeField]
    private GameObject devScore;

    [SerializeField]
    private Sprite[] gradeTextures;
    [SerializeField] private Sprite allMiss;
    [SerializeField] private Sprite pausing;

    private int[] hitTypeVal;

    void Start()
    {
        starObj.CloseStar(.5f,.01f);
        challenge.SetActive(PlayerPrefs.GetInt("Challenge") == 1);
        speed.SetActive(PlayerPrefs.GetFloat("Speed") >= 1.2f);
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
        /*
        starObj.transform.localScale = Vector3.zero;
        starObj.transform.DOScale(100,1.7f).SetEase(Ease.InOutSine);
        starObj.transform.DORotate(Vector3.forward * 360, 1.6f, RotateMode.LocalAxisAdd);
        */

        starObj.OpenStar(1.6f,.1f);

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

        Sprite texture = null;
        ScoreGrade grade;

        if (accuracy == 0f)
        {
            grade = ScoreGrade.Cang;
            texture = allMiss;
        }
        else if (PlayerPrefs.GetInt("PauseNum") >= 5)
        {
            grade = ScoreGrade.Paused;
            texture = pausing;
        }
        else if (accuracy >= 1f)
        {
            grade = ScoreGrade.Perfect;
            texture = gradeTextures[0];
        }
        else if (accuracy > 0.98f)
        {
            grade = ScoreGrade.S;
            texture = gradeTextures[1];
        }
        else if (accuracy > 0.90f)
        {
            grade = ScoreGrade.A;
            texture = gradeTextures[2];
        }
        else if (accuracy > 0.80f)
        {
            grade = ScoreGrade.B;
            texture = gradeTextures[3];
        }
        else if (accuracy > 0.70f)
        {
            grade = ScoreGrade.C;
            texture = gradeTextures[4];
        }
        else if (accuracy > 0.6f)
        {
            grade = ScoreGrade.D;
            texture = gradeTextures[5];
        }
        else
        {
            grade = ScoreGrade.F;
            texture = gradeTextures[6];
        }

        devScore.SetActive(score > PlayerPrefs.GetInt("DevScore") && PlayerPrefs.GetInt("PauseNum") < 5);
        SaveScore(score, hitTypeCount, grade, devScore.activeSelf);

        finalScoreboardObjs[finalScoreboardObjs.Length - 4].GetComponent<Image>().sprite = texture;

        StartCoroutine("DisplayScoreboard");
    }

    private void SaveScore(int score, int[] hitCategories, ScoreGrade grade, bool dev)
    {
        LevelScoreData scoreData = new LevelScoreData { score = score, hitCategories = hitCategories, grade = grade, beatDev = dev };
        foreach (var mapPair in saveData.save.scoreData)
        {
            if (mapPair.Key.Equal(PlayerPrefs.GetString("CurrentMap"), PlayerPrefs.GetFloat("Speed")))
            {
                //Too lazy to do binary search rn
                for (int i = 0; i < mapPair.Value.Count; i++)
                {
                    if (mapPair.Value[i].score < score)
                    {
                        mapPair.Value.Insert(i, scoreData);
                        SaveManager.Save("FishballKite", saveData.save);
                        return;
                    }
                }
                mapPair.Value.Add(scoreData);
                SaveManager.Save("FishballKite", saveData.save);
                return;
            }
        }

        LevelType level = new LevelType { levelName = PlayerPrefs.GetString("CurrentMap"), speed = PlayerPrefs.GetFloat("Speed") };
        List<LevelScoreData> newScoreList = new List<LevelScoreData> { scoreData };
        saveData.save.scoreData.Add(level, newScoreList);

        SaveManager.Save("FishballKite", saveData.save);
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
