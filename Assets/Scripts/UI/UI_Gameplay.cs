using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UI_Gameplay : MonoBehaviour
{
    #region Gameplay Variables
    [SerializeField] public CanvasGroup BlackScn, GameOver;
    [SerializeField] public GameObject PauseMenu, PMBox, Blackscreen;

    private IEnumerator menuScreen;
    private IEnumerator exit;
    private IEnumerator restart;
    private IEnumerator ending;

    public bool isMenuOpened = false;

    public PlayerInput input;
    #endregion

    #region Start Scene Variables
    [SerializeField] GameObject IntroScene, blackTop, blackBottom, dialogue;
    public bool dInteract;
    public bool canUseMenu;

    private Tutorial tutorial;
    #endregion

    public void Awake()
    {
        tutorial = gameObject.GetComponent<Tutorial>();
    }

    public void Start()
    {
        canUseMenu = true;
        input.currentActionMap.Disable();
        StartScene();
    }

    #region Pause Menu Animations
    public void OpenMenu()
    {
        canUseMenu = false;
        Time.timeScale = 0f;
        input.SwitchCurrentActionMap("Menu");
        PauseMenu.SetActive(true);
        Blackscreen.SetActive(true);
        LeanTween.moveLocal(PMBox, new Vector3(0f, 0f, 0f), 0.75f).setDelay(0f).setIgnoreTimeScale(true).setEase(LeanTweenType.easeOutCirc).setOnComplete(OnFinishTransition);
        isMenuOpened = true;
    }

    public void CloseMenu()
    {
        OnFinishTransition();
        Time.timeScale = 1f;
        input.SwitchCurrentActionMap("Gameplay");
        PauseMenu.SetActive(false);
        isMenuOpened = false;
    }

    public void Resume()
    {
        canUseMenu = false;
        Blackscreen.SetActive(true);
        LeanTween.moveLocal(PMBox, new Vector3(0f, 900f, 0f), 0.75f).setDelay(0f).setIgnoreTimeScale(true).setEase(LeanTweenType.easeOutQuint).setOnComplete(CloseMenu);
    }

    public void MainMenu()
    {
        AudioManager.instance.Stop();
        Blackscreen.SetActive(true);
        LeanTween.alphaCanvas(BlackScn, 1f, 1f).setDelay(0f).setIgnoreTimeScale(true).setEase(LeanTweenType.easeInQuad).setOnComplete(Return);
    }

    public void Restart()
    {
        AudioManager.instance.Stop();
        input.actions.Disable();
        Blackscreen.SetActive(true);
        LeanTween.alphaCanvas(BlackScn, 1f, 1f).setDelay(1.5f).setIgnoreTimeScale(true).setEase(LeanTweenType.easeInQuad);
        LeanTween.alphaCanvas(GameOver, 1f, 1f).setDelay(3f).setIgnoreTimeScale(true).setEase(LeanTweenType.easeInQuad);
        LeanTween.alphaCanvas(GameOver, 0f, 1f).setDelay(6f).setIgnoreTimeScale(true).setEase(LeanTweenType.easeInQuad).setOnComplete(ReloadLevel);

    }

    public void Complete()
    {
        AudioManager.instance.Stop();
        input.actions.Disable();
        Blackscreen.SetActive(true);
        LeanTween.alphaCanvas(BlackScn, 1f, 1f).setDelay(1.5f).setIgnoreTimeScale(true).setEase(LeanTweenType.easeInQuad).setOnComplete(LoadEnding);
    }

    public void Quit()
    {
        AudioManager.instance.Stop();
        Blackscreen.SetActive(true);
        LeanTween.alphaCanvas(BlackScn, 1f, 1.5f).setDelay(0f).setIgnoreTimeScale(true).setEase(LeanTweenType.easeInQuad).setOnComplete(Exit);
    }
    #endregion

    #region Pause Menu Functions / Coroutines
    public void Return()
    {
        menuScreen = GoToMainMenu();
        StartCoroutine(menuScreen);
    }

    public void Exit()
    {
        exit = ExitGame();
        StartCoroutine(exit);
    }

    public void ReloadLevel()
    {
        restart = RestartLevel();
        StartCoroutine(restart);
    }

    public void LoadEnding()
    {
        ending = GoToEnding();
        StartCoroutine(ending);
    }

    public void OnFinishTransition() 
    {
        Blackscreen.SetActive(false);
        canUseMenu = true;
    }

    private IEnumerator GoToMainMenu()
    {
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;
        input.SwitchCurrentActionMap("Gameplay");
        SceneManager.LoadScene(1);
        yield return null;
    }

    private IEnumerator ExitGame()
    {
        yield return new WaitForSecondsRealtime(1f);
        Application.Quit();
        yield return null;
    }

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSecondsRealtime(2f);
        input.actions.Enable();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return null;
    }

    private IEnumerator GoToEnding()
    {
        yield return new WaitForSecondsRealtime(1f);
        input.actions.Enable();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        yield return null;
    }
    #endregion

    #region Start Scene Animations
    public void StartScene()
    {
        AudioManager.instance.PlaySFX("SFX_Warning");
        LeanTween.moveLocal(dialogue, new Vector3(0f, 0f, 0f), 1f).setDelay(0f).setEase(LeanTweenType.easeOutBack).setOnComplete(EnableInteraction);
    }

    public void Interact()
    {
        if (dInteract)
        {
            dInteract = false;
            EndScene();
        }
    }

    public void EndScene()
    {
        //Start Gameplay Music
        LeanTween.moveLocal(dialogue, new Vector3(0f, -300f, 0f), 1f).setDelay(0f).setEase(LeanTweenType.easeInBack);
        LeanTween.moveLocal(blackTop, new Vector3(0f, 850f, 0f), 1.5f).setDelay(2f).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.moveLocal(blackBottom, new Vector3(0f, -850f, 0f), 1.5f).setDelay(2f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(StartGameplay);
    }

    public void StartGameplay()
    {
        AudioManager.instance.Play("Gameplay");
        tutorial.Show();
        input.SwitchCurrentActionMap("Gameplay");
    }

    public void EnableInteraction() => dInteract = true;
    #endregion
}
