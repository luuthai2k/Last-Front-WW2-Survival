using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : Singleton<PlayerController>
{
   
    public GameObject playerModel;
    public PlayerState currentState;
    public WeaponType currentweaponType;
    public SoldierInGameData data;
    public CharactorAnimatorController animatorController;
    public ModelBoneRefrences boneRefrences;
    public PlayerShootController playerShootController;
    private PlayerSwapHandle swapHandle;
    private PlayerGrenadeThrowHandle grenadeThrowHandle;
    public PlayerHealth playerHealth;
    public NavMeshAgent nav;
    public TargetCover targetCover;
	public List<Transform> patrolPath = new List<Transform>();
	private PlayerPoint coverPoint;
	private int currentPathIndex;
	public List<bool> reachNode = new List<bool>();
    public int currentWeaponID;
    public float moveSpeed=5,rotationSlerp;
    public float rotationYOffset;
    public float maxHorizontalAngle = -10;
    public float minHorizontalAngle = 110;
    public bool isCanShoot,canOutToShoot,isOutShoot;
    Coroutine delayCanShoot,delayShootToCover;
    public bool isAimGrenade;
    Tween navSpeedTween;

    private void Update()
    {
        switch (currentState)
        {
            case PlayerState.Ready:
                break;
            case PlayerState.Move:
                MoveState();
                break;
            case PlayerState.Cover:
                CoverState();
                break;
            case PlayerState.Grenade:
                GrenadeState();
                break;
            case PlayerState.Shoot:
                ShootState();
                break;
            case PlayerState.Swap:
                break;
            case PlayerState.Reload:
                ReloadState();
                break;
            case PlayerState.Heal:
                if (!PlayerUIControl.Instance.isHeal)
                {
                    playerHealth.Heal();
                    currentState = PlayerState.Cover;
                    if (currentweaponType == WeaponType.Machinegun)
                    {
                        PlayerUIControl.Instance.MachineState();
                    }
                    else
                    {
                        PlayerUIControl.Instance.DisplayState();
                    }
                }
                break;
        }
    }
    public void CoverState()
    {
        if (PlayerUIControl.Instance.isReloading)
        {
            CoverToReload();
        }
        if (PlayerUIControl.Instance.isAim)
        {
            if (playerShootController.IsCanShoot())
            {
                CoverToShoot();
            }
            else
            {
                PlayerUIControl.Instance.OnClickReloadBtn();
            }
        }
        if (PlayerUIControl.Instance.isHeal)
        {
            currentState = PlayerState.Heal;
            PlayerUIControl.Instance.HideState();
        }
      
    }
    public void ShootState()
    {
        RotationWhenShoot();
        switch (currentweaponType)
        {
            case WeaponType.SMG:
            case WeaponType.ShotGun:
            case WeaponType.Machinegun:
                if (!PlayerUIControl.Instance.isAim && canOutToShoot)
                {
                    if (isOutShoot) return;
                    isOutShoot = true;
                    ShootToCover();
                }
               
                if (isCanShoot)
                {
                    playerShootController.ShootDelay(currentweaponType);
                }
                if (PlayerUIControl.Instance.isReloading)
                {
                    if (TimeController.Instance.isSlowMotion)
                    {
                        TimeController.Instance.DoKillSlowMotion();
                    }
                    ShootToReload();
                }
                break;
            case WeaponType.Sniper:
            case WeaponType.Rocket:
                if (!PlayerUIControl.Instance.isAim&& canOutToShoot)
                {
                    if (isOutShoot) return;
                    isOutShoot = true;
                    if (isCanShoot)
                    {
                        playerShootController.ShootDelay(currentweaponType);
                        ShootToCover(0.5f);
                        return;
                    }
                    ShootToCover();


                }
              
                break;
        }
    }
    public void ReloadState()
    {
        if (!PlayerUIControl.Instance.isReloading)
        {
            OnEndReload();
        }
    }
    public void GrenadeState()
    {
    }
    public void OnEndReload()
    {
        if (currentState == PlayerState.Reload)
        {
            currentState = PlayerState.Cover;
        }
    }
    public void OnSwap()
    {
        if (currentState == PlayerState.Grenade)
        {
            animatorController.CancerGrenade();
            currentState = PlayerState.Swap;
            StartCoroutine(DelayEquipWeapon(0.6f));
        }
        if (currentState == PlayerState.Cover)
        {
            animatorController.Holster();
            currentState = PlayerState.Swap;
            StartCoroutine(DelaySwapWeapon(0.5f));
        }
    }
    IEnumerator DelaySwapWeapon(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        swapHandle.SwapWeapon();
        animatorController.Equip();
    }
    IEnumerator DelayEquipWeapon(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        animatorController.Equip();
    }
    public void OnEndSwap()
    {
        if (currentState == PlayerState.Move) return;
        currentState = PlayerState.Cover;
        currentweaponType = playerShootController.currentWeaponControl.type;
        if (currentweaponType == WeaponType.ShotGun)
        {
            CrosshairControl.Instance.ChangeCrosshair(1);
        }
        else
        {
            CrosshairControl.Instance.ChangeCrosshair(0);
        }

    }
    public void OnGrenade()
    {
        Transform target = GameManager.Instance.levelControl.FindClosestEnemy();
        if (target == null) return;
        if (currentState != PlayerState.Cover) return;
        animatorController.Holster();
        currentState = PlayerState.Swap;
        StartCoroutine(DelayChangeToGrenadeState());
        IEnumerator DelayChangeToGrenadeState()
        {
            yield return new WaitForSeconds(0.7f);
            if (grenadeThrowHandle == null)
            {
                grenadeThrowHandle = playerModel.GetComponent<PlayerGrenadeThrowHandle>();
            }
            grenadeThrowHandle.SetTargetPoint(target.position + Vector3.up);
            currentState = PlayerState.Grenade;
            animatorController.ThrowGrenade((float)coverPoint.coverDirection);
            Quaternion targetRotation =Quaternion.LookRotation( target.position-transform.position);
            playerModel.transform.DOLocalRotate(new Vector3(0, targetRotation.eulerAngles.x - 45, 0), 0.75f);
        }
    }
    public void OnEndGrenade()
    {
        GrenadeToCover();
    }
   
    #region SetUp
    public void Init(Transform spawnPoint,WeaponType mainType,float safeTime)
    {
        data = DataController.Instance.GetCurrentSoldierIngameData();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        var wpid = DataController.Instance.GetCurrentWeaponInGameData(mainType);
        currentweaponType = mainType;
        switch (currentweaponType)
        {
            case WeaponType.Sniper:
                CameraManager.Instance.ChangeCamera(CameraType.Sniper);
                break;
            case WeaponType.Machinegun:
                 wpid = DataController.Instance.GetCurrentWeaponInGameData(WeaponType.SMG);
                break;
            default:
                CameraManager.Instance.ChangeCamera(CameraType.Base);
                break;
        }
        playerHealth.InitHP(data.soldierStat.health,safeTime);
        playerShootController.handStability = data.soldierStat.handStability;
        LoadSoldierFromResources();
        playerShootController.SpawmMainWeapon(wpid.ID);
        SetBoneLK();
        if (currentweaponType == WeaponType.SMG&&DataController.Instance.Level>1)
        {
            var sgwpid = DataController.Instance.GetCurrentWeaponInGameData(WeaponType.ShotGun);
            swapHandle= playerModel.GetComponent<PlayerSwapHandle>();
            swapHandle.SpawmWeapon(sgwpid);
            PlayerUIControl.Instance.SetUpWeaponUIEquipments(new List<WeaponInGameData> { wpid, sgwpid });
        }
        else
        {
            PlayerUIControl.Instance.SetUpWeaponUIEquipments(new List<WeaponInGameData> { wpid});
        }
        CameraManager.Instance.currentVirtualCamnera.SetDamping(new Vector3(0.1f, 0.5f, 1.5f), 0.1f);
        SceneController.Instance.allowLoadingDeactivation = true;

    }
    void LoadSoldierFromResources()
    {

        playerModel = Instantiate(Resources.Load<GameObject>($"Soldier/{data.ID}"), transform);
        playerModel.transform.localPosition = Vector3.zero;
        playerModel.transform.localRotation = Quaternion.identity;
      

    }
    public void SetBoneLK()
    {
        PlayerIK playerIK = playerModel.GetComponent<PlayerIK>();
        animatorController = playerModel.GetComponent<CharactorAnimatorController>();
        boneRefrences.playerIK = playerIK;
        playerIK.SetLK(playerShootController.mainWeaponControl.leftHandIKTarget);
    }
    public void StartWave(TargetCover targetCover)
    {
        if (targetCover.targetPoint == null || this.targetCover.targetPoint == targetCover.targetPoint)
        {
            Debug.LogWarning("ReTurm");
            return;
        }
        if (coverPoint != null && coverPoint.supCamPoint != null)
        {
            CameraManager.Instance.ChangeCamera(CameraType.Base);
        }
        this.targetCover = targetCover;
        coverPoint = targetCover.targetPoint;
        patrolPath.Clear();
        reachNode.Clear();
        currentPathIndex = 0;
        if (!nav.enabled) nav.enabled = true;
        if (targetCover.paths.Length > 0)
        {
            for (int i = 0; i < targetCover.paths.Length; i++)
            {
                patrolPath.Add(targetCover.paths[i]);
                reachNode.Add(false);
            }
            nav.SetDestination(patrolPath[0].position);
        }
        else
        {
            nav.SetDestination(coverPoint.transform.position);
        }
        //GamePlayUIManager.Instance.gamePlayMenu.SetAlarm(false);
        if (currentState == PlayerState.Shoot)
        {
            ShootToMove();
            return;
        }
        else if (currentState == PlayerState.Cover||currentState==PlayerState.Ready)
        {
            StartMove();
            return;

        }

        StartCoroutine(WaitCoverToMove());

    }
    IEnumerator WaitCoverToMove()
    {
        while(true)
        {
            if(currentState == PlayerState.Cover|| currentState == PlayerState.Reload)
            {
                StartMove();
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
      
    }
    public void ChangeToStateMachinegun(MachinegunControl machinegunControl)
    {
        if (currentweaponType == WeaponType.Machinegun) return;
        currentweaponType = WeaponType.Machinegun;
        playerShootController.SetWeaponControl(machinegunControl);
        playerShootController.ReloadAmmo();
        CameraManager.Instance.ChangeCamera(CameraType.Machinegun,machinegunControl.gunAssemblyTrans);
        var wpid = DataController.Instance.GetCurrentWeaponInGameData(WeaponType.Machinegun);
        PlayerUIControl.Instance.SetUpWeaponUIEquipments(new List<WeaponInGameData> { wpid });
    }
    public void ChangeToStateRocket(RocketControl rocketControl)
    {
        if (currentweaponType == WeaponType.Rocket) return;
        currentweaponType = WeaponType.Rocket;
        CameraManager.Instance.ChangeCamera(CameraType.Rocket);
        CameraManager.Instance.CoverState(coverPoint.coverType, coverPoint.coverDirection);
        PlayerUIControl.Instance.SetUpWeaponUIEquipments(null);
        StartCoroutine(DelayPickUpRocket());
        IEnumerator DelayPickUpRocket()
        {
            while (animatorController.IsInState("Cover",0))
            {
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(1f);
            animatorController.Holster();
            currentState = PlayerState.Swap;
            yield return new WaitForSeconds(1f);
            playerShootController.SetWeaponControl(rocketControl);
            PickUpRocket();
            playerShootController.ReloadAmmo();
            currentState = PlayerState.Cover;


        }
    }
   public void PickUpRocket()
    {
        playerShootController.currentWeaponControl.transform.parent = playerShootController.weaponContainerTrans;
        playerShootController.currentWeaponControl.transform.localPosition = Vector3.zero;
        playerShootController.currentWeaponControl.transform.localRotation = Quaternion.identity;
        boneRefrences.playerIK.SetLK(playerShootController.currentWeaponControl.leftHandIKTarget);
    }
    public void TryWeapon(string weaponId)
    {

        playerShootController.ChangeMainWeapon(weaponId);

    }


    #endregion
    #region Move
    public void StartMove()
    {
        float speedValue = targetCover.moveType == MoveType.Walk ?  0.5f : 1;
        navSpeedTween= DOVirtual.Float(0, speedValue, 1, (x) =>
        {
            nav.speed = x * moveSpeed;
        });
        nav.enabled = true;
        animatorController.Move(speedValue);
        CameraManager.Instance.currentVirtualCamnera.SetCamSideUseTween(0.5f, 0.5f);
        currentState = PlayerState.Move;
        PlayerUIControl.Instance.HideState();
    }
    public void ShootToMove()
    {
        Debug.LogError("ShootToMove");
        isOutShoot = true;
        StartCoroutine(DelayShootToMove());
        IEnumerator DelayShootToMove()
        {
            isCanShoot = false;
            if (delayCanShoot != null)
            {
                StopCoroutine(delayCanShoot);
            }
            playerHealth.CanTakeDame(false);
            if (currentweaponType == WeaponType.Sniper)
            {
                StartCoroutine(WaitCoverToMove());
                yield break;

            }

            Debug.LogError("Move");
            currentState = PlayerState.Move;
            float speedValue = targetCover.moveType == MoveType.Walk ? 0.5f : 1;
            navSpeedTween= DOVirtual.Float(0, speedValue, 1, (x) =>
            {
                nav.speed = x * moveSpeed;
            });
            nav.enabled = true;
            animatorController.Move(speedValue);
            animatorController.Aim(false);
            playerHealth.CanTakeDame(false);
            CameraManager.Instance.MoveState();
            PlayerUIControl.Instance.HideState();
            if (TimeController.Instance.isSlowMotion)
            {
                TimeController.Instance.DoKillSlowMotion();
            }
            isOutShoot = false;
        }
    }
    public void MoveState()
    {
      playerModel.transform.localRotation = Quaternion.Slerp(playerModel.transform.localRotation, Quaternion.identity, rotationSlerp * Time.deltaTime);
        if (currentPathIndex>= patrolPath.Count)

        {
            float distanceTarget = Vector3.Distance(transform.position, coverPoint.transform.position);
            if (distanceTarget <= 0.5f)
            {
                MoveToCover();
              
                if (coverPoint.supCamPoint != null)
                {
                    CameraManager.Instance.ChangeCamera(CameraType.Support, coverPoint.supCamPoint.transform);
                    CameraManager.Instance.currentVirtualCamnera.SetCamDistanceUseTween(coverPoint.supCamPoint.distance, 0.5f);
                    CameraManager.Instance.currentVirtualCamnera.SetVerticalArmLengthUseTween(coverPoint.supCamPoint.verticalArmLenght, 0.5f);
                }
                else
                {
                  
                }
            }
        }
        else
        {
          
            if (!reachNode[currentPathIndex] && Vector3.Distance(transform.position, patrolPath[currentPathIndex].position) <= 0.5f)
            {
                reachNode[currentPathIndex] = true;
                currentPathIndex++;
                if (currentPathIndex < patrolPath.Count)
                {
                    nav.SetDestination(patrolPath[currentPathIndex].position);
                }
                else
                {
                    nav.SetDestination(coverPoint.transform.position);
                }
            }

        }
    }
    public void MoveToCover()
    {
        if(navSpeedTween!=null) navSpeedTween.Kill();
        nav.enabled = false;
        nav.speed = 0;
        animatorController.StopMove();
        if (currentweaponType != WeaponType.Machinegun)
        {
            animatorController.Cover((float)coverPoint.coverType, (float)coverPoint.coverDirection);
            CameraManager.Instance.CoverState(coverPoint.coverType, coverPoint.coverDirection);
        }
        currentState = PlayerState.Cover;
        float angle = Quaternion.Angle(transform.rotation, coverPoint.transform.rotation);
        float duration = angle / rotationSlerp;
        transform.DORotate(coverPoint.transform.rotation.eulerAngles, duration).SetEase(Ease.OutSine);
        transform.DOMove(coverPoint.transform.position, duration).SetEase(Ease.OutSine);
        StartCoroutine(DelayActiveUI());
        IEnumerator DelayActiveUI()
        {
            yield return new WaitForSeconds(0.75f);
            if (currentweaponType == WeaponType.Rocket)
            {
                PlayerUIControl.Instance.RoketState();
            }
            else if (currentweaponType == WeaponType.Machinegun)
            {
                PlayerUIControl.Instance.MachineState();
                playerHealth.DelayCanTakeDame();
            }
            else
            {
                PlayerUIControl.Instance.DisplayState();
            }
                
        }
    }
    #endregion

    #region Shoot
    public void RotationWhenShoot()
    {
        Quaternion targetRotation = CameraManager.Instance.GetTargetRotation();
        playerModel.transform.localRotation = Quaternion.Slerp(playerModel.transform.localRotation, Quaternion.Euler(0, targetRotation.eulerAngles.y+ rotationYOffset, 0), rotationSlerp * Time.deltaTime);
    }
    public void ShootToCover(float timeDelay=0)
    {
        isCanShoot = false;
        if (delayCanShoot != null)
        {
            StopCoroutine(delayCanShoot);
        }
        delayShootToCover= StartCoroutine(DelayShootToCover(timeDelay));
    }

    IEnumerator DelayShootToCover(float timeDelay)
    {
     
        yield return new WaitForSeconds(timeDelay);
        if (PlayerUIControl.Instance.isAim && playerShootController.IsCanShoot())
        {
            isOutShoot = false;
            isCanShoot = true;
            yield break;
        }
        animatorController.Aim(false,(float)coverPoint.coverType, (float)coverPoint.coverDirection);
        playerShootController.currentWeaponControl.SetActiveScope(false);
        boneRefrences.playerIK.UpperChestIKWeight(0, 0.5f);
        if (currentweaponType != WeaponType.Machinegun)
        {
            playerHealth.CanTakeDame(false);
            PlayerUIControl.Instance.DisplayState();
            CameraManager.Instance.CoverState(coverPoint.coverType, coverPoint.coverDirection);
            if (TimeController.Instance.isSlowMotion)
            {
                TimeController.Instance.DoKillSlowMotion();
            }
            if (playerModel.transform.localRotation.eulerAngles.y > maxHorizontalAngle)
            {
                playerModel.transform.DOLocalRotate(new Vector3(0, maxHorizontalAngle, 0), 0.25f);
            }
            else if (playerModel.transform.localRotation.eulerAngles.y < minHorizontalAngle)
            {
                playerModel.transform.DOLocalRotate(new Vector3(0, minHorizontalAngle, 0), 0.25f);
            }
            while (true)
            {
                if (animatorController.IsInState("Cover", 0))
                {
                    isOutShoot = false;
                    currentState = PlayerState.Cover;
                    if(currentweaponType == WeaponType.ShotGun)
                    {
                        CrosshairControl.Instance.ChangeCrosshair(1);
                    }
                    else
                    {
                        CrosshairControl.Instance.ChangeCrosshair(0);
                    }
                    yield break;
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            PlayerUIControl.Instance.MachineState();
            CameraManager.Instance.currentVirtualCamnera.SetVerticalFOVUseTween(60, 0.35f);
            yield return new WaitForSeconds(0.35f);
            isOutShoot = false;
            currentState = PlayerState.Cover;
            CrosshairControl.Instance.ChangeCrosshair(0);
          

        }
      
       

    }
    #endregion

    #region Cover
    public void CoverToShoot()
    {
        animatorController.Aim(true, (float)currentweaponType);
        canOutToShoot = false;
        if (currentweaponType == WeaponType.Machinegun)
        {
            CrosshairControl.Instance.ChangeCrosshair(4);
            CameraManager.Instance.currentVirtualCamnera.SetVerticalFOVUseTween(40, 0.25f);
            StartCoroutine(DelayCanOutToShoot(0.1f));
        }
        else if (currentweaponType == WeaponType.Sniper)
        {
            CrosshairControl.Instance.ToggleCrossHair(false);
            playerShootController.currentWeaponControl.SetActiveScope(true);
            StartCoroutine(DelayCanOutToShoot(0.3f));
        }

        else if (currentweaponType == WeaponType.Rocket)
        {
            CrosshairControl.Instance.ChangeCrosshair(2);
            CameraManager.Instance.currentVirtualCamnera.SetCamSideUseTween((float)coverPoint.coverDirection * 0.2f + 0.5f, 0.35f);
            CameraManager.Instance.currentVirtualCamnera.SetVerticalArmLengthUseTween(0.5f, 0.35f);
            CameraManager.Instance.currentVirtualCamnera.SetVerticalFOVUseTween(30f, 0.35f, () =>
            {
                canOutToShoot = true;
            });
        }
        else
        {
            if (currentweaponType == WeaponType.ShotGun)
            {
                CrosshairControl.Instance.ChangeCrosshair(3);
            }
            else
            {
                CrosshairControl.Instance.ChangeCrosshair(2);
            }
            CameraManager.Instance.AimState(coverPoint.coverType, coverPoint.coverDirection);
            StartCoroutine(DelayCanOutToShoot(0.45f));
        }
        currentState = PlayerState.Shoot;
        PlayerUIControl.Instance.ShootState();
        boneRefrences.playerIK.UpperChestIKWeight(1, 0.5f);
        delayCanShoot = StartCoroutine(StartDelayCanShoot());
        IEnumerator StartDelayCanShoot()
        {
            yield return new WaitForSeconds(playerShootController.GetStartDelayWeaponCanShoot());
            isCanShoot = true;
            if (currentweaponType != WeaponType.Machinegun)
            {
                playerHealth.DelayCanTakeDame();
            }
          

        }
    }
    IEnumerator DelayCanOutToShoot(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        canOutToShoot = true;
    }
   
    #endregion

    #region ReLoad

    public void CoverToReload()
    {
        currentState = PlayerState.Reload;
        if (currentweaponType != WeaponType.Machinegun)
        {
            if (playerShootController.currentWeaponControl.magazinHandle != null)
            {
                animatorController.Reload(0);
                PlayerUIControl.Instance.Reloading(2);
            }
            else
            {
                animatorController.Reload(1);
                PlayerUIControl.Instance.Reloading(2);
            }
            AudioController.Instance.PlaySfx(GameConstain.PLAYERRELOADSMG);
        }
        else
        {
            PlayerUIControl.Instance.Reloading(2);
            StartCoroutine(DelayReloadAmmo(2));
            AudioController.Instance.PlaySfx(GameConstain.PLAYERRELOADMACHINEGUN);
        }
        if (delayCanShoot != null)
        {
           
            StopCoroutine(delayCanShoot);
        }
    }
    public void ShootToReload()
    {
        currentState = PlayerState.Reload;
        isCanShoot = false;
        if (currentweaponType != WeaponType.Machinegun)
        {
            if (playerShootController.currentWeaponControl.magazinHandle != null)
            {
                animatorController.Reload(0);
                PlayerUIControl.Instance.Reloading(2);
            }
            else
            {
                animatorController.Reload(1);
                PlayerUIControl.Instance.Reloading(2);
            }
            PlayerUIControl.Instance.DisplayState();
            playerHealth.CanTakeDame(false);
            animatorController.Aim(false);
            CameraManager.Instance.CoverState(coverPoint.coverType, coverPoint.coverDirection);
            AudioController.Instance.PlaySfx(GameConstain.PLAYERRELOADSMG);
        }
        else
        {
            PlayerUIControl.Instance.Reloading(2);
            StartCoroutine(DelayReloadAmmo(2));
            PlayerUIControl.Instance.MachineState();
            CameraManager.Instance.currentVirtualCamnera.SetVerticalFOVUseTween(60, 0.35f);
            AudioController.Instance.PlaySfx(GameConstain.PLAYERRELOADMACHINEGUN);
        }
        playerHealth.CanTakeDame(false);
        boneRefrences.playerIK.UpperChestIKWeight(0, 0.5f);
        if (currentweaponType == WeaponType.ShotGun)
        {
            CrosshairControl.Instance.ChangeCrosshair(1);
        }
        else if (currentweaponType == WeaponType.Sniper)
        {
            CrosshairControl.Instance.ToggleCrossHair(true);
        }
        else
        {
            CrosshairControl.Instance.ChangeCrosshair(0);
        }
       
       
        //if (currentweaponType == WeaponType.Rocket)
        //{
        //    CameraManager.Instance.currentVirtualCamnera.SetCamSideUseTween((float)coverPoint.coverDirection, 0.25f);
        //    CameraManager.Instance.currentVirtualCamnera.SetVerticalArmLengthUseTween(0.1f, 0.25f);
        //}
        if (delayCanShoot != null)
        {
            StopCoroutine(delayCanShoot);
        }
    }
  IEnumerator DelayReloadAmmo(float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        playerShootController.ReloadAmmo();
    }
    #endregion

    #region Granade
   
    public void CoverToGrenade()
    {
        animatorController.Holster();
        currentState = PlayerState.Swap;
        StartCoroutine(DelayChangeToGrenadeState());
        IEnumerator DelayChangeToGrenadeState()
        {
            yield return new WaitForSeconds(0.7f);
            currentState = PlayerState.Grenade;
            animatorController.ThrowGrenade((float)coverPoint.coverDirection);
            Quaternion targetRotation = CameraManager.Instance.GetTargetRotation();
            playerModel.transform.DOLocalRotate(new Vector3(0, targetRotation.eulerAngles.x - 45, 0), 0.75f);
        }

    }
    public void GrenadeToCover()
    {
        animatorController.Equip();
        animatorController.Aim(false);
        currentState = PlayerState.Swap;
        PlayerUIControl.Instance.DisplayState();
      
    }
    #endregion
  
    public void GetHit()
    {
        if (currentweaponType==WeaponType.Sniper) return;
        if (animatorController.IsInState("GetHit", 5)) return;
        animatorController.GetHit(UnityEngine.Random.Range(0, 2));
    }
     public Transform GetHeadTargetTrans()
    {
        return boneRefrences.playerIK.chestBone;
    }
    public void OnWin()
    {
        StopAllCoroutines();
        currentState = PlayerState.Win;
        animatorController.Aim(false, (float)coverPoint.coverType, (float)coverPoint.coverDirection);
        PlayerUIControl.Instance.SetDisplay(false);

    }
    public void Revive()
    {
        playerHealth.Revive();
        animatorController.animator.enabled = true;
        PlayerUIControl.Instance.SetDisplay(true);
        CameraManager.Instance.SetBlend(Cinemachine.CinemachineBlendDefinition.Style.Cut, 0);
        if (coverPoint.supCamPoint != null)
        {
            CameraManager.Instance.ChangeCamera(CameraType.Support, coverPoint.supCamPoint.transform);
            CameraManager.Instance.currentVirtualCamnera.SetCamDistanceUseTween(coverPoint.supCamPoint.distance, 0.5f);
            CameraManager.Instance.currentVirtualCamnera.SetVerticalArmLengthUseTween(coverPoint.supCamPoint.verticalArmLenght, 0.5f);
        }
        else
        {
            switch (currentweaponType)
            {
              
                case WeaponType.Machinegun:
                    CameraManager.Instance.ChangeCamera(CameraType.Machinegun);
                    break;
                case WeaponType.Sniper:
                    CameraManager.Instance.ChangeCamera(CameraType.Sniper);
                    break;
                case WeaponType.Rocket:
                    CameraManager.Instance.ChangeCamera(CameraType.Rocket);
                    break;
                default:
                    CameraManager.Instance.ChangeCamera(CameraType.Base);
                    break;
            }
           
        }
        ShootToCover();
    }
    public void OnDead()
    {
        currentState = PlayerState.Dead;
        animatorController.OnDead(Vector3.zero);
        GamePlayUIManager.Instance.OnFail();
        PlayerUIControl.Instance.SetDisplay(false);
        PlayerUIControl.Instance.isAim = false;
        CameraManager.Instance.SetBlend(Cinemachine.CinemachineBlendDefinition.Style.EaseIn, 1);
        if(currentweaponType == WeaponType.Sniper)
        {
            playerShootController.currentWeaponControl.SetActiveScope(false);
            CameraManager.Instance.mainCamera.enabled = true;
        }
        CameraManager.Instance.ChangeCamera(CameraType.Dead);
        CameraManager.Instance.currentVirtualCamnera.SetLookAt(boneRefrences.playerIK.upperChestBone);
        CameraManager.Instance.TriggerDeadImpulse();

    }
    public CoverDirection GetCoverDirection()
    {
        return coverPoint.coverDirection;
    }
    public CoverType GetCoverType()
    {
        return coverPoint.coverType;
    }

}
public enum PlayerState
{
    Ready,
    Move,
    Cover,
    Grenade,
    Heal,
    Reload,
    Shoot,
    Swap,
    Win,
    Dead
}

