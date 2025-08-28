using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


public class BaseEnemyController : EnemyBase
{
    public MoveType moveType;
    public NavMeshAgent nav;
    public EnemyPoint patrolPoint;
    public EnemyLK enemyLK;
    public CharactorAnimatorController animatorController;
    public UnityEvent OnDead;
    public EnemyBaseShootController shootController;
    public int numberCover;
  
    public override void Init(int id, MoveType height, bool canChangePatrolPoint, int maxHP,WeaponType weaponType, int damage, float fireRate)
    {
        base.Init(id,height,canChangePatrolPoint,maxHP, weaponType,damage, fireRate);
        moveType = height;
        shootController.Init(weaponType,damage, fireRate);
        currentState = EnemyState.Move;
    }
    private void Awake()
    {
        currentState = EnemyState.None;
    }
    void Update()
    {
        if (PlayerController.Instance.currentState == PlayerState.Win)
            return;
        if (PlayerController.Instance.currentState == PlayerState.Dead)
            return;
        if (currentState == EnemyState.Dead)
            return;

        switch (currentState)
        {
            case EnemyState.Move:
                MoveState();

                break;
            case EnemyState.Reload:

                ReloadState();

                break;
            case EnemyState.Cover:

                CoverState();

                break;

            case EnemyState.Shoot:

                ShootState();

                break;
            case EnemyState.Dead:

                break;
        }
        
    }

    public override void StartWave(EnemyPoint targetPoint)
    {
        nav.enabled = true;
        targetPoint.isTargeted = true;
        this.patrolPoint = targetPoint;
        StartMove();
        nav.SetDestination(targetPoint.transform.position);
    }
    #region Move
    public void StartMove()
    {
        nav.speed = moveType == MoveType.Walk ? moveSpeed / 2 : moveSpeed;
        animatorController.Move(moveType == MoveType.Walk ? 0.5f : 1);
        //animatorController.SetBool(GameConstain.ALARM, true);
        currentState = EnemyState.Move;
    }
    private void MoveState()
    {
        if (Vector3.Distance(transform.position, patrolPoint.transform.position) <= 0.5f)
        {
            MoveToCover();
        }
    }
    public void MoveToCover()
    {
        nav.enabled = false;
        animatorController.Cover((float)patrolPoint.coverType, (float)patrolPoint.coverDirection);
        animatorController.StopMove(false);
        transform.DOMove(patrolPoint.transform.position, 0.5f).SetEase(Ease.OutSine);
        transform.DORotate(patrolPoint.transform.rotation.eulerAngles, 0.5f).SetEase(Ease.OutSine);
        currentState = EnemyState.Cover;
       
    }
    #endregion

    #region Cover
    public void CoverState()
    {
       
        coverTime += Time.deltaTime;
        if (coverTime >= 2&&PlayerController.Instance.currentState!=PlayerState.Move)
        {
            CoverToShoot();
            coverTime = 0;
        }
    }
    public void CoverToShoot()
    {
        if (patrolPoint.coverType != CoverType.None)
        {
            animatorController.Aim(true, (float)patrolPoint.coverType, (float)patrolPoint.coverDirection);
        }
        StartCoroutine(StartDelayCanShoot());
        currentState = EnemyState.Shoot;
        Vector3 direction = PlayerController.Instance.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        Quaternion targetRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y /*+ shootRotationOffsetY*/, 0);
        transform.DORotate(targetRotation.eulerAngles, 0.25f);
    }
    #endregion
    #region Shoot
    IEnumerator StartDelayCanShoot()
    {
        isCanShoot = false;
        yield return new WaitForSeconds(shootController.startDelay);
        isCanShoot = true;
    }
    private void ShootState()
    {
        timerToShoot += Time.deltaTime;
        if (timerToShoot >= 3)
        {
            timerToShoot = 0.0f;
            Reload();
        }
        if (isCanShoot)
        {
            animatorController.Recoil((float)shootController.weaponControl.weaponType);
            shootController.Shoot();
            StartCoroutine(DelayCanShoot());
        }
    }
    IEnumerator DelayCanShoot()
    {
        isCanShoot = false;
        yield return new WaitForSeconds(1f/shootController.fireRate);
        isCanShoot = true;
    }
    #endregion

    private void PatrolOrCover()
    {
        if (!canChangePatrolPoint) return;
        numberCover++;
        if (numberCover >= 2)
        {
            EnemyPoint newPoint = patrolPoint.GetNextPoint();
            if (newPoint != null && !newPoint.isTargeted)
            {
                StartCoroutine(DelayChangePatrolPoint(newPoint));
            }
            else
            {
                currentState = EnemyState.Reload;
            }
        }
    }
    public void Relax()
    {

    }
    IEnumerator DelayChangePatrolPoint(EnemyPoint newPoint)
    {
        yield return new WaitForSeconds(0.5f);
        numberCover = 0;
        patrolPoint.isTargeted = false;
        StartWave(newPoint);

    }

   
    #region Reload
    public void Reload()
    {
        timerToReload = 0.0f;
        animatorController.Reload(0);
        animatorController.Aim(false, (float)patrolPoint.coverType, (float)patrolPoint.coverDirection);
        transform.DORotate(patrolPoint.transform.eulerAngles, 0.25f);
        currentState = EnemyState.Reload;
        PatrolOrCover();
    }
    public void ReloadState()
    {
        timerToReload += Time.deltaTime;
        if (timerToReload >= 2)
        {
            timerToReload = 0.0f;
            currentState = EnemyState.Cover;
          
        }
    }
    #endregion
    public override void GetHit(float hitType)
    {
        if (animatorController.IsInState("GetHit", 5)) return;
        animatorController.GetHit(hitType);
        int i = Random.Range(0, sfxHitBodys.Length);
        audioSource.PlayOneShot(sfxHitBodys[i]);
    }
    public override void Dead(Vector3 direction)
    {
        OnDead?.Invoke();
        if (patrolPoint != null) patrolPoint.isTargeted = false;
        currentState = EnemyState.Dead;
        nav.enabled = false;
        transform.DOKill();
        animatorController.OnDead(direction);
        GameManager.Instance.levelControl.RemoveEnemy(this);
        GameManager.Instance.levelControl.ChangePlayerCoverPoint();
        AudioController.Instance.PlaySfx(GameConstain.ENEMY_DEAD + Random.Range(1, 4).ToString());
        if (GameManager.Instance.levelControl.GetEnemyCount() == 0)
        {
            TimeController.Instance.DoSlowMotion(0.5f, 0.2f, 0.5f);
            if (GameManager.Instance.levelControl.IsLastWave())
            {
               
                CameraManager.Instance.SetBlend(CinemachineBlendDefinition.Style.EaseIn, 0.25f);
                if (patrolPoint.killCamPoint != null)
                {
                    CameraManager.Instance.ChangeCamera(CameraType.KillEnemy, patrolPoint.killCamPoint);
                }
                else
                {
                    CameraManager.Instance.ChangeCamera(CameraType.KillEnemy, transform);
                }
                CameraManager.Instance.currentVirtualCamnera.SetLookAt(animatorController.bodyPartControls[0].transform);
                if (PlayerController.Instance.currentweaponType == WeaponType.Sniper)
                {
                    PlayerController.Instance.playerShootController.currentWeaponControl.SetActiveScope(false);
                    CameraManager.Instance.mainCamera.enabled = true;
                }
               
            }
            GameManager.Instance.levelControl.ActiveNextEnemiesWave();
           
        }
        MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnKillEnermy, new object[] { currentType,PlayerController.Instance.currentweaponType }));

    }

  
}
