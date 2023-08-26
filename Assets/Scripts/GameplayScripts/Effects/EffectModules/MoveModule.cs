using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public LoopType loopType;

    //private bool validAsync = false;

    public void Activate(GameObject obj)
    {
        

        MoveType effect;
        float duration = bars / (GameManager.instance.bs.bpm / 30f);
        System.Enum.TryParse<MoveType>(effectType, out effect);

        switch (effect)
        {
            case MoveType.TweenMove:
                obj.transform.DOMove(vec, duration).SetEase(easeType).SetLoops(loops, loopType);
                break;

            case MoveType.TweenMoveX:
                obj.transform.DOMoveX(vec.x, duration).SetEase(easeType).SetLoops(loops, loopType);
                break;

            case MoveType.TweenMoveY:
                obj.transform.DOMoveY(vec.y, duration).SetEase(easeType).SetLoops(loops, loopType);
                break;

            case MoveType.TweenMoveJump:
                obj.transform.DOJump(new Vector3(vec.x, vec.y, 3), vec.z, loops, duration);
                break;

            case MoveType.TweenMoveJumpLocal:
                obj.transform.DOLocalJump(new Vector3(vec.x, vec.y, 3), vec.z, loops, duration);
                break;

            case MoveType.TweenRotate:
                obj.transform.DORotate(vec, duration).SetEase(easeType).SetLoops(loops, loopType);
                break;

            case MoveType.TweenScale:
                obj.transform.DOScale(vec, duration).SetEase(easeType).SetLoops(loops, loopType);
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
                obj.GetComponent<SpriteRenderer>().DOFade(1, duration).SetEase(easeType).OnComplete(() =>
                obj.GetComponent<SpriteRenderer>().DOFade(0, duration).SetEase(easeType));
                break;

            case MoveType.CameraSetScale:
                GameManager.instance.mainCamera.orthographicSize = vec.x;
                break;

            case MoveType.CameraSmoothScale:
                GameManager.instance.mainCamera.DOFieldOfView(vec.x, duration).SetEase(easeType);
                break;

            case MoveType.CameraBop:
                GameManager.instance.mainCamera.DOFieldOfView(vec.x, duration).SetEase(easeType).OnComplete(() =>
                GameManager.instance.mainCamera.DOFieldOfView(vec.y, duration).SetEase(easeType));
                break;

            case MoveType.CameraBopRepeat:
                yVal = GameManager.instance.GameTime;
                for (int i = 0; i < loops; i++)
                {
                    CameraRepeat(vec.z / (GameManager.instance.bs.bpm / 30f) * i, duration);
                }
                break;

            case MoveType.ScreenShake:
                GameManager.instance.mainCamera.DOShakePosition(duration, vec.x, Mathf.RoundToInt(vec.y), vec.z).SetEase(easeType);
                break;
            case MoveType.ActivateObject:
                obj.SetActive(true);
                break;
            case MoveType.DeactivateObject:
                obj.SetActive(false);
                break;
        }
    }

    

    private async void CameraRepeat(float waitTime, float duration)
    {
        var end = Time.time + waitTime;

        while (Time.time < end)
        {
            await Task.Yield();
        }

        if (GameManager.instance != null && Mathf.Abs(yVal+waitTime-GameManager.instance.GameTime) <= .5f)
            GameManager.instance.mainCamera.DOFieldOfView(vec.x, duration).SetEase(easeType).OnComplete(() =>
            GameManager.instance.mainCamera.DOFieldOfView(vec.y, duration).SetEase(easeType));
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
    ActivateObject,
    DeactivateObject,
    Bezier
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
