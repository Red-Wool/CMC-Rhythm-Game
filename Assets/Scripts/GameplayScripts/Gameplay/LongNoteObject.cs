using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(LineRenderer))]
public class LongNoteObject : NoteClass
{
    [SerializeField]
    private bool valid;
    private float parentY;

    private LineRenderer lineRender;
    private int lineLength;
    private Vector3[] lines;

    private NoteObject parentObj;
    private GameObject arrowButton;
    private float percent;
    private float missSubtract;
    private int intervals;
    private int counter;
    private bool flag;
    private bool miss;

    public override NoteType GetNoteType{ get { return NoteType.Long; } }

    // Start is called before the first frame update
    void Start()
    {
        valid = false;
        flag = false;
        miss = false;
    }

    // Update is called once per frame
    void Update()
    {
        eval = parentY - GameManager.instance.GameTime;

        //Temp solution to not calculate notes far away. Purely for testing, do not keep this!
        if (eval > 20)
        {
            lineRender.positionCount = 0;
            return;
        }

        if (!flag && parentObj && !parentObj.gameObject.activeSelf && (Input.GetKey(keyPress) || Input.GetKey(altKeyPress)))
        {
            ParticleToggle(true, true);
            
            flag = true;
            valid = true;
        }

        float size = button.transform.localScale.x;

        //lineRender.widthMultiplier = size;

        
        transform.localPosition = button.SetPosition(eval);

        missSubtract = Mathf.Min((eval + .35f) * 40f + lineLength * percent, 0);

        lineRender.positionCount = Mathf.Clamp((int)(lineLength * (1f-percent) + missSubtract), 0, lineLength) + 1;
        for (int i = 0; i < lineRender.positionCount - 1; i++)
        {
            //Debug.Log("");
            lines[i] = button.transform.position + button.transform.rotation * button.SetPosition(eval + (i - missSubtract + (int)(percent * lineLength)) * .025f)*size;
        }
        lines[lineRender.positionCount - 1] = button.transform.position + button.transform.rotation * button.SetPosition(eval + yVal) * size;
        lineRender.SetPositions(lines);

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
        else if (!miss && counter != intervals && parentY + yVal - GameManager.instance.GameTime < -0.025f)
        {
            MissedLongMiddle();
        }
    }

    public void LongNoteSetup(NoteObject parent, GameObject arrow, int lenVal, float time)
    {
        parentObj = parent;

        lineLength = (int)(time * 40f);

        lineRender = GetComponent<LineRenderer>();
        lines = new Vector3[lineLength + 1];
        lineRender.positionCount = lineLength;

        intervals = lenVal;
        yVal = time;
        parentY = parentObj.yVal;
        arrowButton = arrow;

        counter = 1;
    }

    public void MissedLongMiddle()
    {
        //Debug.Log(intervals + " " + counter + " " + (intervals - counter));

        miss = true;

        GameManager.instance.LongNoteHit(false, intervals - counter);

        ParticleToggle(false, true);

        transform.DOScale(0, eval + yVal + .55f).OnComplete(DisableLongNote);
    }

    public void DisableLongNote()
    {
        gameObject.SetActive(false);
    }

    public void HandleLongObject() 
    {
        //This Formula tells how much of the long bar is complete Very Good!
        percent = (GameManager.instance.GameTime - parentY) / yVal;

        //Debug.Log(Mathf.Max(0, (int)(lineLength * (1-percent))));
        

        /*tempPos = Vector3.zero;
        tempPos.y = (parentObj.transform.localPosition.y) + tempLength - ((1f - percent) * tempLength * 0.5f); //+ (1f - percent) * tempLength * 0.5f + tempLength * 0.5f;

        transform.localPosition = tempPos;

        tempPos = Vector3.one;
        tempPos.y = (tempLength / 2) * (1f - percent);

        transform.localScale = tempPos;*/

        if (percent > counter * (1f / intervals) && counter != intervals)
        {
            counter++;

            GameManager.instance.LongNoteHit(true, 0);

            ParticleToggle(true, true);
        }
    }

    public void ParticleToggle(bool isOn, bool flame)
    {
        ParticleManager.instance.ToggleParticle(isOn, arrowButton.transform.position, noteCol, NoteType.Long, (flame) ? HitText.Perfect : HitText.Miss); // - (arrowButton.transform.up * 0.2f)
    }
}
