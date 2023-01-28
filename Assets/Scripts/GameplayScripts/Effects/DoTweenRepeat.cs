using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoTweenRepeat : MonoBehaviour
{
    //EffectModule effect;
    /*
    public static void DoCamera(EffectModule effect)
    {
        
    }

    private IEnumerator CameraRepeat(EffectModule effect)
    {
        for (int i = 0; i < effect.loops; i++)
        {
            Debug.Log("Ew");
            Camera.main.DOOrthoSize(effect.vec.x, effect.bars / (GameManager.instance.bs.bpm / 30f)).SetEase(effect.easeType).OnComplete(() =>
                Camera.main.DOOrthoSize(effect.vec.y, effect.bars / (GameManager.instance.bs.bpm / 30f)).SetEase(effect.easeType));

            yield return new WaitForSeconds(effect.vec.z / (GameManager.instance.bs.bpm / 60f));
        }
    }*/
}
