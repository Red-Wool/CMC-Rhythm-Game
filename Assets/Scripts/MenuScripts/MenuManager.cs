using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    //Reference for everyone
    [HideInInspector]
    public static MenuManager instance;

    public Album[] album;

    [SerializeField]
    private TextMeshProUGUI infoText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void UpdateInfoText(Song song)
    {
        infoText.text = song.name + "\nBy: " + song.artist + "\n" + song.description;
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
