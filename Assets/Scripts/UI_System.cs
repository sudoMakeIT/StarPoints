using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_System : MonoBehaviour
{
    [Header("System Events")]

    [Header("Main Properties")]
    public UI_Screen startScreen;

    public UnityEvent onSwitchedScreen = new UnityEvent();

    [Header("Fader Properties")]
    public Image fader;
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;

    private Component[] screens = new Component[0];
    private UI_Screen prevScreen;
    public UI_Screen PrevScreen { get { return prevScreen; } }
    private UI_Screen currScreen;
    public UI_Screen CurrScreen { get { return currScreen; } }

    // Start is called before the first frame update
    void Start()
    {
        screens = GetComponentsInChildren<UI_Screen>(true);

        if (startScreen)
        {
            SwitchScreen(startScreen);
        }

        if (fader)
        {
            fader.gameObject.SetActive(true);
        }

        FadeIn();
    }

    public void SwitchScreen(UI_Screen aScreen)
    {
        if (aScreen)
        {
            if (currScreen)
            {
                currScreen.CloseScreen();
                prevScreen = currScreen;
            }

            currScreen = aScreen;
            currScreen.gameObject.SetActive(true);
            currScreen.StartScreen();

            if (onSwitchedScreen != null)
            {
                onSwitchedScreen.Invoke();
            }
        }
    }

    public void FadeIn()
    {
        if (fader)
        {
            fader.CrossFadeAlpha(0f, fadeInDuration, false);
        }
    }

    public void FadeOut()
    {
        if (fader)
        {
            fader.CrossFadeAlpha(1f, fadeOutDuration, false);
        }
    }

    public void GoToPreviousScreen()
    {
        if (prevScreen)
        {
            SwitchScreen(prevScreen);
        }
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(WaitToLoadScene(sceneIndex));
    }

    IEnumerator WaitToLoadScene(int sceneIndex)
    {
        yield return null;
    }
}
