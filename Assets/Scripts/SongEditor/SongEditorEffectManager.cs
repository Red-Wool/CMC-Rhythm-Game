using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

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

    [Space(10), Header("General Field"),
     SerializeField] private TMP_InputField effectObjectField;
    [SerializeField] private TMP_InputField effectDurationField;

    //Ease
    [Space(10),
     SerializeField] private GameObject easeContainer;
    [SerializeField] private TMP_Text easeText;

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

    //Move Effect
    [Space(10), Header("Move"),
     SerializeField] private GameObject loopContainer;
    [SerializeField] private TMP_Text loopTypeText;

    [SerializeField] private TMP_InputField loopNum;

    [Space(10),
     SerializeField] private TMP_InputField effectX;
    [SerializeField] private TMP_InputField effectY;
    [SerializeField] private TMP_InputField effectZ;

    //Arrow Path Effect
    [Space(10), Header("Arrow Path"),
     SerializeField] private TMP_InputField arrowPathSpeedField;
    [SerializeField] private TMP_InputField arrowPathObjIDField;
    [SerializeField] private Transform arrowPathFieldParent;

    //Shader Effect
    [Space(10), Header("Shader"),
     SerializeField] private TMP_InputField shaderSpeedField;
    [SerializeField] private Transform shaderFieldParent;

    [SerializeField] private Transform effectChooseDisplayParent;
    private List<GameObject> effectChooseObjects;

    private List<GameObject>[] moveOptionList;

    private EditorEffectTriggerObject selectedGameObject;
    private EditorMoveEffect selectMove;
    private EditorArrowPathEffect selectArrowPath;
    private EditorShaderEffect selectShader;
     


    GameObject tempGameObj;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        //Create Editor Fields

        editorFieldSingle.AddObjects();
        editorFieldDouble.AddObjects();
        editorFieldTriple.AddObjects();

        //Ease Options
        Ease[] allEases = Enum.GetValues(typeof(Ease)) as Ease[];
        for (int i = 0; i < allEases.Length; i++)
        {
            tempGameObj = Instantiate(effectButtonPrefab, easeContainer.transform);
            tempGameObj.GetComponentInChildren<TMP_Text>().text = ((Ease)i).ToString();

            Ease ea = allEases[i];
            tempGameObj.GetComponent<Button>().onClick.AddListener(() => SetEase(ea));
        }

        //Loop Options
        LoopType[] allLoops = Enum.GetValues(typeof(LoopType)) as LoopType[];
        for (int i = 0; i < allLoops.Length; i++)
        {
            tempGameObj = Instantiate(effectButtonPrefab, loopContainer.transform);
            tempGameObj.GetComponentInChildren<TMP_Text>().text = ((LoopType)i).ToString();

            LoopType l = allLoops[i];
            tempGameObj.GetComponent<Button>().onClick.AddListener(() => selectMove.move.loopType = l);
            tempGameObj.GetComponent<Button>().onClick.AddListener(() => loopTypeText.text = l.ToString());
        }

        //Effect Option Create
        moveOptionList = new List<GameObject>[(Enum.GetValues(typeof(EffectType)) as EffectType[]).Length];
        for (int i = 0; i < moveOptionList.Length; i++)
        {
            moveOptionList[i] = new List<GameObject>();
        }

        //Move Type
        MoveType[] allMove = Enum.GetValues(typeof(MoveType)) as MoveType[];
        for (int i = 0; i < allMove.Length; i++)
        {
            tempGameObj = Instantiate(effectButtonPrefab, effectOptionContainer.transform);
            tempGameObj.GetComponentInChildren<TMP_Text>().text = ((MoveType)i).ToString();

            int t = i;
            tempGameObj.GetComponent<Button>().onClick.AddListener(() => ChooseEffectOption(EffectType.Move, ((MoveType)t).ToString()));

            moveOptionList[1].Add(tempGameObj);
        }

        //Arrow Path
        for (int i = 0; i < data.arrowPathOptions.Length; i++)
        {
            tempGameObj = Instantiate(effectButtonPrefab, effectOptionContainer.transform);
            tempGameObj.GetComponentInChildren<TMP_Text>().text = data.arrowPathOptions[i];

            int t = i;
            tempGameObj.GetComponent<Button>().onClick.AddListener(() => ChooseEffectOption(EffectType.ArrowPath, data.arrowPathOptions[t]));

            moveOptionList[2].Add(tempGameObj);
        }

        //Shader
        for (int i = 0; i < data.shaderNames.Length; i++)
        {
            tempGameObj = Instantiate(effectButtonPrefab, effectOptionContainer.transform);
            tempGameObj.GetComponentInChildren<TMP_Text>().text = data.shaderNames[i];

            int t = i;
            tempGameObj.GetComponent<Button>().onClick.AddListener(() => ChooseEffectOption(EffectType.Shader, data.shaderNames[t]));

            moveOptionList[4].Add(tempGameObj);
        }

        //Shader for Toggle
        #region ShaderToggle
        tempGameObj = Instantiate(effectButtonPrefab, effectOptionContainer.transform);
        tempGameObj.GetComponentInChildren<TMP_Text>().text = "Activate";

        tempGameObj.GetComponent<Button>().onClick.AddListener(() => ChooseEffectOption(EffectType.Shader, "Activate"));

        moveOptionList[4].Add(tempGameObj);

        tempGameObj = Instantiate(effectButtonPrefab, effectOptionContainer.transform);
        tempGameObj.GetComponentInChildren<TMP_Text>().text = "Deactivate";

        tempGameObj.GetComponent<Button>().onClick.AddListener(() => ChooseEffectOption(EffectType.Shader, "Deactivate"));

        moveOptionList[4].Add(tempGameObj);
        #endregion

        //Create Effect Type Display
        effectChooseObjects = new List<GameObject>(); 
        for (int i = 0; i < data.effectTypeSprites.Length; i++)
        {
            GameObject obj = Instantiate(effectChoosePrefab, effectChooseDisplayParent.transform);
            obj.GetComponent<Image>().sprite = data.effectTypeSprites[i].sprite;

            effectChooseObjects.Add(obj);
        }

        MoveChoose(0);

        effectObjectField.onValueChanged.AddListener(s => selectedGameObject.objectEffect = s);

        loopNum.onValueChanged.AddListener(s => selectMove.move.loops = int.Parse(s));
        effectX.onValueChanged.AddListener(s => selectMove.move.vec.x = float.Parse(s));
        effectY.onValueChanged.AddListener(s => selectMove.move.vec.y = float.Parse(s));
        effectZ.onValueChanged.AddListener(s => selectMove.move.vec.z = float.Parse(s));

        effectDurationField.onValueChanged.AddListener(s => ChangeDuration(s));
        arrowPathObjIDField.onValueChanged.AddListener(s => selectArrowPath.arrowPath.objID = int.Parse(s));
        arrowPathSpeedField.onValueChanged.AddListener(s => selectArrowPath.arrowPath.stats.speed = float.Parse(s));
        shaderSpeedField.onValueChanged.AddListener(s => selectShader.shader.speed = float.Parse(s));
        
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
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            MoveChoose(4);
        }
    }

    public void MoveChoose(int id)
    {
        chooseID = id;
        chooseDisplay.transform.SetParent(effectChooseObjects[id].transform, false);
        chooseDisplay.transform.localPosition = Vector3.zero;
    }

    public GameObject CreateEffect(Vector3 position)
    {
        GameObject obj = Instantiate(effectPrefabs[chooseID], position, Quaternion.identity, noteHolder);
        obj.GetComponent<EditorEffectTriggerObject>().objectEffect = "Main";
        return obj;
    }

    public void SetEffectObject(string s)
    {
        selectedGameObject.objectEffect = s;
    }

    public void SetEase(Ease e)
    {
        selectedGameObject.easeType = e;
        easeText.text = e.ToString();
    }

    public void ChangeDuration(string s)
    {
        switch (selectedGameObject.effectType)
        {
            case EffectType.Move:
                selectMove.move.bars = float.Parse(s);
                break;
            case EffectType.ArrowPath:
                selectArrowPath.arrowPath.stats.duration = float.Parse(s);
                break;
            case EffectType.Shader:
                selectShader.shader.duration = float.Parse(s);
                break;
        }
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

        effectObjectField.text = obj.objectEffect;
        easeText.text = obj.easeType.ToString();

        string effectName = "";
        switch (selectedGameObject.effectType)
        {
            case EffectType.Move:
                selectMove = obj.GetComponent<EditorMoveEffect>();

                loopTypeText.text = selectMove.move.loopType.ToString();
                loopNum.text = selectMove.move.loops.ToString();

                effectX.text = selectMove.move.vec.x.ToString();
                effectY.text = selectMove.move.vec.y.ToString();
                effectZ.text = selectMove.move.vec.z.ToString();

                effectDurationField.text = selectMove.move.bars.ToString();

                effectName = selectMove.move.effectType;

                break;
            case EffectType.ArrowPath:
                selectArrowPath = obj.GetComponent<EditorArrowPathEffect>();

                arrowPathSpeedField.text = selectArrowPath.arrowPath.stats.speed.ToString();
                arrowPathObjIDField.text = selectArrowPath.arrowPath.objID.ToString();
                effectDurationField.text = selectArrowPath.arrowPath.stats.duration.ToString();

                effectName = selectArrowPath.arrowPath.notePosID;

                
                break;
            case EffectType.Shader:
                selectShader = obj.GetComponent<EditorShaderEffect>();

                shaderSpeedField.text = selectShader.shader.speed.ToString();
                effectDurationField.text = selectShader.shader.duration.ToString();
                
                if (selectShader.shaderData == null)
                {
                    selectShader.shaderData = LoadAssetBundle.GetShaderObject(selectShader.shader.shaderDataName);
                }

                effectName = selectShader.shader.shaderDataName;
                
                break;
        }

        effectOptionText.text = effectName;

        ChangeEffectOptions((int)selectedGameObject.effectType);
        ChooseEffectOption(selectedGameObject.effectType, effectName);
    }

    private void ChangeEffectOptions(int id)
    {
        //Debug.Log(moveOptionList.Length);
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
            case EffectType.Shader:
                ChooseShaderOption(effect);
                break;
        }
    }

    #region ChooseEffectOptionMethods
    public void ChooseMoveOption(MoveType effect)
    {
        selectMove.move.effectType = effect.ToString();
        effectOptionText.text = effect.ToString();
    }

    public void ChooseArrowPathOption(string effect)
    {
        selectArrowPath.arrowPath.notePosID = effect;
        effectOptionText.text = effect;

        for (int i = 0; i < arrowPathFieldParent.childCount; i++)
        {
            arrowPathFieldParent.GetChild(i).gameObject.SetActive(false);
        }

        EditorRequest request = ArrowPathFunctions.RequestEditorData(effect);

        int num = request.fieldNum;
        if (selectArrowPath.arrowPath.stats.store.Length != num)
        {
            //Debug.Log("Reset");
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
                num += SetupEditorField(fields[i], ArrowPathFieldChange, arrowPathFieldParent, storeList, num);
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

    public void ChooseShaderOption(string effect)
    {
        Debug.Log(" dwa " + effect);

        switch (effect)
        {
            case "Activate":
                selectShader.shader.option = ShaderOption.On;
                selectShader.shader.shaderDataName = "Activate";
                effectOptionText.text = "Activate";
                return;
            case "Deactivate":
                selectShader.shader.option = ShaderOption.Off;
                selectShader.shader.shaderDataName = "Deactivate";
                effectOptionText.text = "Deactivate";
                return;
        }
        effectOptionText.text = effect;
        selectShader.shader.option = ShaderOption.Change;

        //selectArrowPath.arrowPath.notePosID = effect;
        ShaderDataObject shaderData = LoadAssetBundle.GetShaderObject(effect);

        if (shaderData == null)
        {
            return;
        }

        selectShader.shaderData = shaderData;
        selectShader.shader.shaderDataName = effect;

        for (int i = 0; i < shaderFieldParent.childCount; i++)
        {
            shaderFieldParent.GetChild(i).gameObject.SetActive(false);
        }

        EditorRequest request = shaderData.request;

        int num = request.fieldNum;
        //Debug.Log(selectArrowPath.arrowPath.stats.store.Length + " " + num + " " + effect);
        if (selectShader.shader.store.Length != num)
        {
            Debug.Log("Reset");
            float[] list = new float[num];
            selectShader.shader.store = list;
        }

        num = 0;
        if (request != null)
        {
            float[] storeList = selectShader.shader.store;
            EditorRequestField[] fields = request.requestFields;
            for (int i = 0; i < fields.Length; i++)
            {
                num += SetupEditorField(fields[i], ShaderFieldChange, shaderFieldParent, storeList, num);
            }
        }
        else
        {
            Debug.LogError("Null Shader Option");
        }
    }

    public void ShaderFieldChange(string text, int objID)
    {
        selectShader.shader.store[objID] = float.Parse(text);
    }

    #endregion
    public int SetupEditorField(EditorRequestField field, GetFieldString fieldMethod, Transform parent, float[] content, int id)
    {
        int argumentNum = 0;

        switch (field.requestType)
        {
            case RequestType.Vector3:
                tempGameObj = editorFieldTriple.GetObject();
                argumentNum = 3;
                break;
            case RequestType.Vector2:
                tempGameObj = editorFieldDouble.GetObject();
                argumentNum = 2;
                break;
            default:
                tempGameObj = editorFieldSingle.GetObject();
                argumentNum = 1;
                break;
        }

        tempGameObj.transform.SetParent(parent, false);
        tempGameObj.transform.SetAsLastSibling();

        string[] arguments = new string[argumentNum];
        for (int i = 0; i < argumentNum; i++)
        {
            arguments[i] = content[id + i].ToString();
        }
        tempGameObj.GetComponent<EditorField>().SetUp(field, fieldMethod, arguments, id);

        return argumentNum;
    }
}
