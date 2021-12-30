using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNoteObject : NoteClass
{
    [SerializeField]
    private bool valid;

    private NoteObject parentObj;
    private GameObject arrowButton;
    private Vector3 tempPos;
    private float percent;
    private float tempLength;
    private int intervals;
    private int counter;
    private bool flag;

    public override NoteType GetNoteType{ get { return NoteType.Long; } }

    // Start is called before the first frame update
    void Start()
    {
        valid = false;
        flag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!flag && parentObj && !parentObj.gameObject.activeSelf && (Input.GetKey(keyPress) || Input.GetKey(altKeyPress)))
        {
            ParticleToggle(true, true);
            
            flag = true;
            valid = true;
        }

        if (valid)
        {
            if (Input.GetKeyUp(keyPress) || Input.GetKeyUp(altKeyPress))
            {
                ParticleToggle(false, true);
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
        else if (counter != intervals && parentObj.yVal + yVal - GameManager.instance.GameTime < -0.25f)
        {
            MissedLongMiddle();
        }
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Activator"))
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
        if (collision.CompareTag("Activator") && this.gameObject.activeSelf)
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

            //buttonController.avalible = true;
        }
    }*/

    public void LongNoteSetup(NoteObject parent, GameObject arrow, float len, int lenVal, float time)
    {
        parentObj = parent;
        tempLength = len;
        intervals = lenVal;
        yVal = time;
        arrowButton = arrow;

        counter = 1;
    }

    public void MissedLongMiddle()
    {
        //Debug.Log(intervals + " " + counter + " " + (intervals - counter));

        GameManager.instance.LongNoteHit(false, intervals - counter);

        ParticleToggle(false, true);

        GetComponent<LongNoteObject>().enabled = false;
    }

    public void HandleLongObject() 
    {
        //This Formula tells how much of the long bar is complete Very Good!
        percent = (GameManager.instance.GameTime - parentObj.yVal) / yVal;

        //Debug.Log(arrowButton.transform.position);
        Debug.Log(1f - percent);
        tempPos = Vector3.zero;//arrowButton.transform.position;
        tempPos.y = (parentObj.transform.localPosition.y) + tempLength - (1f - percent) * tempLength * 0.5f; //+ (1f - percent) * tempLength * 0.5f + tempLength * 0.5f;
        //Debug.Log(tempPos.y);
        transform.localPosition = tempPos;//arrowButton.transform.position + 

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
        ParticleManager.instance.ToggleParticle(flag, arrowButton.transform.position, noteCol, NoteType.Long, (flame) ? HitText.Perfect : HitText.Miss);
    }
}
