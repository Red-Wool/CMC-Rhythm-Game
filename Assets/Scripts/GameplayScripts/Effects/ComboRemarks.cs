using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ComboRemarks : MonoBehaviour
{
    [SerializeField] private Image remarkImage;
    [SerializeField] private Sprite[] remarks;

    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpTime;
    [SerializeField] private float remarkTime;

    private Vector3 savePos;
    private IEnumerator timer;

    public void Start()
    {
        remarkImage.gameObject.SetActive(false);
        savePos = remarkImage.transform.position;
    }

    public void Remark(int i)
    {
        remarkImage.sprite = remarks[i % remarks.Length];
        remarkImage.gameObject.SetActive(true);
        remarkImage.transform.DOJump(savePos, jumpHeight, 1, jumpTime);

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
        remarkImage.gameObject.SetActive(false);
    }
}

/*[System.Serializable]
public class RemarkAnim
{
    public Sprite remark;

    
}*/
