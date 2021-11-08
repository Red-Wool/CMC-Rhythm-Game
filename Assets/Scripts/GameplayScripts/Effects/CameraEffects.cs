using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    private Camera mainCamera;
    private const float baseCameraSize = 7f;

    private float cameraSizeMultiplier = 1f;
    private float screenBopintensity = 1f;
    private float screenBopTimeTotal = 1f;
    private float screenBopTimer = 1f;

    [SerializeField]
    private AnimationCurve screenBop;
    //private AnimationCurve
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        ScreenBop(1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (screenBopTimer >= 1f)
        {
            ScreenBop(-0.2f, 0.75f);
        }
        screenBopTimer += Time.deltaTime / screenBopTimeTotal;

        cameraSizeMultiplier = baseCameraSize * (1 + (screenBop.Evaluate(screenBopTimer) * screenBopintensity));
        mainCamera.orthographicSize = cameraSizeMultiplier;
    }

    public void ScreenBop(float intesity, float time)
    {
        screenBopintensity = intesity;
        screenBopTimeTotal = time;

        screenBopTimer = 0;
    }
}
