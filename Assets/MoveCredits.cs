using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveCredits : MonoBehaviour
{
    public GameObject credit;
    public GameObject target;

    public void MoveCredit(bool flag)
    {
        credit.transform.DOMoveX(flag ? target.transform.position.x : -1000f, 1f).SetEase(Ease.InOutSine);
    }
}
