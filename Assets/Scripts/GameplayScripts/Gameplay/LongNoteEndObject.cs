using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LongNoteEndObject : NoteClass
{
    //[SerializeField]
    private bool valid;
    private bool canBePressed;

    // Start is called before the first frame update
    void Start()
    {
        canBePressed = false;
        valid = false;
    }

    public override NoteType GetNoteType { get { return NoteType.LongEnd; } }

    // Update is called once per frame
    void Update()
    {
        if (yVal != -1)
        {
            eval = yVal - GameManager.instance.GameTime;
            transform.localPosition = button.SetPosition(eval);

            if (valid && (Input.GetKeyUp(keyPress) || Input.GetKeyUp(altKeyPress)))
            {
                gameObject.SetActive(false);

                valid = false;

                GameManager.instance.NoteHit(eval, gameObject);
            }
            else if (!valid && Mathf.Abs(eval) < 0.25f)
            {
                valid = true;
            }
            else if (valid && eval < -0.25f)
            {
                valid = false;

                Debug.Log("This Miss");

                transform.DOScale(0, .1f).OnComplete(DisableArrow);
                GameManager.instance.NoteMissed(null);
            }
        }
    }

    private void DisableArrow()
    {
        gameObject.SetActive(false);
        yVal = -1;
    }
}
