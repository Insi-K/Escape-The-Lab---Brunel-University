using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    //Reference to UI Object and text
    //public Tutorial ID (int) - This script will be added to each TutorialTriggerBox in level
    public GameObject tutorialUI;
    private Text tutorialTxt;

    public float startDelay;
    public float showDuration;
    public string tutorialSentence;

    private bool hasBeenActivated;

    //Generate all UI text animations here
    private void Awake()
    {
        hasBeenActivated = false;
        tutorialTxt = tutorialUI.GetComponentInChildren<Text>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Player player))
        {
            if(!hasBeenActivated)
            {
                hasBeenActivated = true;
                Show();
            }
        }
    }

    //Show Animation should first change position to Starting position (this if a second animation is triggered during previous animation)

    public void Show()
    {
        LeanTween.cancel(tutorialUI);
        LeanTween.moveLocal(tutorialUI, new Vector3(0, 600, 0), 0f);
        tutorialTxt.text = tutorialSentence;
        LeanTween.moveLocal(tutorialUI, new Vector3(0, 250, 0), 1f).setDelay(startDelay).setEase(LeanTweenType.easeOutCubic).setOnComplete(Hide);
    }

    public void Hide()
    {
        LeanTween.moveLocal(tutorialUI, new Vector3(0, 600, 0), 1f).setDelay(showDuration).setEase(LeanTweenType.easeOutCubic).setOnComplete(Hide);
    }

}
