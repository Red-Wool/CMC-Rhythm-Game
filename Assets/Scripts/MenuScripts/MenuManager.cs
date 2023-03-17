using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    //Reference for everyone
    [HideInInspector]
    public static MenuManager instance;

    [SerializeField] private Album album;

    [SerializeField] private Button playButton;
    [SerializeField] private Toggle challengeToggle;
    [SerializeField] private RectTransform keybindPage;
    [SerializeField] private RectTransform target;

    [SerializeField] private GameObject songContainer;
    [SerializeField] private GameObject songPrefab;

    [SerializeField]
    private TextMeshProUGUI infoText;

    private Song currentMap;
    private bool challengeMode;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        SetUpSongList(album);
        
    }

    public void MoveKeyBind(bool flag)
    {
        keybindPage.DOMoveX(flag ? target.transform.position.x : 5000, .5f);
    }
    
    public void SetUpSongList(Album album)
    {
        PlayerPrefs.SetString("CurrentMap", album.songList[0].mapName);
        for (int i = 0; i < album.songList.Length; i++)
        {
            Instantiate(songPrefab, songContainer.transform).GetComponent<SongMenuButton>().SetupButton(album.songList[i]);

        }
    }

    public void UpdateInfoText(Song song)
    {
        playButton.interactable = true;

        currentMap = song;
        string mapName = (challengeMode ? song.challengeMap : song.mapName);
        if (mapName == "")
            mapName = song.mapName;
        PlayerPrefs.SetString("CurrentMap", mapName);
        PlayerPrefs.SetString("MapName", song.songName);
        infoText.text = song.songName + "\nBy: " + song.artist + "\n" + song.description;
    }

    public void ChangeChallenge()
    {
        if (currentMap == null)
        {
            return;
        }
        challengeMode = challengeToggle.isOn;

        string mapName = (challengeMode ? currentMap.challengeMap : currentMap.mapName);
        if (mapName == "")
            mapName = currentMap.mapName;
        PlayerPrefs.SetString("CurrentMap", mapName);
    }
}

/*[System.Serializable]
public struct Album
{
    public string name;
    public Texture coverArt;
    public Song[] songList;
}*/

/*[System.Serializable]
public struct Song
{
    public string name;
    public string artist;
    public string description;
    public Texture songArt;
    public Texture songFrameArt;
}*/
