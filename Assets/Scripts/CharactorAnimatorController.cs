using DG.Tweening;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class CharactorAnimatorController : MonoBehaviour
{
	public Animator animator;
    Tween speedTween,runHeightTween;
    public BodyPartControl[] bodyPartControls;
    public bool IsInState(string stateName, int layerIndex = 0)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
        return stateInfo.IsName(stateName);
    }
    public void SetFloat(string key , float value)
	{
		animator.SetFloat(key, value);
	}
    public void SetInteger(string key, int value)
    {
        animator.SetInteger(key, value);
    }
    public void SetBool(string key, bool active)
    {
        animator.SetBool(key, active);
    }
    public void SetTrigger(string key)
    {
        animator.SetTrigger(key);
    }

    public void ResetTrigger(string key)
    {
        animator.ResetTrigger(key);
    }
    public void Idle()
    {
        animator.SetTrigger(GameConstain.IDLE);
    }
    public void Move(float speed)
    {
        animator.SetBool(GameConstain.IS_COVER, false);
        animator.SetFloat(GameConstain.SPEED, speed);
        animator.SetBool(GameConstain.IS_MOVE, true);
       
    }
    public void StopMove(bool isStopping=true)
    {
        animator.SetBool(GameConstain.IS_MOVE, false);
        if (isStopping)
        {
            animator.SetTrigger(GameConstain.IS_STOPPING);
        }
       
    }
    public void Cover(float coverHeigth, float coverDirection)
    {
        animator.SetBool(GameConstain.IS_COVER, true);
        animator.SetFloat(GameConstain.COVER_HEIGHT, coverHeigth);
        animator.SetFloat(GameConstain.COVER_DIRECTION, coverDirection);
    }
    public void Cover()
    {
        animator.SetFloat(GameConstain.SPEED, 0);
    }
    public void Aim(bool active, float weaponType,float coverHeigth, float coverDirection)
    {
        animator.SetBool(GameConstain.AIM, active);
        animator.SetFloat(GameConstain.WEAPON_TYPE, weaponType);
        animator.SetFloat(GameConstain.COVER_HEIGHT, coverHeigth);
        animator.SetFloat(GameConstain.COVER_DIRECTION, coverDirection);
    }
    public void Aim(bool active, float coverHeigth, float coverDirection)
    {
        animator.SetBool(GameConstain.AIM, active);
        animator.SetFloat(GameConstain.COVER_HEIGHT, coverHeigth);
        animator.SetFloat(GameConstain.COVER_DIRECTION, coverDirection);
    }
    public void Aim(bool active, float weaponType)
    {
        animator.SetBool(GameConstain.AIM, active);
        animator.SetFloat(GameConstain.WEAPON_TYPE, weaponType);
    }
    public void Aim(bool active)
    {
        animator.SetBool(GameConstain.AIM, active);
    }
    public void Stand( float stand)
    {
        animator.SetFloat(GameConstain.STAND, stand);
    }
    public void Reload(float reloadType)
    {
        animator.SetTrigger(GameConstain.RELOAD);
        animator.SetFloat(GameConstain.RELOAD_TYPE,reloadType);
    }
    public void Grenade()
    {
        animator.SetTrigger(GameConstain.GRENADE);
    }
    public void ThrowGrenade(float coverDirection)
    {
        animator.SetFloat(GameConstain.COVER_DIRECTION, coverDirection);
        animator.SetTrigger(GameConstain.IS_THROW);
        animator.SetBool(GameConstain.AIM, true);
    }
    public void CancerGrenade()
    {
        animator.SetTrigger(GameConstain.GRENADE_CANCER);

    }
    public void Holster()
    {
        animator.SetTrigger(GameConstain.IS_HOLSTER);
      
    }
   
    public void Equip()
    {
        animator.SetTrigger(GameConstain.IS_EQUIP);

    }
    public async void OnDead(Vector3 direction)
    {
      
        for (int i = 0; i < bodyPartControls.Length; i++)
        {
            bodyPartControls[i].OnDead(direction);
        }
        await Task.Yield();
        animator.enabled = false;
    }
    public void GetHit(float hitType)
    {
        animator.SetTrigger(GameConstain.GET_HIT);
        animator.SetFloat(GameConstain.HIT_TYPE, hitType);
    }
    public void Recoil(float weaponType)
    {
        animator.SetFloat(GameConstain.WEAPON_TYPE, weaponType);
        animator.SetTrigger(GameConstain.IS_RECOIL);
    }

}
