using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SongEditorAudioPreview : MonoBehaviour
{
    [SerializeField] private GameObject judgementLine;
    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private SongComplier compiler;
    [SerializeField] private TMP_InputField previewTimeInput;

    [SerializeField] private Slider songSpeed;
    [SerializeField] private TMP_Text songSpeedDisplay;

    [SerializeField] private TMP_InputField delayInput;
    private float delayTime;

    private const string validNum = "0123456789.";

    private bool isPlaying;
    private float scrollSpeed;
    private float playSpeed;

    private void Start()
    {
        previewTimeInput.onValidateInput = (string text, int charIndex, char addedChar) => { return ValidateCharacter(validNum, addedChar); };
        previewTimeInput.characterLimit = 10;

        songSpeed.value = 10f;
        UpdateSongSpeedDisplay();
    }
    private char ValidateCharacter(string validCharacters, char addedChr)
    {
        if (validCharacters.IndexOf(addedChr) != -1)
        {
            return addedChr;
        }
        else
        {
            Debug.Log("Nonvalid!");
            return '\0'; //Null Character
        }

    }

    private void Update()
    {
        if (isPlaying)
        {
            judgementLine.transform.position = Vector3.up * (audioPlayer.time * scrollSpeed + delayTime);//+= Vector3.up * scrollSpeed * playSpeed * Time.deltaTime;
            Camera.main.transform.position = judgementLine.transform.position + Vector3.back * 10;
        }
    }

    private void TryPlaySong(float time)
    {
        try
        {
            AudioClip music = LoadAssetBundle.GetMusic(compiler.SongFileName); //Resources.Load<AudioClip>("Music/" + compiler.SongFileName);
            if (music == null)
            {
                Debug.LogError("Invalid Song Path: Music/" + compiler.SongFileName);
            }
            else
            {
                UpdateSongSpeedDisplay();

                audioPlayer.clip = music;
                audioPlayer.pitch = playSpeed;
                audioPlayer.Play();
                audioPlayer.time = time;
            }
            
        }
        catch
        {
            Debug.LogError("Invalid Song Path: Music/" + compiler.SongFileName);
        }
    }

    private void SetUpJudgementBar(float pos)
    {
        judgementLine.transform.position = Vector3.up * pos;
        judgementLine.SetActive(true);
        delayTime = float.Parse(delayInput.text);
    }

    public void PlaySnippet()
    {
        TryPlaySong(0);
        StopCoroutine("StopMusic");
        StartCoroutine("StopMusic");

        isPlaying = false;
    }

    private IEnumerator StopMusic()
    {
        yield return new WaitForSeconds(5f);
        audioPlayer.Stop();
    }

    public void UpdateSongSpeedDisplay()
    {
        playSpeed = songSpeed.value / 10f;
        songSpeedDisplay.text = "x" + playSpeed;
    }

    public void PreviewBar()
    {
        isPlaying = true;

        float bars = float.Parse(previewTimeInput.text);
        scrollSpeed = float.Parse(compiler.BPM) / 30;

        TryPlaySong(bars / scrollSpeed);
        SetUpJudgementBar(bars);
    } //120 bpm 1 mult = 4bars 1 sec (bpm / 30 = barps) (sec = bars / barps) (sec * barps = bars)

    public void PreviewTime()
    {
        isPlaying = true;

        float sec = float.Parse(previewTimeInput.text);
        scrollSpeed = float.Parse(compiler.BPM) / 30;

        TryPlaySong(sec);
        SetUpJudgementBar(sec * scrollSpeed);
    }

    public void StopPreview()
    {
        audioPlayer.Stop();
        StopCoroutine("StopMusic");
        judgementLine.SetActive(false);

        isPlaying = false;
    }
}
