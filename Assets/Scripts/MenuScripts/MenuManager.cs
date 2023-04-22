using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private SaveDataScriptableObject saveFile;

    //Reference for everyone
    [HideInInspector]
    public static MenuManager instance;

    [SerializeField] private Vector3 movePos;

    [SerializeField] private Vector3 infoDefaultPos;
    [SerializeField] private Vector3 infoSongPos;
    [SerializeField] private Vector3 infoPlayPos;

    [SerializeField] private Album[] albumList;

    [SerializeField] private RectTransform albumDisplay;
    [SerializeField] private RectTransform songListDisplay;
    [SerializeField] private RectTransform infoDisplay;
    [SerializeField] private Button exitAlbumButton;

    [SerializeField] private StarTransition starTransition;

    [SerializeField] private TMP_Text leaderboardTitle;
    [SerializeField] private Transform leaderboardContainer;
    [SerializeField] private Sprite[] leaderBoardGradeSprites;
    [SerializeField] private GameObject leaderboardPrefab;

    [SerializeField] private Button playButton;
    [SerializeField] private Toggle challengeToggle;
    [SerializeField] private Toggle speedToggle;
    [SerializeField] private TMP_Text devScore;
    [SerializeField] private RectTransform keybindPage;
    [SerializeField] private RectTransform target;

    [SerializeField] private GameObject songContainer;
    [SerializeField] private GameObject songPrefab;
    [SerializeField] private GameObject albumContainer;
    [SerializeField] private GameObject albumPrefab;

    [SerializeField]
    private TextMeshProUGUI infoText;
    [SerializeField]
    private RawImage infoIcon;

    [SerializeField] private GameObject loadingText;

    private Album currentAlbum;
    private Song currentMap;
    private bool challengeMode;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        //SetUpSongList(album);
        infoIcon.enabled = false;

        starTransition.CloseStar(.8f, .1f);

        saveFile.save = (SaveDataObject)SaveManager.Load("FishballKite");
        if (saveFile.save == null)
        {
            SaveDataObject save = new SaveDataObject();
            save.scoreData = new Dictionary<LevelType, List<LevelScoreData>>();
            SaveManager.Save("FishballKite", save);
            saveFile.save = save;
        }

        challengeToggle.isOn = PlayerPrefs.GetInt("Challenge") == 1;
        speedToggle.isOn = PlayerPrefs.GetFloat("Speed") > 1.2f;

        SetUpAlbumList();
    }

    public void MoveKeyBind(bool flag)
    {
        keybindPage.DOMoveX(flag ? target.transform.position.x : 5000, .5f);
    }
    
    public void SetUpAlbumList()
    {
        FocusAlbumList();

        for (int i = 0; i < albumList.Length; i++)
        {
            Instantiate(albumPrefab, albumContainer.transform).GetComponent<SongMenuButton>().SetupButton(albumList[i]);
            if (albumList[i].albumName == PlayerPrefs.GetString("AlbumName"))
            {
                SetUpSongList(albumList[i]);
            }
        }
    }

    public void FocusAlbumList()
    {
        exitAlbumButton.interactable = false;
        playButton.interactable = false;

        infoIcon.enabled = false;
        infoText.text = "Select an album!";

        songListDisplay.DOAnchorPos(movePos, .5f);
        albumDisplay.DOAnchorPos(Vector3.zero, .5f);
        infoDisplay.DOAnchorPos(infoDefaultPos, 1f).SetEase(Ease.OutBounce);

        GameObject[] childList = new GameObject[leaderboardContainer.transform.childCount];
        for (int i = 0; i < childList.Length; i++)
        {
            childList[i] = leaderboardContainer.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < childList.Length; i++)
        {
            Destroy(childList[i]);
        }

        leaderboardTitle.text = "Cadence!";
        devScore.enabled = false;
    }

    public void SetUpSongList(Album album)
    {
        currentAlbum = album;

        infoText.text = "Select a song!";

        infoDisplay.DOAnchorPos(infoSongPos, 1f).SetEase(Ease.OutBounce);
        exitAlbumButton.interactable = true;
        songListDisplay.DOAnchorPos(Vector3.zero, .5f);
        albumDisplay.DOAnchorPos(movePos, .5f);

        GameObject[] childList = new GameObject[songContainer.transform.childCount];
        for (int i = 0; i < childList.Length; i++)
        {
            childList[i] = songContainer.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < childList.Length; i++)
        {
            Destroy(childList[i]);
        }

        PlayerPrefs.SetString("CurrentMap", album.songList[0].mapName);
        for (int i = 0; i < album.songList.Length; i++)
        {
            Instantiate(songPrefab, songContainer.transform).GetComponent<SongMenuButton>().SetupButton(album.songList[i]);

        }
    }

    public void UpdateInfoText(Song song)
    {
        infoDisplay.DOAnchorPos(infoPlayPos, 1f).SetEase(Ease.OutBounce);
        playButton.interactable = true;

        currentMap = song;
        string mapName = (challengeMode ? song.challengeMap : song.mapName);
        if (mapName == "")
            mapName = song.mapName;

        challengeToggle.gameObject.SetActive(song.hasChallenge);

        PlayerPrefs.SetString("CurrentMap", mapName);
        PlayerPrefs.SetString("AlbumName", currentAlbum.albumName);
        PlayerPrefs.SetString("MapName", song.songName);
        PlayerPrefs.SetInt("Challenge", challengeMode ? 1 : 0);
        PlayerPrefs.SetInt("DevScore", song.devScore);

        SetUpLeaderboard();

        infoText.text = song.songName + "\n<size=16>" + song.artist + "\n" + song.description;
        infoIcon.enabled = true;
        infoIcon.texture = song.songArt;
    }

    public void ChangeChallenge()
    {
        challengeMode = challengeToggle.isOn;
        if (currentMap == null)
        {
            return;
        }
        

        string mapName = (challengeMode ? currentMap.challengeMap : currentMap.mapName);
        if (mapName == "")
            mapName = currentMap.mapName;
        PlayerPrefs.SetString("CurrentMap", mapName);
        PlayerPrefs.SetInt("Challenge", challengeMode ? 1 : 0);

        SetUpLeaderboard();
    }

    public void ChangeSpeed()
    {
        PlayerPrefs.SetFloat("Speed", speedToggle.isOn ? 1.5f : 1f);
        SetUpLeaderboard();
    }

    public void SetUpLeaderboard()
    {
        string mapName = PlayerPrefs.GetString("CurrentMap");
        float speed = PlayerPrefs.GetFloat("Speed");

        GameObject[] childList = new GameObject[leaderboardContainer.transform.childCount];
        for (int i = 0; i < childList.Length; i++)
        {
            childList[i] = leaderboardContainer.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < childList.Length; i++)
        {
            Destroy(childList[i]);
        }

        List<LevelScoreData> scoreDatas = null;

        Dictionary<LevelType, List<LevelScoreData>> map = saveFile.save.scoreData;
        foreach(var mapPair in map)
        {
            if (mapPair.Key.Equal(mapName, speed))
            {
                scoreDatas = mapPair.Value;
                break;
            }
        }

        devScore.enabled = false;
        if (scoreDatas == null)
        {
            leaderboardTitle.text = "No Scores Set...";
            return;
        }

        leaderboardTitle.text = PlayerPrefs.GetString("MapName");
        LevelScoreData score;
        for (int i = 0; i < scoreDatas.Count; i++)
        {
            score = scoreDatas[i];
            GameObject obj = Instantiate(leaderboardPrefab, leaderboardContainer);
            obj.GetComponent<LeaderboardEntry>().SetUp(score.score, leaderBoardGradeSprites[(int)score.grade], score.beatDev);
            obj.transform.SetAsLastSibling();
        }
        if (scoreDatas[0].grade <= ScoreGrade.S)
        {
            devScore.enabled = true;
            if (currentMap != null)
                devScore.text = "Dev Score: " + currentMap.devScore;
        }
        
    }

    public void StartLevel()
    {
        loadingText.SetActive(true);
        playButton.interactable = false;
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
