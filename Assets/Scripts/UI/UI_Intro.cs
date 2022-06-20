using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Intro : MonoBehaviour
{
    [SerializeField] CanvasGroup StartScn, SecondScn, StartBody, SecondBody;
    [SerializeField] GameObject StartTitle, SecondTitle;

    public float moveDuration = 1f;
    public float fadeDuration = 1.5f;
    public float hideDelay = 6f;

    private IEnumerator menu;

    // Start is called before the first frame update
    void Start()
    {
        StartScn.alpha = 0f;
        SecondScn.alpha = 0f;
        StartScreen();
    }

    public void StartScreen()
    {
        LeanTween.alphaCanvas(StartScn, 1f, fadeDuration).setDelay(1f).setEase(LeanTweenType.easeOutCubic).setOnComplete(animateStart);
    }
    //Move
    public void animateStart()
    {
        LeanTween.moveLocal(StartTitle, new Vector3(0, 200f, 0), moveDuration).setDelay(0f).setEase(LeanTweenType.easeInOutQuint).setOnComplete(showFirstBody);
    }
    public void showFirstBody()
    {
        LeanTween.alphaCanvas(StartBody, 1f, fadeDuration).setDelay(0.1f).setEase(LeanTweenType.easeOutCubic).setOnComplete(HideFirst);
    }
    public void HideFirst()
    {
        LeanTween.alphaCanvas(StartScn, 0f, fadeDuration).setDelay(hideDelay).setEase(LeanTweenType.easeOutCubic).setOnComplete(SecondScreen);
    }

    public void SecondScreen()
    {
        LeanTween.alphaCanvas(SecondScn, 1f, fadeDuration).setDelay(1f).setEase(LeanTweenType.easeOutCubic).setOnComplete(animateSecond);
    }

    //Move
    public void animateSecond()
    {
        LeanTween.moveLocal(SecondTitle, new Vector3(0, 150f, 0), moveDuration).setDelay(0f).setEase(LeanTweenType.easeOutQuint).setOnComplete(showSecondBody);
    }

    public void showSecondBody()
    {
        LeanTween.alphaCanvas(SecondBody, 1f, fadeDuration).setDelay(0.1f).setEase(LeanTweenType.easeOutCubic).setOnComplete(HideSecond);
    }

    public void HideSecond()
    {
        LeanTween.alphaCanvas(SecondScn, 0f, fadeDuration).setDelay(hideDelay).setEase(LeanTweenType.easeOutCubic).setOnComplete(GoToMenu);
    }

    public void GoToMenu()
    {
        menu = Menu();
        StartCoroutine(menu);
    }

    public IEnumerator Menu()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        yield return null;
    }

    
}
