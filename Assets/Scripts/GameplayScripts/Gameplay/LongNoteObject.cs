using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNoteObject : NoteClass
{
    [SerializeField]
    private bool valid;

    private BoxCollider2D hitbox;
    private GameObject parentObj;
    private GameObject arrowButton;
    private Vector3 tempPos;
    private float percent;
    private float tempLength;
    private int intervals;
    private int counter;

    public override NoteType GetNoteType{ get { return NoteType.Long; } }

    // Start is called before the first frame update
    void Start()
    {
        valid = false;

        hitbox = GetComponent<BoxCollider2D>();
        hitbox.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (parentObj && !parentObj.activeSelf && !hitbox.enabled)
        {
            hitbox.enabled = true;

            ParticleToggle(true, true);
        }

        if (valid)
        {
            if (Input.GetKeyUp(keyPress) || Input.GetKeyUp(altKeyPress))
            {
                valid = false;

                if (counter != intervals)
                {
                    MissedLongMiddle();
                }
            }
            else
            {
                HandleLongObject();
            }

        }
        else if (counter != intervals && transform.position.y + (tempLength / 2) < -6f)
        {
            MissedLongMiddle();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Activator")
        {
            arrowButton = collision.gameObject;

            if (Input.GetKey(keyPress) || Input.GetKey(altKeyPress))
            {
                valid = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Activator" && this.gameObject.activeSelf)
        {
            ParticleToggle(false, true);

            if (valid)
            {
                valid = false;
            }
            //gameObject.SetActive(false);

            /*if (!endPart)
            {
                GameManager.instance.LongNoteHit(true);
            }
            //GameManager.instance.NoteMissed();

            //buttonController.avalible = true;*/
        }
    }

    public void LongNoteSetup(GameObject parent, float len, int lenVal)
    {
        parentObj = parent;
        tempLength = len;
        intervals = lenVal;

        counter = 1;
        //length = len / 2f;
    }

    public void MissedLongMiddle()
    {
        //Debug.Log(intervals + " " + counter + " " + (intervals - counter));

        GameManager.instance.LongNoteHit(false, intervals - counter);

        ParticleToggle(false, true);

        GetComponent<LongNoteObject>().enabled = false;
        hitbox.enabled = false;
    }

    public void HandleLongObject() 
    {
        //This Formula tells how much of the long bar is complete Very Good!
        percent = (arrowButton.transform.position.y - parentObj.transform.position.y) / tempLength;

        transform.position = arrowButton.transform.position + Vector3.up * ((1f - percent) * tempLength / 2);

        tempPos = Vector3.one;
        tempPos.y = tempLength / 2 * (1f - percent);

        transform.localScale = tempPos;

        if (percent > counter * (1f / intervals) && counter != intervals)
        {
            counter++;

            GameManager.instance.LongNoteHit(true, 0);

            ParticleToggle(true, true);
        }
    }

    public void ParticleToggle(bool flag, bool flame)
    {
        ParticleManager.instance.ToggleParticle(flag, noteCol, NoteType.Long, (flame) ? HitText.Perfect : HitText.Miss);
    }
}
