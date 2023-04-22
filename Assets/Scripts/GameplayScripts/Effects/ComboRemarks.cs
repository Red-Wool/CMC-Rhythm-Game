using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ComboRemarks : MonoBehaviour
{
    [SerializeField] private Image remarkImage;
    [SerializeField] private Image remarkModImage;
    [SerializeField] private Sprite[] remarks;
    [SerializeField] private Sprite[] remarkMod;

    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpTime;
    [SerializeField] private float remarkTime;

    private Vector3 savePos;
    private IEnumerator timer;

    public void Start()
    {
        remarkImage.DOFade(0, 0f);
        remarkModImage.DOFade(0, 0f);
        savePos = remarkImage.transform.position;
    }

    public void Remark(int i)
    {
        remarkImage.sprite = remarks[i % remarks.Length];
        remarkImage.transform.DOJump(savePos, jumpHeight, 1, jumpTime);
        remarkImage.DOFade(1, .1f);

        int mod = i / remarks.Length;
        if (mod == 0)
        {
            remarkModImage.color = new Color(1, 1, 1, 0);
        }
        else
        {
            remarkModImage.DOFade(1, .1f);
            remarkModImage.sprite = remarkMod[(mod - 1) % remarkMod.Length];
        }

        if (timer != null)
        {
            StopCoroutine(timer);
        }
        timer = RemarkTimer();
        StartCoroutine(timer);
    }

    private IEnumerator RemarkTimer()
    {
        yield return new WaitForSeconds(remarkTime);
        remarkImage.DOFade(0, .1f);
        remarkModImage.DOFade(0, .1f);
    }
}

/*[System.Serializable]
public class RemarkAnim
{
    public Sprite remark;

    
}*/
