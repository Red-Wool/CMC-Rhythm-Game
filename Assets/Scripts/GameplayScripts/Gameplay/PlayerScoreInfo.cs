using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class PlayerScoreInfo
{
    public int score;
    public int hit;
    public int combo;
    public int topCombo;

    public int currentMultiplier;
    public TMP_Text scoreText;
    public TMP_Text comboText;
    public TMP_Text multiplierText;
    public ComboCircle comboCircle;
}
