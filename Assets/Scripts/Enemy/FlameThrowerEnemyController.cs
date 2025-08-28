using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
public class FlameThrowerEnemyController : EnemyBase
{
    public NavMeshAgent nav;
    public EnemyPoint patrolPoint;
    public float shootRotationOffsetY;
    public UnityEvent OnDead;
    public CharactorAnimatorController animatorController;
    public EnemyFlameThrowerShootController flameThrowerShootController;
    public int numberCover;
    public override void Init(int id, MoveType height, bool canChangePatrolPoint, int maxHP, WeaponType weaponType, int damage, float fireRate)
    {
        base.Init(id, height, canChangePatrolPoint, maxHP,weaponType, damage, fireRate);
        flameThrowerShootController.Init(damage,fireRate);
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
        nav.speed = moveSpeed;
        animatorController.Move(1);
        animatorController.SetBool(GameConstain.ALARM, true);
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
        animatorController.Cover();
        Vector3 target = patrolPoint.transform.position;
        target.y = transform.position.y;
        transform.DOMove(target, 0.5f).SetEase(Ease.OutSine);
        transform.DORotate(patrolPoint.transform.rotation.eulerAngles, 0.5f).SetEase(Ease.OutSine);
        currentState = EnemyState.Cover;
    }
    #endregion

    #region Cover
    public void CoverState()
    {

        coverTime += Time.deltaTime;
        if (coverTime >= 2.5f && PlayerController.Instance.currentState != PlayerState.Move)
        {
            CoverToShoot();
            coverTime = 0;
        }
    }
    public void CoverToShoot()
    {
        currentState = EnemyState.Shoot;
        flameThrowerShootController.StartShoot();
    }
    #endregion
    #region Shoot
    private void ShootState()
    {
        timerToShoot += Time.deltaTime;
        if (timerToShoot >= 4f)
        {
            timerToShoot = 0.0f;
            PatrolOrCover();
            StartCoroutine(DelayFinishShoot());
        }
    }
    IEnumerator DelayFinishShoot()
    {
        yield return new WaitForSeconds(0.5f);
        flameThrowerShootController.FinishShoot();
    }
    #endregion
    private void PatrolOrCover()
    {
        if (!canChangePatrolPoint)
        {
            currentState = EnemyState.Cover;
            return;
        }
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
                currentState = EnemyState.Cover;
            }
        }
    }
    IEnumerator DelayChangePatrolPoint(EnemyPoint newPoint)
    {
        yield return new WaitForSeconds(0.5f);
        numberCover = 0;
        patrolPoint.isTargeted = false;
        StartWave(newPoint);

    }
    public override void GetHit(float hitType)
    {
        if (animatorController.IsInState("GetHit", 5)) return;
        animatorController.GetHit(hitType);
        int i = Random.Range(0, sfxHitBodys.Length);
        audioSource.PlayOneShot(sfxHitBodys[i]);
    }
    public override void Dead( Vector3 direction)
    {
        OnDead?.Invoke();
        flameThrowerShootController.FinishShoot();
        if (patrolPoint != null) patrolPoint.isTargeted = false;
        currentState = EnemyState.Dead;
        nav.enabled = false;
        animatorController.OnDead(direction);
        GameManager.Instance.levelControl.RemoveEnemy(this);
        GameManager.Instance.levelControl.ChangePlayerCoverPoint();
        AudioController.Instance.PlaySfx(GameConstain.ENEMY_DEAD + Random.Range(1, 4).ToString());
        if (GameManager.Instance.levelControl.GetEnemyCount() == 0)
        {
            TimeController.Instance.DoSlowMotion(0.5f, 0.2f, 0.5f);
            if (GameManager.Instance.levelControl.IsLastWave())
            {

                CameraManager.Instance.SetBlend(CinemachineBlendDefinition.Style.EaseIn, 0.35f);
                if (patrolPoint.killCamPoint != null)
                {
                    CameraManager.Instance.ChangeCamera(CameraType.KillEnemy, patrolPoint.killCamPoint);
                }
                else
                {
                    CameraManager.Instance.ChangeCamera(CameraType.KillEnemy, transform);
                }
                CameraManager.Instance.currentVirtualCamnera.SetLookAt(animatorController.bodyPartControls[0].transform);

            }
            GameManager.Instance.levelControl.ActiveNextEnemiesWave();

        }
        MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnKillEnermy, new object[] { currentType, PlayerController.Instance.currentweaponType }));

    }
    public void FlameThrowerGunDestruction()
    {
        if (enemyHealth.isDead) return;
        enemyHealth.health = 0;
        enemyHealth.isDead = true;
        enemyHealth.healthBar.UpdateHealth(300, 0, false);
        Dead( Vector3.zero);

    }

}
