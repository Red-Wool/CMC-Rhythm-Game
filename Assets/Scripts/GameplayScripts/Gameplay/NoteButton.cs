using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        moveModules.Add(module);
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
