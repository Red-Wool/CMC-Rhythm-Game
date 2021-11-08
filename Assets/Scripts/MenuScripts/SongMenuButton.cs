using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Script for buttons that show the song in the song select menu
[RequireComponent(typeof(Button))]
public class SongMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private AnimationCurve sizeChange;
    [SerializeField] //Remember to remove this later
    private Song songData;
    //private Button button;

    private bool flag;
    private float timer;

    private const float animSpdUp = 3f;
    private const float animSpdDown = -1.5f;

    // Start is called before the first frame update
    void Start()
    {
        //button = GetComponent<Button>();
        flag = true;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * (flag ? animSpdDown : animSpdUp);
        transform.localScale = Vector3.one * sizeChange.Evaluate(timer);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        flag = false;
        if (timer < 0f) { timer = 0f; }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        flag = true;
        if (timer > 1f) { timer = 1f; }
    }

    //private float ReverseByYX()
    //{
    //    sizeChange.Evaluate(timer)
    //}

    public void SetupButton(Song data)
    {
        songData = data;
    }

    public void ButtonPress()
    {
        MenuManager.instance.UpdateInfoText(songData);
    }


}
