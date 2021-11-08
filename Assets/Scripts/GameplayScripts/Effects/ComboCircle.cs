using UnityEngine;
using UnityEngine.UI;

public class ComboCircle : MonoBehaviour
{
    [Header("Combo Circle Reference")]
    public Image comboCircle;

    [Header("Speed"), Space(10)]
    [Range(0f, 1f)]
    public float percentSmoothSpeed;

    private float target;

    [Header("Particle Systems"), Space(10)]
    public ParticleSystem multUpPS;

    public ParticleSystem maxMultPS;

    [Header("Color Effect"), Space(10), SerializeField]
    private Gradient colorPulse;
    [SerializeField]
    private float colorPulseSpd;
    private float colorPulseTimer;

    //public ParticleSystem comboBreakPS;

    private bool multUp;
    private bool setMax;

    // Start is called before the first frame update
    void Start()
    {
        multUp = false;
        setMax = false;

        //maxMultSupportPS = maxMultPS.GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        comboCircle.fillAmount = Mathf.Lerp(comboCircle.fillAmount, target, percentSmoothSpeed);

        if (colorPulseTimer < 1f)
        {
            colorPulseTimer += colorPulseSpd * Time.deltaTime;

            comboCircle.color = colorPulse.Evaluate(colorPulseTimer);

            if (setMax)
            {
                colorPulseTimer %= 1f;
            }
        }
            
    }

    public void UpdateComboCircle(int combo, int[] cmi) //cmi = comboMultiplierInterval
    {
        colorPulseTimer = 0f;

        //Figure out how much of the circle needs to be filled out
        for (int i = cmi.Length - 1; i >= 0; i--)
        {
            if (combo >= cmi[i])
            {
                if (i == cmi.Length - 1)
                {
                    if (!setMax)
                    {
                        //Debug.Log("Max PALKWJDHYGEOWER");

                        target = 1f;
                        setMax = true;

                        SetStaticActive(true);
                    }

                    break;
                }
                else if (setMax && i != cmi.Length - 1)
                {
                    setMax = false;

                    SetStaticActive(false);
                }
                else if (combo == cmi[i] && i != 0)
                {
                    if (!multUp)
                    {
                        multUp = true;

                        multUpPS.Play();
                    }
                }
                else
                {
                    multUp = false;
                }

                //Debug.Log(cmi[i]);
                //Debug.Log(cmi[i] + " " + combo);
                //Debug.Log(Mathf.Abs(cmi[i] - combo) + " " + (cmi[i + 1] - cmi[i]));
                //Debug.Log((Mathf.Abs(cmi[i] - combo) / (cmi[i + 1] - cmi[i])));

                target = ((Mathf.Abs(cmi[i] - combo) / (float)(cmi[i + 1] - cmi[i])));

                break;
            }
        }
    }

    private void SetStaticActive(bool flag)
    {
        ParticleManager.instance.ToggleParticle(flag, true, maxMultPS);

        if (flag)
        {
            multUpPS.Play();
        }
    }
}


