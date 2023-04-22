using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Image gradeImage;
    [SerializeField] private GameObject beatDev;

    public void SetUp(int score, Sprite grade, bool dev)
    {
        scoreText.text = score.ToString();
        gradeImage.sprite = grade;
        beatDev.SetActive(dev);
    }
}
