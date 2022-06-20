using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Tween : MonoBehaviour
{

    [SerializeField] GameObject backPanel, title, play, quit, crates, blackScreen, review, credits, creditsBox, mainBox;
    [SerializeField] CanvasGroup blackScn;
    public string gameReviewURL;

    private IEnumerator level;
    private IEnumerator exit;

    #region Unity Callback Functions
    void Start()
    {
        AudioManager.instance.Play("MenuTrack");
        StartMenu();
    }
    #endregion

    #region UI Animations
    //Starting Animation (Performed in order)
    void StartMenu()
    {
        blackScreen.SetActive(true);
        LeanTween.alphaCanvas(blackScn, 0f, 1.2f).setDelay(0.5f).setEase(LeanTweenType.easeInQuad).setOnComplete(ShowTitle);
    }

    void ShowTitle()
    {
        LeanTween.moveLocal(backPanel, new Vector3(0f, 160f, 0f), 1.1f).setDelay(0.2f).setEase(LeanTweenType.easeOutBack);
        LeanTween.moveLocal(title, new Vector3(0f, 160f, 0f), 1.1f).setDelay(0.2f).setEase(LeanTweenType.easeOutBack).setOnComplete(ShowButtons);
    }

    void ShowButtons()
    {
        LeanTween.moveLocal(play, new Vector3(0, -100, 0), 1.3f).setDelay(0.5f).setEase(LeanTweenType.easeOutQuint);
        LeanTween.moveLocal(quit, new Vector3(0, -300, 0), 1.3f).setDelay(0.5f).setEase(LeanTweenType.easeOutQuint).setOnComplete(ShowCrates);
        LeanTween.moveLocal(review, new Vector3(200, -450, 0), 1.3f).setDelay(0.5f).setEase(LeanTweenType.easeOutQuint);
        LeanTween.moveLocal(credits, new Vector3(-200, -450, 0), 1.3f).setDelay(0.5f).setEase(LeanTweenType.easeOutQuint);
    }

    void ShowCrates()
    {
        crates.SetActive(true);
        blackScreen.SetActive(false);
    }

    //Black Screen - Used at the beginning and end of the animation menu
    public void StartBlackout()
    {
        AudioManager.instance.Stop();
        blackScreen.SetActive(true);
        LeanTween.alphaCanvas(blackScn, 1f, 1.2f).setDelay(0f).setEase(LeanTweenType.easeInQuad).setOnComplete(StartGame);
    }

    public void QuitBlackout()
    {
        AudioManager.instance.Stop();
        blackScreen.SetActive(true);
        LeanTween.alphaCanvas(blackScn, 1f, 1.2f).setDelay(0f).setEase(LeanTweenType.easeInQuad).setOnComplete(QuitGame);
    }
    #endregion

    #region Menu Functions
    public void StartGame()
    {
        level = LoadLevel();
        StartCoroutine(level);
    }

    public void QuitGame()
    {
        exit = ExitGame();
        StartCoroutine(exit);
    }

    public void OpenQuestionnaire()
    {
        Application.OpenURL(gameReviewURL);
    }

    private IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        yield return null;
    }

    private IEnumerator ExitGame()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Exit Game");
        Application.Quit();
        yield return null;
    }

    public void ShowCreditsBox()
    {
        mainBox.SetActive(false);
        creditsBox.SetActive(true);
    }

    public void HideCreditsBox()
    {
        creditsBox.SetActive(false);
        mainBox.SetActive(true);
    }
    #endregion
}
