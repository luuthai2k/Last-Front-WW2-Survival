using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyTankController : EnemyBase
{
    public MainGunControl mainGunControl;
    public EnemyPoint patrolPoint;
    public NavMeshAgent nav;
    public UnityEvent OnDead;
    public Material material;
    private float offsetY=0;
    public Rigidbody[] destructionRbs;
    public override void Init(int id, MoveType height, bool canChangePatrolPoint, int maxHP, WeaponType weaponType, int damage, float fireRate)
    {
        base.Init(id, height, canChangePatrolPoint, maxHP,weaponType, damage, fireRate);
        //moveHeight = height;
        //shootController.Init(damage, fireRate);
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
        currentState = EnemyState.Move;
    }
    private void MoveState()
    {
        offsetY += Time.deltaTime * moveSpeed;
            material.SetTextureOffset("_MainTex", new Vector2(0f, offsetY));
        if (Vector3.Distance(transform.position, patrolPoint.transform.position) <= 0.5f)
        {
            MoveToCover();
        }
    }
    public void MoveToCover()
    {
        nav.enabled = false;
        transform.DOMove(patrolPoint.transform.position, 0.5f).SetEase(Ease.OutSine);
        currentState = EnemyState.Reload;
        RotationToTarget();
    }
    public void RotationToTarget()
    {
        Vector3 direction = PlayerController.Instance.GetHeadTargetTrans().position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        Quaternion targetRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
        float currentY = mainGunControl.transform.rotation.eulerAngles.y;
        float targetY = targetRotation.eulerAngles.y;
        float angleDiff = Mathf.DeltaAngle(currentY, targetY);
        float duration = Mathf.Abs(angleDiff) / rotationSpeed;
        mainGunControl.transform.DORotate(targetRotation.eulerAngles, duration);
        mainGunControl.LookAtTarget();
    }
    #endregion

    #region Shoot
    IEnumerator StartDelayCanShoot()
    {
        isCanShoot = false;
        yield return new WaitForSeconds(2);
        isCanShoot = true;
    }
    private void ShootState()
    {
        timerToShoot += Time.deltaTime;
        if (timerToShoot >= 5)
        {
            timerToShoot = 0.0f;
            Reload();
        }
        if (isCanShoot)
        {
            Shoot();
        }
    }


    private void Shoot()
    {
        Vector3 playerFixedPos = PlayerController.Instance.GetHeadTargetTrans().position;
        mainGunControl.Shooting(playerFixedPos);
        StartCoroutine(DelayCanShoot());
    }
    IEnumerator DelayCanShoot()
    {
        isCanShoot = false;
        yield return new WaitForSeconds(1f / mainGunControl.fireRate);
        isCanShoot = true;
    }
    #endregion


    #region Reload
    public void Reload()
    {
        timerToReload = 0.0f;
        currentState = EnemyState.Reload;
    }
    public void ReloadState()
    {
        timerToReload += Time.deltaTime;
        if (timerToReload >= 2)
        {
            timerToReload = 0.0f;
            ReloadToShoot();

        }
    }
    public void ReloadToShoot()
    {
        StartCoroutine(StartDelayCanShoot());
        currentState = EnemyState.Shoot;
       
    }
    #endregion
    public override void Dead(Vector3 direction)
    {
        OnDead?.Invoke();
        ResourceHelper.Instance.GetEffect(EffectType.BigExplosion, transform.position, Quaternion.identity);
        if (patrolPoint != null) patrolPoint.isTargeted = false;
        for(int i=0;i< destructionRbs.Length; i++)
        {
            destructionRbs[i].gameObject.SetActive(true);
            destructionRbs[i].transform.parent = null;
            destructionRbs[i].AddForce(direction);
        }
        currentState = EnemyState.Dead;
        nav.enabled = false;
        GameManager.Instance.levelControl.RemoveEnemy(this);
        GameManager.Instance.levelControl.ChangePlayerCoverPoint();
        AudioController.Instance.PlaySfx(GameConstain.ENEMY_DEAD + Random.Range(1, 4).ToString());
        if (GameManager.Instance.levelControl.GetEnemyCount() == 0)
        {
            TimeController.Instance.DoSlowMotion(0.2f, 1f, 0.5f);
            if (GameManager.Instance.levelControl.IsLastWave())
            {

                //CameraManager.Instance.SetBlend(CinemachineBlendDefinition.Style.EaseIn, 0.25f);
                //if (patrolPoint.killCamPoint != null)
                //{
                //    CameraManager.Instance.ChangeCamera(CameraType.KillEnemy, patrolPoint.killCamPoint);
                //}
                //else
                //{
                //    CameraManager.Instance.ChangeCamera(CameraType.KillEnemy, transform);
                //}
                CameraManager.Instance.currentVirtualCamnera.SetLookAt(mainGunControl.transform);

            }
            GameManager.Instance.levelControl.ActiveNextEnemiesWave();

        }
        MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnDestroyVehicles));
        gameObject.SetActive(false);


    }
}
