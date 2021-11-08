using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SongEditorCameraManager : MonoBehaviour
{
    //Gameobject + Instantiates
    [SerializeField]
    private SongComplier sc;

    [SerializeField]
    private GameObject noteHolder;

    [SerializeField]
    private GameObject[] moveWithCamera;

    [SerializeField]
    private GameObject[] snapWithCamera;

    [SerializeField]
    private GameObject[] arrowGameObjs;

    //GameObject Selection
    private GameObject selectedObj;

    [SerializeField]
    private GameObject selectorIndicator;
    [SerializeField]
    private Image noteDisplay;
    [SerializeField]
    private TMP_InputField customYAddInputField;

    [SerializeField]
    private TMP_InputField longNoteInputField;
    [SerializeField]
    private Toggle longNoteToggle;
    [HideInInspector]
    private bool longNoteFlag;

    private const string validNum = "-0123456789.";
    //private const string validNumNoNeg = "0123456789.";

    //Camera
    private Camera mainCamera;

    private Vector3 mousePos;
    private Vector2 mousePos2D;
    private RaycastHit2D hit;

    //Note Grid Background Info + Note Place Varibles
    private Vector3 snapPos;
    private const float bgSnapDisplacement = -1f;
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

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        resetCamera = mainCamera.transform.position;

        baseScreenSize = mainCamera.orthographicSize;

        //customYAddInputField.in
        customYAddInputField.onValidateInput = (string text, int charIndex, char addedChar) => { return ValidateCharacter(validNum, addedChar, false); };
        customYAddInputField.characterLimit = 10;
        longNoteInputField.onValidateInput = (string text, int charIndex, char addedChar) => { return ValidateCharacter(validNum, addedChar, false); };
        longNoteInputField.characterLimit = 10;

        snapPos.z = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log(mainCamera.ScreenToWorldPoint(Input.mousePosition));
        }

        //=====Left Click=========
        //For Placing Notes down and Stuff
        if (Input.GetMouseButtonDown(0))
        {
            //Regular Arrows
            if (Mathf.Abs(mousePos.x) <= 4f)
            {
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
                    default:
                        selectedObj = arrowGameObjs[0];
                        Debug.Log("Something went wrong!");
                        break;
                }

                selectedObj = Instantiate(selectedObj, snapPos, Quaternion.identity, noteHolder.transform);
                OnSelectGameObject(selectedObj);
                SetLongNote();
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

                GetLongNote(hit.collider.gameObject);
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

            AlignGameObjects();
        }
        //=======Scroll Wheel========
        //Scrolling to pan in and out
        scrollVal = Input.mouseScrollDelta.y * -1;

        if (scrollVal != 0)
        {
            multiplierScreenSize = Mathf.Clamp(multiplierScreenSize + (multiplierInterval * scrollVal), 0.15f, 30f);

            mainCamera.orthographicSize = baseScreenSize * multiplierScreenSize;
        }
        //=========R Key===========
        //To Reset the camera View
        if (Input.GetKeyDown(KeyCode.R))
        {
            mainCamera.transform.position = resetCamera;

            multiplierScreenSize = 1f;
            mainCamera.orthographicSize = baseScreenSize;
        }

        //========T Key===========
        //Delete a Note
        if (Input.GetKeyDown(KeyCode.T))
        {
            mousePos2D = new Vector2(mousePos.x, mousePos.y);

            hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                Destroy(hit.collider.gameObject);
            }
        }

        
    }

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
            moveWithCamera[i].transform.localPosition = Vector3.up * yVal.y;
        }

        for (int i = 0; i < snapWithCamera.Length; i++)
        {
            snapWithCamera[i].transform.position = Vector3.up * (Mathf.Round(yVal.y / bgSnapInterval) * bgSnapInterval + bgSnapDisplacement);
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
