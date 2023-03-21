using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using DG.Tweening;

public class V6Convert : MonoBehaviour
{
    public string fileName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Convert()
    {
        ConvertText(fileName);
        Debug.Log("Converted " + fileName);
    }

    public void ConvertText(string songName)
    {
        string path = Application.dataPath + "/AssetBundles/songdata/" + songName + ".txt";
        string[] text = File.ReadAllLines(path);
        string result = "";

        bool effect = false;

        for (int i = 0; i < text.Length; i++)
        {
            string l = text[i].Trim();
            if (effect && l != "End")
            {
                
                OldV6Effect e = JsonUtility.FromJson<OldV6Effect>(l);

                Debug.Log("Changing... " + e);

                result += JsonUtility.ToJson(new EffectStat { effectObj = e.objID, ease = e.easeType, yTime = e.yVal, xEditor = e.xSpot, type = EffectType.Move }) + "\n";
                result += JsonUtility.ToJson(new MoveModule {yVal = e.yVal, editorPos = e.xSpot, effectType = e.effectType, vec = e.vec, bars = e.bars, loops = e.loops, extra = e.extra, easeType = e.easeType, loopType = e.loopType }) + "\n";
            }
            else
            {
                result += l + "\n";
            }
            
            if (l == "Effect")
            {
                effect = true;
            }
        }

        File.WriteAllText(path, result);

    }
}

public struct OldV6Effect
{
    public string objID;
    public float yVal;
    public float xSpot;
    public string effectType;
    public Vector3 vec;
    public float bars;
    public int loops;
    public float[] extra;

    public Ease easeType;
    public LoopType loopType;
}
