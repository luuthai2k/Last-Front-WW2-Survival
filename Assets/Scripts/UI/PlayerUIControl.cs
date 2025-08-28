using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using UnityEngine.UI;
public class PlayerUIControl : Singleton<PlayerUIControl>
{
    public GameObject goShootBtn,goReloadBtn,goHealBtn,goGrenadeBtn;
    [SerializeField] private Transform reloadTrans;
    //[SerializeField] private RectTransform scopeRectrans;
    [SerializeField] private FadeScreenEffect damageScreenEffect,healScreenEffect;
    [SerializeField] private Image reloadImg;
    //Image scopeImg;
    public bool isAim,isReloading,isGrenade,isHeal,isSwap;
    public WeaponUIEquipment[] weaponUIEquipments;
    SpriteAtlas spriteAtlas;
    public async void SetUpWeaponUIEquipments(List<WeaponInGameData> datas)
    {
        foreach(var w in weaponUIEquipments)
        {
            w.gameObject.SetActive(false);
        }
        if (datas==null||datas.Count == 0) return;
        if (spriteAtlas == null)
        {
            AsyncOperationHandle<SpriteAtlas> obj = Addressables.LoadAssetAsync<UnityEngine.U2D.SpriteAtlas>("Weapon_Icon.spriteatlas");
            await obj.Task;
            spriteAtlas = obj.Result;
        }
       
        for (int i = 0; i < datas.Count; i++)
        {
            weaponUIEquipments[i].SetUp(datas[i], spriteAtlas.GetSprite($"Icon_{datas[i].ID}"));
            weaponUIEquipments[i].gameObject.SetActive(true);
        }
        weaponUIEquipments[0].SetEquip(true);
    }
   
    public void OnPointDownShootBtn()
    {
        isAim = true;
    }
    public void OnPointUpShootBtn()
    {
        isAim = false;
    }
    public void OnClickReloadBtn()
    {
        if (isReloading) return;
        isReloading = true;
    }
    public void Reloading(float time = 2)
    {
        reloadImg.gameObject.SetActive(true);
        reloadImg.fillAmount = 0;
        reloadImg.DOFillAmount(1, time);
        reloadTrans.DOLocalRotate(new Vector3(0, 0, -360), time, RotateMode.LocalAxisAdd).SetEase(Ease.OutSine);
        StartCoroutine(DelayactiveReloadImg());
        IEnumerator DelayactiveReloadImg()
        {
            yield return new WaitForSeconds(time);
            reloadImg.gameObject.SetActive(false);
            reloadTrans.rotation = Quaternion.identity;
            isReloading = false;

        }
    }
    public void OnClickGrenadeBtn()
    {
        if (DataController.Instance.Grenade <= 0)
        {
            ManagerAds.ins.ShowRewarded((x) =>
            {
                if (x)
                {
                    DataController.Instance.AddGrenade(1);
                    GamePlayUIManager.Instance.gamePlayMenu.UpdateGrenadeText();
                }

            });
        }
        else
        {
            PlayerController.Instance.OnGrenade();
        }
      


    }
   
    public void OnClickHealBtn()
    {
        if (isReloading) return;
        if (isHeal) return;
        PlayerPrefs.SetInt(GameConstain.HEAL_TUT, 1);
        if (DataController.Instance.MedKit <= 0)
        {
            ManagerAds.ins.ShowRewarded((x) =>
            {
                if (x)
                {
                    DataController.Instance.AddMedKit(1);
                    GamePlayUIManager.Instance.gamePlayMenu.UpdateMedKitText();
                }
              
            });
        }
        else
        {
            DataController.Instance.MedKit--;
            GamePlayUIManager.Instance.gamePlayMenu.UpdateMedKitText();
            isHeal = true;
            StartCoroutine(DelayHeal());
            AudioController.Instance.PlaySfx(GameConstain.HEAL);
        }
    }
    IEnumerator DelayHeal()
    {
        yield return new WaitForSeconds(0.25f);
        HealEffect();
        yield return new WaitForSeconds(1f);
        HealEffect();
        yield return new WaitForSeconds(0.75f);
        isHeal = false;
    }
 
    //public void ToggleScope(bool active)
    //{
      
    //    if (active)
    //    {
    //        if (!scopeRectrans.gameObject.activeSelf)
    //        {
    //            scopeRectrans.anchoredPosition=new Vector2(500, -Screen.height * 0.75f);
    //            scopeRectrans.gameObject.SetActive(true);
    //        }
          
    //        if (scopeImg == null)
    //        {
    //            scopeImg = scopeRectrans.GetComponent<Image>();
    //        }
    //        scopeImg.color = Color.white;
    //        scopeRectrans.DOKill();
    //        scopeImg.DOKill();
    //        float scaleFactor = (float)Screen.width / 1080f;
    //        scopeRectrans.sizeDelta = new Vector2(Screen.height/scaleFactor, Screen.height/scaleFactor);
    //        scopeImg.color = Color.white;
    //        scopeRectrans.DOScale(1, 0.25f);
    //        scopeRectrans.DOAnchorPos(Vector2.zero, 0.25f);
    //    }
    //    else
    //    {
          
    //        scopeRectrans.DOAnchorPos(new Vector2(500, -Screen.height * 0.75f), 0.5f);
    //        scopeRectrans.DOPunchScale(Vector3.one*0.2f, 0.35f);
    //        scopeImg.DOFade(0, 0.35f).OnComplete(() =>
    //        {
    //            scopeRectrans.gameObject.SetActive(false);
    //        });

    //    }

    //}
    
    public void SetDisplay(bool onDisplay)
    {
        transform.GetChild(0).gameObject.SetActive(onDisplay);

    }
    public void HideState()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        isAim = false;
    }
    public void DisplayState()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        goReloadBtn.SetActive(true);
        goHealBtn.SetActive(true);
        goGrenadeBtn.SetActive(true);
    }
    public void ShootState()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        goReloadBtn.SetActive(false);
        goHealBtn.SetActive(false);
        goGrenadeBtn.SetActive(false);
    }
    public void TutorialState()
    {

        goHealBtn.transform.localScale = Vector3.zero;
        goGrenadeBtn.transform.localScale = Vector3.zero;
    }

    public void MachineState()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        goReloadBtn.SetActive(true);
        goHealBtn.SetActive(true);
        goGrenadeBtn.SetActive(false);
    }
    public void RoketState()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        goReloadBtn.SetActive(false);
        goHealBtn.SetActive(true);
        goGrenadeBtn.SetActive(false);
    }
    public void TakeDameEffect()
    {
        damageScreenEffect.Play();
    }
    public void HealEffect()
    {
        healScreenEffect.Play();
    }
    public void OnClickWeaponBtn(WeaponUIEquipment weaponUIEquipment)
    {
       
        if (PlayerController.Instance.currentState == PlayerState.Grenade)
        {
            PlayerController.Instance.OnSwap();
            return;
        }
        if (PlayerController.Instance.currentState != PlayerState.Cover) return;
        if (weaponUIEquipment.isEquip)
        {
            OnClickReloadBtn();
        }
        else
        {
            foreach (var w in weaponUIEquipments)
            {
                if (w == weaponUIEquipment)
                {
                    PlayerController.Instance.OnSwap();
                    w.SetEquip(true);
                }
                else
                {
                    w.SetEquip(false);
                }
            }
          
        }
    }
}
