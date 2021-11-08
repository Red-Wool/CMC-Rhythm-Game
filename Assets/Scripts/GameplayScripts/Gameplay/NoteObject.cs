using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : NoteClass
{
    [SerializeField]
    private bool canBePressed;
    [SerializeField]
    private bool isLongNote;
    [SerializeField]
    private float length;

    private ButtonController buttonController;

    //private bool holding;

    public override NoteType GetNoteType { get { return NoteType.Normal; } }

    // Start is called before the first frame update
    void Start()
    {
        buttonController = null;

        //CodeToSaveLater: For Instatiating the long note part, which will be done in beatScroller, which will fix the length and have less references
        /*if (isLongNote && length > 0f)
        {
            int counter = 0;

            while (counter < length)
            {

            }
        }*/
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canBePressed && (Input.GetKeyDown(keyPress) || Input.GetKeyDown(altKeyPress)))
        {
            gameObject.SetActive(false);

            buttonController.avalible = true;

            GameManager.instance.NoteHit(6f + this.transform.position.y, this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
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
    }

    public float CheckIfLongNote()
    {
        if (isLongNote)
        {
            return length;
        }

        return 0f;
    }
}
