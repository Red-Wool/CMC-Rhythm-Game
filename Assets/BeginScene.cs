using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BeginScene : MonoBehaviour
{
    public StarTransition star;
    public TMP_Text disclaimer;
    public Color bgColor;
    public Image disclaimerBG;

    private IEnumerator startCutscene;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetString("AlbumName", "");
        PlayerPrefs.SetInt("Challenge", 0);
        PlayerPrefs.SetFloat("Speed", 1f);
        disclaimerBG.color = Color.black;
        startCutscene = Cutscene();
        StartCoroutine(startCutscene);
    }

    private void Update()
    {
        /*if (startCutscene != null && Input.anyKeyDown)
        {
            StopCoroutine(startCutscene);
        }*/
    }


    public IEnumerator Cutscene()
    {
        yield return new WaitForSeconds(.5f);
        disclaimerBG.DOColor(bgColor, .5f);
        yield return new WaitForSeconds(5f);
        disclaimer.DOFade(0, .5f);
        yield return new WaitForSeconds(.5f);
        star.CloseStar(1.6f,.1f);
    }
}
