using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoTweenTest : MonoBehaviour
{
    public GameObject noteMove;

    // Start is called before the first frame update
    //1 sec ~= 5.1 ~= 157
    //bpm / 30 = b 
    //bpm / 30 = sps = bars
    //bars -> sec bpm / 60
    void Start()
    {
        //noteMove.transform.DOMoveX(6, 4/(157f/120f)).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        //noteMove.transform.DOMoveY(6, 4 / (157f / 45f)).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        //noteMove.transform.DORotate(Vector3.left * 180, 3).SetEase(Ease.InOutBounce).SetLoops(-1, LoopType.Incremental);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
