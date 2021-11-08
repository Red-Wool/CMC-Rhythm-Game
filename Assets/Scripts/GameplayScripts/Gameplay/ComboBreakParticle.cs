using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI), typeof(Rigidbody2D))]
public class ComboBreakParticle : MonoBehaviour
{
    private float timer;

    // Start is called before the first frame update
    private void Update()
    {
        if (gameObject.activeSelf)
        {
            timer += Time.deltaTime;

            if (timer > 3f)
            {
                gameObject.SetActive(false);    
            }
        }
        
    }

    public void Spawn(Vector2 vel)
    {
        gameObject.SetActive(true);

        GetComponent<Rigidbody2D>().velocity = vel;

        timer = 0f;
    }
}
