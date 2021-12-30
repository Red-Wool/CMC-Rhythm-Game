using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : NoteClass
{
    [SerializeField]
    private bool canBePressed;

    private bool flag;

    public override NoteType GetNoteType { get { return NoteType.Normal; } }

    // Start is called before the first frame update
    void Start()
    {
        flag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (yVal != -1) 
        {
            eval = yVal - GameManager.instance.GameTime;
            if (canBePressed && (Input.GetKeyDown(keyPress) || Input.GetKeyDown(altKeyPress)))
            {
                gameObject.SetActive(false);

                //buttonController.avalible = true;

                GameManager.instance.NoteHit(eval, this.gameObject);
            }
            else if (!flag && Mathf.Abs(eval) < 0.25f)
            {
                flag = true;
                canBePressed = GameManager.instance.NoteCanBePressed(this);
            }
            else if (flag && eval < -0.25f)
            {
                canBePressed = false;
                flag = false;
                yVal = -1;
                GameManager.instance.NoteMissed(this);
            }
        }
    }

    public void ActivateArrow()
    {
        if (Mathf.Abs(yVal - GameManager.instance.GameTime) < 0.25f)
        {
            canBePressed = true;
        }
    }

    #region Archived
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Activator")
        {
            buttonController = collision.gameObject.GetComponent<ButtonController>();

            if (buttonController.avalible)
            {
                canBePressed = true;

                buttonController.avalible = false;
            }
            

        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!buttonController)
        {
            buttonController = collision.gameObject.GetComponent<ButtonController>();
        }

        if (gameObject.activeSelf && collision.tag == "Activator" && buttonController.avalible)
        {
            canBePressed = true;

            buttonController.avalible = false;

            //Debug.Log("Ot works;");

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Activator" && this.gameObject.activeSelf)
        {
            canBePressed = false;

            GameManager.instance.NoteMissed();

            buttonController.avalible = true;
        }
    }*/

    /*public Note GetNoteData() //Archived as was used for ArrowArchiver, which is also archived
    {
        Note note;

        note.color = GetNoteColor();
        note.yVal = transform.position.y;
        note.isLongNote = isLongNote;
        note.longNoteLen = length;

        return note;
    }*/
    #endregion
}
