using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    [SerializeField] private bool useStar;
    [SerializeField] private StarTransition starTransition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene()
    {
        //DOTween.KillAll();
        DOTween.CompleteAll();
        DOTween.Clear();
        DOTween.ClearCachedTweens();

        StopAllCoroutines();
        

        

        if (useStar)
        {
            StartCoroutine(LoadSceneAnim());
            
        }
        else
        {

            Time.timeScale = 1;
            SceneManager.LoadScene(sceneName);
        }
    }

    public IEnumerator LoadSceneAnim()
    {
        starTransition.OpenStar(.5f, .01f);

        yield return new WaitForSecondsRealtime(1f);
        DOTween.CompleteAll();
        DOTween.Clear();
        DOTween.ClearCachedTweens();

        StopAllCoroutines();
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
}
