using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ButtonController : MonoBehaviour
{
    private SpriteRenderer sr;

    //If can Press
    [Header("If can Press")]
    public bool avalible;

    [Header("Sprite Asset"), Space(10)]
    public Sprite defaultImg;
    public Sprite pressedImg;

    [Header("Keybind"), Space(10)]
    public KeyCode keyPress;
    public KeyCode altKeyPress;

    public ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        avalible = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Shows different images if pressing or not
        if (Input.GetKeyDown(keyPress) || Input.GetKeyDown(altKeyPress))
        {
            ps.gameObject.transform.position = this.transform.position;
            ParticleManager.instance.ToggleParticle(true, true, ps);
            ps.Play();
        }
        else if (Input.GetKey(keyPress) || Input.GetKey(altKeyPress))
        {
            sr.sprite = pressedImg;
        }
        else
        {
            sr.sprite = defaultImg;

            ParticleManager.instance.ToggleParticle(false, true, ps);
        }
    }

    //private void OnCollisionStay(Collision collision)
    //{
        //if (collision.gameObject.tag == "Arrow")
        //{

        //}
    //}
}
