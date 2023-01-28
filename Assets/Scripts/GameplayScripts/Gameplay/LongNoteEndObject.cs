using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LongNoteEndObject : NoteClass
{
    [SerializeField]
    private bool valid;

    // Start is called before the first frame update
    void Start()
    {
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
            else if (!valid && Mathf.Abs(eval) < 0.25f && (Input.GetKey(keyPress) || Input.GetKey(altKeyPress)))
            {
                valid = true;
            }
            else if (!valid && eval < -0.25f)
            {
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
