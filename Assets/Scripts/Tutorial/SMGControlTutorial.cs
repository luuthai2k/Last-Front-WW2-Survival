using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class SMGControlTutorial : MonoBehaviour
{
    public RectTransform handRectTransform, targetSwipeRectTransform, targetShootRectTransform, targetGrenadeRectTransform;
    public TextMeshProUGUI aimAtTargetTxt, holdToShootTxt, clickToThrowGrenadeTxt;
    public Animator handAnimController;
    private TextMeshProUGUI currentTxt;
    private LevelControl levelControl;
    int step = 0;
    private void Start()
    {
        levelControl = GameManager.Instance.levelControl;
        PlayerUIControl.Instance.goShootBtn.SetActive(false);
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
    public void ClickToThrowGrenade()
    {
        if (currentTxt == clickToThrowGrenadeTxt) return;
        transform.GetChild(0).gameObject.SetActive(true);
        if (currentTxt != null)
        {
            var lastTxt = currentTxt;
            lastTxt.DOFade(0, 0.5f);
        }
        currentTxt = clickToThrowGrenadeTxt;
        currentTxt.DOFade(1, 0.5f);
        handAnimController.gameObject.SetActive(true);
        StartCoroutine(HandGranade());
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
    IEnumerator HandGranade()
    {
        handAnimController.SetBool("Swipe", false);
        handAnimController.SetBool("Shoot", false);
        handRectTransform.DOKill();
        handRectTransform.DOMove(targetGrenadeRectTransform.position, 0.5f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.5f);
        handAnimController.SetBool("Click", true);

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
        switch (step)
        {
            case 0:
                Step0();
                break;
            case 1:
                Step1();
                break;
            case 2:
                Step2();
                break;
            case 3:
                Step3();
                break;
        }
    }
    void Step0()
    {
        AimAtTarget(true);
        step++;
    }
    void Step1()
    {
        if (CameraManager.Instance.IsAimTarget(LayerConfig.Instance.shootMask))
        {
            HoldToShoot();
            PlayerUIControl.Instance.goShootBtn.SetActive(true);
            step++;
        }
    }
    void Step2()
    {
        if (levelControl.currentEnemyWave == 2)
        {
            step++;
            DataController.Instance.Grenade = 1;
        }
        if (PlayerController.Instance.currentState == PlayerState.Cover)
        {

            HoldToShoot();

        }
        else
        {
            AimAtTarget(false);
        }
    }
    void Step3()
    {
        if (DataController.Instance.Grenade == 0)
        {
            DataController.Instance.Level = 1;
            gameObject.SetActive(false);
        }
        PlayerUIControl.Instance.isAim = false;
        PlayerUIControl.Instance.goShootBtn.SetActive(false);
        Transform target = GameManager.Instance.levelControl.FindClosestEnemy();
        if (target != null)
        {
            ClickToThrowGrenade();
        }
        else
        {
            AimAtTarget(true);
        }
    }
   

}
