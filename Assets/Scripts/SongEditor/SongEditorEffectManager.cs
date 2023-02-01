using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongEditorEffectManager : MonoBehaviour
{
    [SerializeField]
    private SongEditorData data;
    [SerializeField]
    private Transform noteHolder;
    [SerializeField]
    private GameObject[] effectPrefabs;

    [SerializeField]
    private GameObject effectChoosePrefab;
    [SerializeField]
    private GameObject choose;
    private int chooseID;



    [SerializeField]
    private Transform effectChooseDisplayParent;
    private List<GameObject> effectChooseObjects;

    private List<GameObject>[] moveOptionList;

    private EditorEffectTriggerObject selectedGameObject;
    private EditorMoveEffect selectMove;
    private EditorArrowPathEffect selectArrowPath;


    // Start is called before the first frame update
    void Awake()
    {
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
        choose.transform.parent = effectChooseObjects[id].transform;
        choose.transform.localPosition = Vector3.zero;
    }

    public GameObject CreateEffect(Vector3 position)
    {
        return Instantiate(effectPrefabs[chooseID], position, Quaternion.identity, noteHolder);
    }

    public void SelectEffect(EditorEffectTriggerObject obj)
    {
        selectedGameObject = obj;
        if (selectedGameObject == null)
        {
            Debug.LogError("Null Editor Effect Trigger Object Selected!");
            return;
        }
        ChangeEffectOptions((int)selectedGameObject.effectType);
        switch (selectedGameObject.effectType)
        {
            case EffectType.Move:
                selectMove = obj.GetComponent<EditorMoveEffect>();
                break;
            case EffectType.ArrowPath:
                selectArrowPath = obj.GetComponent<EditorArrowPathEffect>();
                break;
        }
    }

    private void ChangeEffectOptions(int id)
    {
        for (int i = 0; i < moveOptionList.Length; i++)
        {
            for (int j = 0; j < moveOptionList[i].Count; j++)
            {
                moveOptionList[i][j].SetActive(id == i);
            }
        }
        
    }
}
