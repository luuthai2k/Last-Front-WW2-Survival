using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TutorialUIController : MonoBehaviour
{
    public RectTransform handRectTransform,targetSwipeRectTransform, targetShootRectTransform, targetGrenadeRectTransform, targetHealRectTransform;
    public TextMeshProUGUI aimAtTargetTxt, holdToShootTxt, holdToAimTxt, releaseToShootTxt,clickToThrowGrenadeTxt,clickToHealTxt;
    public Animator handAnimController;
    private TextMeshProUGUI currentTxt;
    private LevelControl levelControl;
    private bool isGrenadeTutDone, isHealTutDone;
    private void Start()
    {
        levelControl = GameManager.Instance.levelControl;
    }
    public void AimAtTarget(bool isActiveHand=false)
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
    public void ClickToHeal()
    {
        if (currentTxt == clickToHealTxt) return;
        transform.GetChild(0).gameObject.SetActive(true);
        if (currentTxt != null)
        {
            var lastTxt = currentTxt;
            lastTxt.DOFade(0, 0.5f);
        }
        currentTxt = clickToHealTxt;
        currentTxt.DOFade(1, 0.5f);
        handAnimController.gameObject.SetActive(true);
        StartCoroutine(HandHeal());
    }
    public void HoldToAim()
    {
        if (currentTxt == holdToAimTxt) return;
        transform.GetChild(0).gameObject.SetActive(true);
        if (currentTxt != null)
        {
            var lastTxt = currentTxt;
            lastTxt.DOFade(0, 0.5f);
        }

        currentTxt = holdToAimTxt;
        currentTxt.DOFade(1, 0.5f);
        handAnimController.gameObject.SetActive(true);
        StartCoroutine(HandShoot());
    }
    public void ReleaseToShoot()
    {
        if (currentTxt == releaseToShootTxt) return;
        transform.GetChild(0).gameObject.SetActive(true);
        if (currentTxt != null)
        {
            var lastTxt = currentTxt;
            lastTxt.DOFade(0, 0.5f);
        }

        currentTxt = releaseToShootTxt;
        currentTxt.DOFade(1, 0.5f);
        handAnimController.gameObject.SetActive(false);
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
    IEnumerator HandHeal()
    {
        handAnimController.SetBool("Swipe", false);
        handAnimController.SetBool("Shoot", false);
        handRectTransform.DOKill();
        handRectTransform.DOMove(targetHealRectTransform.position, 0.5f).SetEase(Ease.Linear);
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
        switch (PlayerController.Instance.currentweaponType)
        {
            case WeaponType.SMG:
            case WeaponType.ShotGun:
                SMGTutorialController();
                break;
            case WeaponType.Sniper:
                SniperTuturialController();
                break;
            case WeaponType.Machinegun:
                MachineGunTuturialController();
                break;

        }
       
       

    }
    public void SMGTutorialController()
    {
        if (IsTutorialGrenadePhase())
        {
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
            return;
        }
        HandlePlayerSMGState(PlayerController.Instance.currentState);
    }
    private bool IsTutorialGrenadePhase()
    {
        return levelControl.currentWave == 0 && levelControl.currentEnemyWave == 2;
    }
    private void HandlePlayerSMGState(PlayerState state)
    {
        switch (state)
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
    public void SniperTuturialController()
    {
        PlayerUIControl.Instance.goShootBtn.SetActive(true);
        HandlePlayerSniperState(PlayerController.Instance.currentState);
    }
    private void HandlePlayerSniperState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Move:
                transform.GetChild(0).gameObject.SetActive(false);
                break;
            case PlayerState.Cover:
                HoldToAim();


                break;
            case PlayerState.Shoot:
                if (CameraManager.Instance.IsAimTarget(LayerConfig.Instance.shootMask))
                {
                    ReleaseToShoot();
                }
                else
                {
                    AimAtTarget();
                }

                break;

        }
    }
    public void MachineGunTuturialController()
    {
        if (PlayerController.Instance.playerHealth.health == 50)
        {
            ClickToHeal();
            PlayerUIControl.Instance.goShootBtn.SetActive(false);
            return;
        }
        PlayerUIControl.Instance.goShootBtn.SetActive(true);
        HandlePlayerMachineGunState(PlayerController.Instance.currentState);
    }
    private void HandlePlayerMachineGunState(PlayerState state)
    {
        switch (state)
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
