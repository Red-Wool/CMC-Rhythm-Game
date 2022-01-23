using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class EffectModule
{
    public string objID;
    public EffectType effectType;
    public Vector3 vec;
    public float bars;

    public int loops;
    public float[] extra;

    public Ease easeType;
    public LoopType loopingStyle;

    public void Activate(GameObject obj)
    {
        switch (effectType)
        {
            case EffectType.TweenMove:
                obj.transform.DOMove(vec, bars / (GameManager.instance.bs.bpm / 60f)).SetEase(easeType).SetLoops(loops, loopingStyle);
                break;

            case EffectType.TweenRotate:
                obj.transform.DORotate(vec, bars / (GameManager.instance.bs.bpm / 60f)).SetEase(easeType).SetLoops(loops, loopingStyle);
                break;

            case EffectType.TweenScale:
                obj.transform.DOScale(vec, bars / (GameManager.instance.bs.bpm / 60f)).SetEase(easeType).SetLoops(loops, loopingStyle);
                break;

            case EffectType.TweenKill:
                obj.transform.DOKill();
                break;

            case EffectType.Teleport:
                obj.transform.position = vec;
                break;

            case EffectType.CountDown:
                break;
        }
    }

    public EffectModule GetData()
    {
        return this;
    }
}
public class SpecialEffectModule
{
    public EffectModule module;
    public GameObject obj;
}

public enum EffectType
{
    TweenMove,
    TweenRotate,
    TweenScale,
    TweenKill,
    Teleport,
    CountDown
}



/*public class TweenMove : EffectModule
{
    public Ease type;
    public Vector3 target;
    public float bars;
    public int loops;
    public LoopType loopingStyle;

    public override void Activate(GameObject obj)
    {
        obj.transform.DOMove(target, bars / (GameManager.instance.bs.bpm / 60f)).SetEase(type).SetLoops(loops, loopingStyle);
    }

    public override object GetData()
    {
        return this;
    }
}

public class TweenRotate : EffectModule
{
    public Ease type;
    public Vector3 direction;
    public float bars;
    public int loops;
    public LoopType loopingStyle;
    public override void Activate(GameObject obj)
    {
        obj.transform.DORotate(direction, bars / (GameManager.instance.bs.bpm / 60f)).SetEase(type).SetLoops(loops, loopingStyle);
    }

    public override object GetData()
    {
        return this;
    }
}

public class TweenScale : EffectModule
{
    public Ease type;
    public Vector3 size;
    public float bars;
    public int loops;
    public LoopType loopingStyle;

    public override void Activate(GameObject obj)
    {
        obj.transform.DOScale(size, bars / (GameManager.instance.bs.bpm / 60f)).SetEase(type).SetLoops(loops, loopingStyle);
    }

    public override object GetData()
    {
        return this;
    }
}

public class TweenKill : EffectModule
{
    public override void Activate(GameObject obj)
    {
        obj.transform.DOKill();
    }

    public override object GetData()
    {
        return this;
    }
}

public class Teleport : EffectModule
{
    public Vector3 target;

    public override void Activate(GameObject obj)
    {
        obj.transform.position = target;
    }

    public override object GetData()
    {
        return this;
    }
}

public class CountDown : EffectModule
{
    public float bars;
    public int beats;

    public override void Activate(GameObject obj)
    {
        
    }

    public override object GetData()
    {
        return this;
    }
}*/
