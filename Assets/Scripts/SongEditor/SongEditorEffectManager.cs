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

    private List<GameObject> moveOptionList;
    private List<GameObject> arrowPathOptionList;

    private EditorEffectTriggerObject selectedGameObject;
    private EditorMoveEffect selectMove;
    private EditorArrowPathEffect selectArrowPath;


    // Start is called before the first frame update
    void Awake()
    {
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

    public void SelectEffect(GameObject obj)
    {

    }
}
