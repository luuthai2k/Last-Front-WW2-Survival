using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MachineGunControlTutorial : MonoBehaviour
{
    public RectTransform handRectTransform, targetSwipeRectTransform, targetShootRectTransform;
    public TextMeshProUGUI aimAtTargetTxt, holdToShootTxt;
    public Animator handAnimController;
    private TextMeshProUGUI currentTxt;
    private LevelControl levelControl;
    private void Start()
    {
        levelControl = GameManager.Instance.levelControl;
    }
    public void AimAtTarget(bool isActiveHand = false)
    {
        if (currentTxt == aimAtTargetTxt) return;
        transform.GetChild(0).gameObject.SetActive(true);
        if (currentTxt != null)
        {
            var lastTxt = currentTxt;
            lastTxt.DOFade(0, 0.5f);
        }

        currentTxt = aimAtTargetTxt;
        currentTxt.DOFade(1, 0.5f);
        if (isActiveHand)
        {
            handAnimController.gameObject.SetActive(true);
            StartCoroutine(HandSwipe());
        }
        else
        {
            handAnimController.gameObject.SetActive(false);
        }

    }
    public void HoldToShoot()
    {
        if (currentTxt == holdToShootTxt) return;
        transform.GetChild(0).gameObject.SetActive(true);
        if (currentTxt != null)
        {
            var lastTxt = currentTxt;
            lastTxt.DOFade(0, 0.5f);
        }
        currentTxt = holdToShootTxt;
        currentTxt.DOFade(1, 0.5f);
        handAnimController.gameObject.SetActive(true);
        StartCoroutine(HandShoot());
    }
    IEnumerator HandShoot()
    {
        handAnimController.SetBool("Swipe", false);
        handAnimController.SetBool("Click", false);
        handRectTransform.DOKill();
        handRectTransform.DOMove(targetShootRectTransform.position, 0.5f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.5f);
        handAnimController.SetBool("Shoot", true);

    }
    IEnumerator HandSwipe()
    {
        handAnimController.SetBool("Shoot", false);
        handAnimController.SetBool("Click", false);
        handRectTransform.DOKill();
        handRectTransform.DOMove(targetSwipeRectTransform.position, 0.5f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.5f);
        handAnimController.SetBool("Swipe", true);

    }

    public void Update()
    {
        if (levelControl.currentWave == 1)
        {
            gameObject.SetActive(false);
            PlayerPrefs.SetInt(GameConstain.MACHINEGUN_CONTROL_TUT, 1);
        }
        switch (PlayerController.Instance.currentState)
        {
            case PlayerState.Move:
                transform.GetChild(0).gameObject.SetActive(false);
                break;
            case PlayerState.Cover:
                HoldToShoot();
                break;
            case PlayerState.Shoot:
                AimAtTarget();
                break;
        }
    }
   
}
