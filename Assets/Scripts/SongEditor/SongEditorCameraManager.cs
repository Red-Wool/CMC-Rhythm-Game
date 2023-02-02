using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SongEditorCameraManager : MonoBehaviour
{
    //Gameobject + Instantiates
    [SerializeField]
    private SongComplier sc;


    [Header("Editor Objects"), Space(10), SerializeField]
    private GameObject noteHolder;

    [SerializeField]
    private GameObject[] moveWithCamera;

    [SerializeField]
    private GameObject[] snapWithCamera;

    [SerializeField]
    private GameObject[] arrowGameObjs;

    //GameObject Selection
    private GameObject selectedObj;

    [Header("Arrow Modifiers"), Space(10), 
     SerializeField] private GameObject selectorIndicator;
    [SerializeField] private Image noteDisplay;
    [SerializeField] private TMP_InputField customYAddInputField;

    [SerializeField] private TMP_InputField longNoteInputField;
    [SerializeField] private Toggle longNoteToggle;
    [HideInInspector] private bool longNoteFlag;

    private const string validNum = "-0123456789.";

    //Effect Section
    [Header("Effect Editor"), Space(10),
     SerializeField] private SongEditorEffectManager effectManager;

    [Space(10),
     SerializeField] private GameObject effectButtonPrefab;
    [Space(10),
     SerializeField] private GameObject effectContainer;
    [SerializeField] private TMP_Text effectContTxt;

    [Space(10),
     SerializeField] private GameObject easeContainer;
    [SerializeField] private TMP_Text easeContTxt;

    [Space(10),
     SerializeField] private GameObject loopContainer;
    [SerializeField] private TMP_Text loopContTxt;

    [Space(10),
     SerializeField] private TMP_InputField effectObjectID;

    [Space(10),
     SerializeField] private TMP_InputField effectX;
    [SerializeField] private TMP_InputField effectY;
    [SerializeField] private TMP_InputField effectZ;

    [Space(10),
     SerializeField] private TMP_InputField effectBarCount;
    [SerializeField] private TMP_InputField effectLoopCount;


    //Camera
    private Camera mainCamera;

    private Vector3 mousePos;
    private Vector2 mousePos2D;
    private RaycastHit2D hit;

    //Note Grid Background Info + Note Place Varibles
    private Vector3 snapPos;
    private const float bgSnapDisplacement = 0f;
    private const float bgSnapInterval = 4f;

    private const float yPlaceSnap = 1f;

    //Camera Move Varibles
    private Vector3 origin;
    private Vector3 difference;
    private Vector3 resetCamera;

    private Vector3 yVal;

    private bool drag = false;

    //Zoom and Pan Varibles
    private float scrollVal;
    private float baseScreenSize;
    private float multiplierScreenSize = 1f;

    private const float multiplierInterval = 0.3f;

    //ExtraVaribles
    private EditorNoteObject noteObj;
    private EditorEffectTriggerObject triggerObj;

    private GameObject tempGameObj;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        resetCamera = mainCamera.transform.position;

        baseScreenSize = mainCamera.orthographicSize;

        //Custom Y and Long Note Len Input Constraints
        AddNumConstraint(customYAddInputField, 10, false);
        AddNumConstraint(longNoteInputField, 10, true);

        AddNumConstraint(effectX, 10, false);
        AddNumConstraint(effectY, 10, false);
        AddNumConstraint(effectZ, 10, false);
        AddNumConstraint(effectBarCount, 10, true);
        AddNumConstraint(effectLoopCount, 10, false);

        snapPos.z = 0f;

        //Add all effects, eases, and loops to editor

        Ease[] allEases = System.Enum.GetValues(typeof(Ease)) as Ease[];
        for (int i = 0; i < allEases.Length; i++)
        {
            tempGameObj = Instantiate(effectButtonPrefab, easeContainer.transform);
            tempGameObj.GetComponentInChildren<TMP_Text>().text = ((Ease)i).ToString();

            Ease ea = allEases[i];
            tempGameObj.GetComponent<Button>().onClick.AddListener(() => SetEaseType(ea));
        }

        LoopType[] allLoops = System.Enum.GetValues(typeof(LoopType)) as LoopType[];
        for (int i = 0; i < allLoops.Length; i++)
        {
            tempGameObj = Instantiate(effectButtonPrefab, loopContainer.transform);
            tempGameObj.GetComponentInChildren<TMP_Text>().text = ((LoopType)i).ToString();

            LoopType l = allLoops[i];
            tempGameObj.GetComponent<Button>().onClick.AddListener(() => SetLoopType(l));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log(mainCamera.ScreenToWorldPoint(Input.mousePosition));
        }

        AlignGameObjects();

        //=====Left Click=========
        //For Placing Notes down and Stuff
        if (Input.GetMouseButtonDown(0))
        {
            //Regular Arrows
            if (mousePos.x <= 14f && mousePos.x >= -4f)
            {
                bool isEffect = false;
                snapPos.y = Mathf.Round(mousePos.y / yPlaceSnap) * yPlaceSnap;

                snapPos.x = CeilOrFloorBasedOnSign(mousePos.x / 2f) * 2f;
                snapPos.x -= FindSign(snapPos.x);

                switch (snapPos.x)
                {
                    case -3f:
                        selectedObj = arrowGameObjs[0];
                        break;
                    case -1f:
                        selectedObj = arrowGameObjs[1];
                        break;
                    case 1f:
                        selectedObj = arrowGameObjs[2];
                        break;
                    case 3f:
                        selectedObj = arrowGameObjs[3];
                        break;
                    case 7f:
                        isEffect = true;
                        
                        break;
                    case 9f:
                        isEffect = true;
                        
                        break;
                    case 11f:
                        isEffect = true;
                        
                        break;
                    case 13f:
                        isEffect = true;
                        
                        break;
                    default:
                        //selectedObj = arrowGameObjs[0];
                        Debug.Log("InvalidPos");
                        selectedObj = null;
                        break;
                }

                if (isEffect)
                {
                    selectedObj = effectManager.CreateEffect(snapPos);
                    HandleEffectInput();
                    UpdateEffectEditor();
                }
                else if (selectedObj != null)
                {
                    selectedObj = Instantiate(selectedObj, snapPos, Quaternion.identity, noteHolder.transform);
                    OnSelectGameObject(selectedObj);

                    SetLongNote();
                }
                
            }
        }
        //========Right Click=======
        //Get Info on a Note
        if (Input.GetMouseButtonDown(1))
        {
            mousePos2D = new Vector2(mousePos.x, mousePos.y);

            hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                OnSelectGameObject(hit.collider.gameObject);

                if (selectedObj.GetComponent<EditorNoteObject>())
                {
                    GetLongNote(hit.collider.gameObject);
                }
                else if (selectedObj.GetComponent<EditorEffectTriggerObject>())
                {
                    effectManager.SelectEffect(selectedObj.GetComponent<EditorEffectTriggerObject>());
                    //UpdateEffectEditor();
                }
            }
            else
            {
                selectedObj = null;

                selectorIndicator.transform.position = new Vector3(6f, 0f);
                noteDisplay.sprite = selectorIndicator.GetComponent<SpriteRenderer>().sprite;
            }
        }

        //=====Middle Click=======
        //Moving the Camera around
        if (Input.GetMouseButton(2))
        {
            difference = mousePos - mainCamera.transform.position;

            if (!drag)
            {
                drag = true;

                origin = mousePos;
            }
        }
        else
        {
            drag = false;
        }
        if (drag)
        {
            yVal = origin - difference;
            mainCamera.transform.position = yVal;

            
        }
        //=======Scroll Wheel========
        //Scrolling to pan in and out
        scrollVal = Input.mouseScrollDelta.y * -1;

        if (scrollVal != 0)
        {
            multiplierScreenSize = Mathf.Clamp(multiplierScreenSize + (multiplierInterval * scrollVal), 0.15f, 30f);

            mainCamera.orthographicSize = baseScreenSize * multiplierScreenSize;
        }
        //=========Plus Key===========
        //To Reset the camera View
        if (Input.GetKeyDown(KeyCode.Plus))
        {
            mainCamera.transform.position = resetCamera;

            multiplierScreenSize = 1f;
            mainCamera.orthographicSize = baseScreenSize;
        }

        //========Minus Key===========
        //Delete a Note
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            mousePos2D = new Vector2(mousePos.x, mousePos.y);

            hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                Destroy(hit.collider.gameObject);
            }
        }

        
    }

    //Effect Editor Methods
    public void UpdateEffectEditor()
    {
        /*
        triggerObj = selectedObj.GetComponent<EditorEffectTriggerObject>();

        effectContTxt.text = triggerObj.effectInfo.effectType.ToString();
        easeContTxt.text = triggerObj.effectInfo.easeType.ToString();
        loopContTxt.text = triggerObj.effectInfo.loopingStyle.ToString();

        effectObjectID.text = triggerObj.effectInfo.objID;

        effectX.text = triggerObj.effectInfo.vec.x.ToString();
        effectY.text = triggerObj.effectInfo.vec.y.ToString();
        effectZ.text = triggerObj.effectInfo.vec.z.ToString();

        effectBarCount.text = triggerObj.effectInfo.bars.ToString();
        effectLoopCount.text = triggerObj.effectInfo.loops.ToString();
        */
    }

    public void HandleEffectInput()
    {
        /*
        if (CheckEffectInfo())
        {
            triggerObj.effectInfo.objID = effectObjectID.text;

            triggerObj.effectInfo.vec = new Vector3(float.Parse(effectX.text), float.Parse(effectY.text), float.Parse(effectZ.text));

            triggerObj.effectInfo.bars = float.Parse(effectBarCount.text);
            triggerObj.effectInfo.loops = int.Parse(effectLoopCount.text);
        }
        */
    }

    private bool CheckEffectInfo()
    {
        /*
        triggerObj = selectedObj.GetComponent<EditorEffectTriggerObject>();
        if (triggerObj != null)
        {
            if (triggerObj.effectInfo == null)
            {
                triggerObj.effectInfo = new EffectModule();
            }

            return true;
        }
        */
        return false;
        
    }

    public void SetEffectType(EffectType effect)
    {
        /*
        if (CheckEffectInfo())
        {
            triggerObj.effectInfo.effectType = effect.ToString();
            effectContTxt.text = effect.ToString();
        }
        */
    }

    public void SetEaseType(Ease easeType)
    {
        /*
        if (CheckEffectInfo())
        {
           

            triggerObj.effectInfo.easeType = easeType;
            easeContTxt.text = easeType.ToString();
        }
        */
    }

    public void SetLoopType(LoopType loopType)
    {
        /*
        if (CheckEffectInfo())
        {

            triggerObj.effectInfo.loopingStyle = loopType;
            loopContTxt.text = loopType.ToString();
        }
        */
    }

    //Helper Methods
    public float CeilOrFloorBasedOnSign(float num)
    {
        if (num >= 0f)
        {
            return Mathf.CeilToInt(num);
        }
        else
        {
            return Mathf.FloorToInt(num);
        }
        
    }

    public int FindSign(float num)
    {
        return (int)Mathf.Round(num / Mathf.Abs(num));
    }
    public void AlignGameObjects()
    {
        for (int i = 0; i < moveWithCamera.Length; i++)
        {
            moveWithCamera[i].transform.localPosition = Vector3.up * mainCamera.transform.position.y;
        }

        for (int i = 0; i < snapWithCamera.Length; i++)
        {
            snapWithCamera[i].transform.position = Vector3.up * (Mathf.Round(mainCamera.transform.position.y / bgSnapInterval) * bgSnapInterval + bgSnapDisplacement);
        }
    }

    public void OnSelectGameObject(GameObject note)
    {
        selectedObj = note;
        selectorIndicator.transform.position = selectedObj.transform.position;

        noteDisplay.sprite = selectedObj.GetComponent<SpriteRenderer>().sprite;
    }

    //Changes the Y of the selected Note
    public void ChangeNoteY(float val)
    {
        selectedObj.transform.position += Vector3.up * val;

        selectorIndicator.transform.position = selectedObj.transform.position;
    }

    //Method for the custom Y change
    public void CustomChangeNoteY()
    {
        //Debug.Log(float.Parse(customYAddInputField.text));
        ChangeNoteY(
            (customYAddInputField.text == "") ?
            0 :
            float.Parse(customYAddInputField.text));
    }

    //Method for the custom Y Set
    public void CustomSetNoteY()
    {
        ChangeNoteY(
            (customYAddInputField.text == "") ?
            0 :
            float.Parse(customYAddInputField.text) - selectedObj.transform.position.y);
        //selectedObj.transform.position += Vector3.up * (float.Parse(customYAddInputField.text) - selectedObj.transform.position.y);

        //selectorIndicator.transform.position = selectedObj.transform.position;
    }

    public void SetLongNote()
    {
        noteObj = selectedObj.GetComponent<EditorNoteObject>();

        noteObj.SetLongNote(longNoteToggle.isOn, (longNoteInputField.text == "") ? 0 : float.Parse(longNoteInputField.text));
    }

    public void GetLongNote(GameObject sel)
    {
        noteObj = sel.GetComponent<EditorNoteObject>();

        longNoteInputField.text = noteObj.GetLength().ToString();
        longNoteToggle.isOn = noteObj.GetIfLongNote();

        
    }

    private void AddNumConstraint(TMP_InputField field, int maxLen, bool noNegative)
    {
        field.onValidateInput = (string text, int charIndex, char addedChar) => { return ValidateCharacter(validNum, addedChar, noNegative); };
        field.characterLimit = maxLen;
    }

    private char ValidateCharacter(string validCharacters, char addedChr, bool noNegative)
    {
        //Debug.Log(validCharacters + " " + addedChr + " " + validCharacters.IndexOf(addedChr));
        if (validCharacters.IndexOf(addedChr) != -1 && (!noNegative || validCharacters.IndexOf(addedChr) != 0))
        {
            return addedChr;
        }
        else
        {
            Debug.Log("Nonvalid!");
            return '\0'; //Null Character
        }
            
    }
}
