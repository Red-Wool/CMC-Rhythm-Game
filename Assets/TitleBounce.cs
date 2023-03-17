using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleBounce : MonoBehaviour
{
    private Vector3 jumpPoint;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        jumpPoint = transform.localPosition;
        //InvokeRepeating("AutoBounce", 0, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1f)
        {
            timer -= 1f;
            AutoBounce();
        }
    }

    public void AutoBounce()
    {
        transform.DOPunchScale(Vector3.one * .1f, .4f,2,.2f).SetEase(Ease.OutSine);
    }

    public void BounceObject()
    {
        transform.DOKill();
        transform.DOLocalJump(jumpPoint, 30f, 1, .6f).SetEase(Ease.OutExpo);
        transform.DOScaleX(transform.localScale.x + .1f, .1f).OnComplete(() =>
        transform.DOScaleX(1, .05f));
        transform.DOScaleY(transform.localScale.y - .1f, .05f).OnComplete(() =>
        transform.DOScaleY(1, .025f));
    }
}
