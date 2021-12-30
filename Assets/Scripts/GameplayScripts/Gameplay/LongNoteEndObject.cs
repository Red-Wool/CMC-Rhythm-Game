using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (valid && (Input.GetKeyUp(keyPress) || Input.GetKeyUp(altKeyPress)))
            {
                gameObject.SetActive(false);

                valid = false;

                GameManager.instance.NoteHit(eval, this.gameObject);
            }
            else if (!valid && Mathf.Abs(eval) < 0.25f && (Input.GetKey(keyPress) || Input.GetKey(altKeyPress)))
            {
                valid = true;
            }
            else if (!valid && eval < -0.25f)
            {
                yVal = -1;
                GameManager.instance.NoteMissed(null);
            }
        }
        /*if (valid && (Input.GetKeyUp(keyPress) || Input.GetKeyUp(altKeyPress)))
        {
            valid = false;

            gameObject.SetActive(false);

            GameManager.instance.NoteHit(6f + this.transform.position.y, this.gameObject);
        }*/
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Activator")
        {
            if ((Input.GetKey(keyPress) || Input.GetKey(altKeyPress)))
            {
                valid = true;

                //GetComponent<SpriteRenderer>().sprite = null;

            }
        }
    }*/
}
