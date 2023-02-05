using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SongEditorEffectManager : MonoBehaviour
{
    #region Varibles
    //Basic Data
    [Header("Editor Data"),
     SerializeField] private SongEditorData data;
    [SerializeField] private Transform noteHolder;
    [SerializeField] private GameObject[] effectPrefabs;
    [SerializeField] private GameObject effectButtonPrefab;
    [Header("Tab Button"),
     SerializeField] private TabGroup tabGroup;
    [SerializeField] private TabButton effectTab;

    //Editor Field
    [Space(10), Header("Effect Field"),
     SerializeField] private ObjectPool editorFieldSingle;
    [SerializeField] private ObjectPool editorFieldDouble;
    [SerializeField] private ObjectPool editorFieldTriple;

    //Effect Type Choose
    [Space(10), Header("Effect Type"),
     SerializeField] private GameObject effectChoosePrefab;
    [SerializeField] private GameObject chooseDisplay;
    private int chooseID;

    [SerializeField] private GameObject[] effectTypeTab;

    //Effect Type Option
    [Space(10), Header("Effect Option"),
     SerializeField] private GameObject effectOptionContainer;
    [SerializeField] private TMP_Text effectOptionText;

    //Ease
    [Space(10), Header("Ease"),
     SerializeField] private GameObject easeContainer;
    [SerializeField] private TMP_Text easeText;

    //Loop

    [SerializeField]
    private Transform effectChooseDisplayParent;
    private List<GameObject> effectChooseObjects;

    private List<GameObject>[] moveOptionList;

    private EditorEffectTriggerObject selectedGameObject;
    private EditorMoveEffect selectMove;
    private EditorArrowPathEffect selectArrowPath;

    //ArrowPath
    [Space(10), Header("Arrow Path"),
     SerializeField] private Transform arrowPathFieldParent;


    GameObject tempGameObj;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        //Create Editor Fields

        editorFieldSingle.AddObjects();
        editorFieldDouble.AddObjects();
        editorFieldTriple.AddObjects();

        //Effect Option Create
        moveOptionList = new List<GameObject>[(Enum.GetValues(typeof(EffectType)) as EffectType[]).Length];
        for (int i = 0; i < moveOptionList.Length; i++)
        {
            moveOptionList[i] = new List<GameObject>();
        }

        MoveType[] allMove = Enum.GetValues(typeof(MoveType)) as MoveType[];
        for (int i = 0; i < allMove.Length; i++)
        {
            tempGameObj = Instantiate(effectButtonPrefab, effectOptionContainer.transform);
            tempGameObj.GetComponentInChildren<TMP_Text>().text = ((MoveType)i).ToString();

            int t = i;
            tempGameObj.GetComponent<Button>().onClick.AddListener(() => ChooseEffectOption(EffectType.Move, ((MoveType)t).ToString()));

            moveOptionList[1].Add(tempGameObj);
        }

        for (int i = 0; i < data.arrowPathOptions.Length; i++)
        {
            tempGameObj = Instantiate(effectButtonPrefab, effectOptionContainer.transform);
            tempGameObj.GetComponentInChildren<TMP_Text>().text = data.arrowPathOptions[i];

            int t = i;
            tempGameObj.GetComponent<Button>().onClick.AddListener(() => ChooseEffectOption(EffectType.ArrowPath, data.arrowPathOptions[t]));

            moveOptionList[2].Add(tempGameObj);
        }

        //Create Effect Type Display
        effectChooseObjects = new List<GameObject>(); 
        for (int i = 0; i < data.effectTypeSprites.Length; i++)
        {
            GameObject obj = Instantiate(effectChoosePrefab, effectChooseDisplayParent.transform);
            obj.GetComponent<Image>().sprite = data.effectTypeSprites[i].sprite;

            effectChooseObjects.Add(obj);
        }
        MoveChoose(0);


    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            MoveChoose(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            MoveChoose(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            MoveChoose(2);
        }
    }

    public void MoveChoose(int id)
    {
        chooseID = id;
        chooseDisplay.transform.parent = effectChooseObjects[id].transform;
        chooseDisplay.transform.localPosition = Vector3.zero;
    }

    public GameObject CreateEffect(Vector3 position)
    {
        return Instantiate(effectPrefabs[chooseID], position, Quaternion.identity, noteHolder);
    }

    public void SelectEffect(EditorEffectTriggerObject obj)
    {
        tabGroup.OnTabClick(effectTab);

        selectedGameObject = obj;
        if (selectedGameObject == null)
        {
            Debug.LogError("Null Editor Effect Trigger Object Selected!");
            return;
        }

        string effectName = "";
        switch (selectedGameObject.effectType)
        {
            case EffectType.Move:
                selectMove = obj.GetComponent<EditorMoveEffect>();

                effectName = selectMove.move.effectType;
                effectOptionText.text = effectName;
                break;
            case EffectType.ArrowPath:
                selectArrowPath = obj.GetComponent<EditorArrowPathEffect>();

                effectName = selectArrowPath.arrowPath.notePosID;
                effectOptionText.text = effectName;
                break;
        }

        ChangeEffectOptions((int)selectedGameObject.effectType);
        ChooseEffectOption(selectedGameObject.effectType, effectName);
    }

    private void ChangeEffectOptions(int id)
    {
        for (int i = 0; i < Mathf.Min(moveOptionList.Length, effectTypeTab.Length); i++)
        {
            for (int j = 0; j < moveOptionList[i].Count; j++)
            {
                moveOptionList[i][j].SetActive(id == i);
            }
            effectTypeTab[i].SetActive(id == i);
        }
    }

    public void ChooseEffectOption(EffectType type, string effect)
    {
        switch (type)
        {
            case EffectType.Move:
                MoveType move;
                if (Enum.TryParse(effect, out move))
                    ChooseMoveOption(move);
                else
                    Debug.Log("Invalid Move Type: " + effect);
                break;
            case EffectType.ArrowPath:
                ChooseArrowPathOption(effect);
                break;
        }
    }

    public void ChooseMoveOption(MoveType effect)
    {
        selectMove.move.effectType = effect.ToString();
        effectOptionText.text = effect.ToString();
    }

    public void ChooseArrowPathOption(string effect)
    {
        selectArrowPath.arrowPath.notePosID = effect;
        effectOptionText.text = effect;

        /*foreach (GameObject i in childrenList)
        {
            Destroy(i);
        }*/

        for (int i = 0; i < arrowPathFieldParent.childCount; i++)
        {
            arrowPathFieldParent.GetChild(i).gameObject.SetActive(false);
        }

        EditorRequest request = ArrowPathFunctions.RequestEditorData(effect);

        int num = request.fieldNum;
        Debug.Log(selectArrowPath.arrowPath.stats.store.Length + " " + num + " " + effect);
        if (selectArrowPath.arrowPath.stats.store.Length != num)
        {
            Debug.Log("Reset");
            float[] list = new float[num];
            selectArrowPath.arrowPath.stats.store = list;
        }

        num = 0;
        if (request != null)
        {
            float[] storeList = selectArrowPath.arrowPath.stats.store;
            EditorRequestField[] fields = request.requestFields;
            for (int i = 0; i < fields.Length; i++)
            {
                
                //Debug.Log(storeList[1]);
                if (fields[i].requestType == RequestType.Vector3)
                {
                    tempGameObj = editorFieldTriple.GetObject();
                    tempGameObj.transform.parent = arrowPathFieldParent;
                    tempGameObj.transform.SetAsLastSibling();

                    GetFieldString fieldString = ArrowPathFieldChange;
                    tempGameObj.GetComponent<EditorField>().SetUp(fields[i], fieldString, new string[] { storeList[num].ToString(), storeList[num + 1].ToString(), storeList[num+2].ToString() }, num);

                    num += 3;
                }
                else if (fields[i].requestType == RequestType.Vector2)
                {
                    tempGameObj = editorFieldDouble.GetObject();
                    tempGameObj.transform.parent = arrowPathFieldParent;
                    tempGameObj.transform.SetAsLastSibling();

                    GetFieldString fieldString = ArrowPathFieldChange;
                    tempGameObj.GetComponent<EditorField>().SetUp(fields[i], fieldString, new string[] { storeList[num].ToString(), storeList[num + 1].ToString()}, num);

                    num += 2;
                }
                else
                {
                    tempGameObj = editorFieldSingle.GetObject();
                    tempGameObj.transform.parent = arrowPathFieldParent;
                    tempGameObj.transform.SetAsLastSibling();

                    GetFieldString fieldString = ArrowPathFieldChange;
                    tempGameObj.GetComponent<EditorField>().SetUp(fields[i], fieldString, new string[] { storeList[num].ToString().ToString() }, num);

                    num += 1;
                }
            }
        }
        else
        {
            Debug.LogError("Null Arrow Path Option");
        }

    }

    public void ArrowPathFieldChange(string text, int objID)
    {
        selectArrowPath.arrowPath.stats.store[objID] = float.Parse(text);
    }
}
