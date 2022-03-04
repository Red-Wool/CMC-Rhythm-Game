using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    //Reference for everyone
    [HideInInspector]
    public static MenuManager instance;

    [SerializeField] private Album album;

    [SerializeField] private GameObject songContainer;
    [SerializeField] private GameObject songPrefab;

    [SerializeField]
    private TextMeshProUGUI infoText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        SetUpSongList(album);
        
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
        PlayerPrefs.SetString("CurrentMap", song.mapName);
        PlayerPrefs.SetString("MapName", song.songName);
        infoText.text = song.songName + "\nBy: " + song.artist + "\n" + song.description;
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
