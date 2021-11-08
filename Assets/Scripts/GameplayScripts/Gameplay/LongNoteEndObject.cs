using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNoteEndObject : NoteClass
{
    [SerializeField]
    private bool valid;

    //public KeyCode keyPress;
    //public KeyCode altKeyPress;

    //public NoteColor noteCol;

    // Start is called before the first frame update
    void Start()
    {
        valid = false;
    }

    public override NoteType GetNoteType { get { return NoteType.LongEnd; } }

    // Update is called once per frame
    void Update()
    {
        if (valid && (Input.GetKeyUp(keyPress) || Input.GetKeyUp(altKeyPress)))
        {
            valid = false;

            gameObject.SetActive(false);

            GameManager.instance.NoteHit(6f + this.transform.position.y, this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Activator")
        {
            if ((Input.GetKey(keyPress) || Input.GetKey(altKeyPress)))
            {
                valid = true;

                //GetComponent<SpriteRenderer>().sprite = null;

            }
        }
    }
}
