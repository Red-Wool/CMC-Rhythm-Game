using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class NoteButton : MonoBehaviour
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

    //Move Module Stuff
    public List<ArrowPathModule> moveModules;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        avalible = true;

        for (int i = 0; i < moveModules.Count; i++)
        {
            moveModules[i].RequestData();
        }
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

    public void AddModule(ArrowPathModule module)
    {
        for (int i = 0; i < moveModules.Count; i++)
        {
            if (moveModules[i].objID == module.objID && moveModules[i].notePosID == module.notePosID)
            {
                //float change = 0;
                //Debug.Log("Change it up");
                int o = i;

                DOTween.To(() => moveModules[o].stats.speed,
                        x => moveModules[o].stats.speed = x,
                        module.stats.speed, module.stats.duration).SetEase(module.stats.easeType);//.OnUpdate(() => Debug.Log(moveModules[i].stats.speed));

                for (int j = 0; j < moveModules[i].stats.store.Length; j++)
                {
                    
                    int t = j;
                    //Debug.Log("killer");
                    DOTween.To(() => moveModules[o].stats.store[t],
                        x => moveModules[o].stats.store[t] = x,
                        module.stats.store[t], module.stats.duration).SetEase(module.stats.easeType);//.OnUpdate(() => Debug.Log(moveModules[o].stats.store[t]));
                }
                return;
            }
        }
        moveModules.Add(module);
        module.isActive = true;
    }

    public void DisableModule(string notePosID, int objID)
    {
        foreach(ArrowPathModule module in moveModules)
        {
            if (module.notePosID == notePosID && module.objID == objID)
                module.isActive = false;
        }
    }

    public void EnableModule(string notePosID, int objID)
    {
        foreach (ArrowPathModule module in moveModules)
        {
            if (module.notePosID == notePosID && module.objID == objID)
                module.isActive = true;
        }
    }

    public Vector3 SetPosition(float time)
    {
        Vector3 total = Vector3.zero;
        for (int i = 0; i < moveModules.Count; i++)
        {
            //Debug.Log(moveModules[i].notePosID + " " + moveModules[i].notePos);
            total += moveModules[i].CalculateNotePosition(time);
        }
        return total;
    }

    //private void OnCollisionStay(Collision collision)
    //{
        //if (collision.gameObject.tag == "Arrow")
        //{

        //}
    //}
}
