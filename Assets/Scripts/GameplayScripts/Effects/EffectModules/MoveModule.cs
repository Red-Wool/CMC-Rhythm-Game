using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening;

[System.Serializable]
public class MoveModule
{
    public float yVal;
    public float editorPos;
    public string effectType;
    public Vector3 vec;
    public float bars;

    public int loops;
    public float[] extra;

    public Ease easeType;
    public LoopType loopingStyle;

    public void Activate(GameObject obj)
    {
        MoveType effect;
        System.Enum.TryParse<MoveType>(effectType, out effect);

        switch (effect)
        {
            case MoveType.TweenMove:
                obj.transform.DOMove(vec, bars / (GameManager.instance.bs.bpm / 60f)).SetEase(easeType).SetLoops(loops, loopingStyle);
                break;

            case MoveType.TweenMoveX:
                obj.transform.DOMoveX(vec.x, bars / (GameManager.instance.bs.bpm / 60f)).SetEase(easeType).SetLoops(loops, loopingStyle);
                break;

            case MoveType.TweenMoveY:
                obj.transform.DOMoveY(vec.y, bars / (GameManager.instance.bs.bpm / 60f)).SetEase(easeType).SetLoops(loops, loopingStyle);
                break;

            case MoveType.TweenMoveJump:
                obj.transform.DOJump(new Vector3(vec.x, vec.y), vec.z, loops, bars / (GameManager.instance.bs.bpm / 30f));
                break;

            case MoveType.TweenMoveJumpLocal:
                obj.transform.DOLocalJump(new Vector3(vec.x, vec.y), vec.z, loops, bars / (GameManager.instance.bs.bpm / 30f));
                break;

            case MoveType.TweenRotate:
                obj.transform.DORotate(vec, bars / (GameManager.instance.bs.bpm / 60f)).SetEase(easeType).SetLoops(loops, loopingStyle);
                break;

            case MoveType.TweenScale:
                obj.transform.DOScale(vec, bars / (GameManager.instance.bs.bpm / 60f)).SetEase(easeType).SetLoops(loops, loopingStyle);
                break;

            case MoveType.TweenKill:
                obj.transform.DOKill();
                break;

            case MoveType.TweenKillAll:
                DOTween.KillAll();
                break;

            case MoveType.Teleport:
                obj.transform.position = vec;
                break;

            case MoveType.CountDown:
                break;

            case MoveType.Flash:
                obj.GetComponent<SpriteRenderer>().DOFade(1, bars / (GameManager.instance.bs.bpm / 30f)).SetEase(easeType).OnComplete(() =>
                obj.GetComponent<SpriteRenderer>().DOFade(0, bars / (GameManager.instance.bs.bpm / 30f)).SetEase(easeType));
                break;

            case MoveType.CameraSetScale:
                GameManager.instance.mainCamera.orthographicSize = vec.x;
                break;

            case MoveType.CameraSmoothScale:
                GameManager.instance.mainCamera.DOOrthoSize(vec.x, bars / (GameManager.instance.bs.bpm / 30f)).SetEase(easeType);
                break;

            case MoveType.CameraBop:
                GameManager.instance.mainCamera.DOFieldOfView(vec.x, bars / (GameManager.instance.bs.bpm / 30f)).SetEase(easeType).OnComplete(() =>
                GameManager.instance.mainCamera.DOFieldOfView(vec.y, bars / (GameManager.instance.bs.bpm / 30f)).SetEase(easeType));
                break;

            case MoveType.CameraBopRepeat:

                for (int i = 0; i < loops; i++)
                {
                    CameraRepeat((vec.z / (GameManager.instance.bs.bpm / 30f)) * i);
                }
                break;

            case MoveType.ScreenShake:
                GameManager.instance.mainCamera.DOShakePosition(bars / (GameManager.instance.bs.bpm / 30f), vec.x, Mathf.RoundToInt(vec.y), vec.z).SetEase(easeType);
                break;

            case MoveType.ActivateShader:
                GameManager.instance.mainCamera.GetComponent<ShaderApply>().isActive = true;
                break;

            case MoveType.DeactivateShader:
                GameManager.instance.mainCamera.GetComponent<ShaderApply>().isActive = false;
                break;
        }
    }

    private async void CameraRepeat(float duration)
    {
        var end = Time.time + duration;

        while (Time.time < end)
            await Task.Yield();


        //Debug.Log("Ew");
        GameManager.instance.mainCamera.DOFieldOfView(vec.x, bars / (GameManager.instance.bs.bpm / 30f)).SetEase(easeType).OnComplete(() =>
            GameManager.instance.mainCamera.DOFieldOfView(vec.y, bars / (GameManager.instance.bs.bpm / 30f)).SetEase(easeType));
    }
}

public enum MoveType
{
    TweenMove,
    TweenMoveX,
    TweenMoveY,
    TweenMoveJump,
    TweenMoveJumpLocal,
    TweenRotate,
    //TweenRotateTo,
    TweenScale,
    //TweenScaleTo,
    TweenKill,
    TweenKillAll,
    Teleport,
    CountDown,
    Flash,
    FadeIn,
    FadeOut,
    CameraSetScale,
    CameraSmoothScale,
    CameraBop,
    CameraBopRepeat,
    ScreenShake,
    ActivateShader,
    DeactivateShader,
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
