using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using TMPro;
using UnityEngine;

public class GreetingTutorialState : State
{
    [SerializeField]
    private int dialogType = 0;

    [SerializeField]
    private float delay;

    [SerializeField]
    private GameObject[] childs;

    [SerializeField]
    private bool shouldPauseGame = true;

    [SerializeField]
    private float delayPauseGame;

    private bool hasClickChangeState = false;

    private void OnEnable()
    {
        hasClickChangeState = false;
        if (shouldPauseGame && delayPauseGame == 0)
            Time.timeScale = 0;
        else
            StartCoroutine(DelayPauseGame(shouldPauseGame, delayPauseGame));
        foreach (GameObject child in childs) child.SetActive(true);
    }

    public void OnClickChangeState()
    {
        if (hasClickChangeState) return;
        hasClickChangeState = true;
        Time.timeScale = 1;
        foreach (GameObject child in childs) child.SetActive(false);
        StartCoroutine(DelayChangeState());
    }

    private IEnumerator DelayChangeState()
    {
        yield return new WaitForSecondsRealtime(delay);
        gameObject.SetActive(false);
        Time.timeScale = 1;
        Next();
    }

    private IEnumerator
    DelayPauseGame(bool shouldPauseGame, float delayPauseGame)
    {
        yield return new WaitForSeconds(delayPauseGame);
        Time.timeScale = shouldPauseGame ? 0 : 1;
    }
}
