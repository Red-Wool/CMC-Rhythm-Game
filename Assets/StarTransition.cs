using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StarTransition : MonoBehaviour
{
    [SerializeField] private GameObject star;

    public void OpenStar(float time, float additionalTime)
    {
        star.transform.localScale = Vector3.zero;
        star.transform.DOScale(100, time + additionalTime).SetEase(Ease.InOutSine).SetUpdate(true);
        star.transform.DORotate(Vector3.forward * 360, time, RotateMode.LocalAxisAdd).SetUpdate(true);
    }

    public void CloseStar(float time, float additionalTime)
    {
        star.transform.eulerAngles = Vector3.zero;
        star.transform.localScale = Vector3.one * 100;
        star.transform.DOScale(0, time + additionalTime).SetEase(Ease.InOutSine).SetUpdate(true);
        star.transform.DORotate(Vector3.forward * -360, time, RotateMode.LocalAxisAdd).SetUpdate(true);
    }
}
