using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class ColorChange : MonoBehaviour
{
    [SerializeField]
    private Gradient colorGradient;
    [SerializeField]
    private float speed;

    private RawImage image;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        image.color = colorGradient.Evaluate((timer * speed) % 1);
    }
}
