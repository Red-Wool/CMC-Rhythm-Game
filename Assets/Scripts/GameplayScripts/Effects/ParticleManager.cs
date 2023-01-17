using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    //Reference for everyone
    [HideInInspector]
    public static ParticleManager instance;

    [Header("Hit Note Particle Systems")]
    public PSArray[] noteHitPS;
    [Header("Mistime Note Particle Systems"), Space(10)]
    public PSArray[] noteMistimePS;

    [Header("Other Particle Systems"), Space(10)]
    public ParticleSystem comboBreakPS;

    //Extra Varibles declared only once to save memory
    private ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayParticle(Vector3 pos, NoteColor noteCol, NoteType noteType, HitText hitType)
    {
        ps = ((int)hitType >= 4) ?
            noteMistimePS[(int)noteType].particleSystem[(int)noteCol] : 
            noteHitPS[(int)noteType].particleSystem[(int)noteCol];

        ps.transform.position = pos;

        ps.Play(hitType == HitText.Perfect);
    }

    public void ToggleParticle(bool flag, bool activateChildren, ParticleSystem particle)
    {
        var mainPS = particle.main;
        mainPS.loop = flag;

        foreach (ParticleSystem i in particle.transform.GetComponentsInChildren<ParticleSystem>())
        {
            var supportPS = i.main;
            supportPS.loop = flag;
        }

        if (flag && !particle.isPlaying)
        {
            particle.Play(activateChildren);
        }
    }

    public void ToggleParticle(bool flag, Vector3 pos, NoteColor noteCol, NoteType noteType, HitText hitType)
    {
        ps = ((int)hitType >= 4) ?
            noteMistimePS[(int)noteType].particleSystem[(int)noteCol] :
            noteHitPS[(int)noteType].particleSystem[(int)noteCol];

        ps.transform.position = pos;

        ToggleParticle(flag, hitType == HitText.Perfect, ps);
    }
    
    public void PlayBreak(bool flag, int combo)
    {
        var emission = comboBreakPS.emission;

        emission.rateOverTime = Mathf.Min(combo, 150) * 4;

        comboBreakPS.Play(flag);
    }

}

[System.Serializable]
public struct PSArray
{
    public string name;
    public ParticleSystem[] particleSystem;
}

[System.Serializable]
public enum NoteColor
{
    Red = 0,
    Blue = 1,
    Green = 2,
    Yellow = 3
}