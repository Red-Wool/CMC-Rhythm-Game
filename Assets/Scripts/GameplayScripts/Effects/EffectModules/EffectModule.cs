using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class EffectModule
{
    public string objID;
    public EffectType effectType;

    public abstract void Activate(GameObject obj);
}

/*public class NoteModule
{
    public EffectModule module;
}*/

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
    Teleport,
    CountDown
}

public class TweenMove : EffectModule
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
}

public class Teleport : EffectModule
{
    public Vector3 target;

    public override void Activate(GameObject obj)
    {
        obj.transform.position = target;
    }
}

public class CountDown : EffectModule
{
    public float bars;
    public int beats;

    public override void Activate(GameObject obj)
    {
        
    }
}
