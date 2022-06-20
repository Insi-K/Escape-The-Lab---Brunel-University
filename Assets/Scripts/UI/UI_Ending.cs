using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Ending : MonoBehaviour
{
    [SerializeField] public GameObject Title;
    [SerializeField] public CanvasGroup TitleScn, TitleBody;

    private IEnumerator menu;

    public void Start()
    {
        ShowEnding();
    }

    public void ShowEnding()
    {
        LeanTween.alphaCanvas(TitleScn, 1f, 1.5f).setDelay(1f).setEase(LeanTweenType.easeOutCubic);
        LeanTween.moveLocal(Title, new Vector3(0,50f,0), 1f).setDelay(2.5f).setEase(LeanTweenType.easeInOutQuint);
        LeanTween.alphaCanvas(TitleBody, 1f, 1.5f).setDelay(3.5f).setEase(LeanTweenType.easeOutCubic).setOnComplete(HideEnding);
    }

    public void HideEnding()
    {
        LeanTween.alphaCanvas(TitleScn, 0f, 1.5f).setDelay(3f).setEase(LeanTweenType.easeOutCubic).setOnComplete(FinishScene);
    }

    public void FinishScene()
    {
        menu = GoToMainMenu();
        StartCoroutine(menu);
    }

    private IEnumerator GoToMainMenu()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(1);
        yield return null;
    }

}
