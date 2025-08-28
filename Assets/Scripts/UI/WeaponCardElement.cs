using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCardElement : RewardElement
{
    public TextMeshProUGUI nameTxt,processTxt;
    public GameObject goUpgrade, goProcess;
    public Image processFillImg;
    public WeaponInGameData weaponInGameData;
    public void Init(WeaponInGameData data, KeyValue reward)
    {
        transform.localScale = Vector3.zero;
        weaponInGameData = data;
        this.reward = reward;
        UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<UnityEngine.U2D.SpriteAtlas>("Weapon_Icon.spriteatlas").Completed += (UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<UnityEngine.U2D.SpriteAtlas> obj) =>
        {
            rewardIcon.sprite = obj.Result.GetSprite("Icon_" + data.ID);
            transform.localScale = Vector3.one*1.5f;
            transform.DOScale(Vector3.one, 0.35f).SetEase(Ease.InOutBack);
        };
        rewardAmount.text = "+" + reward.Value;
        var updateData = DataController.Instance.GetWeaponUpdateData(data.weaponType, data.ID);
        int cardsRequire = updateData.weaponLevelDatas[data.level].cardsRequired;
        int currentCards = data.cards;
        if (currentCards >= cardsRequire)
        {
            goUpgrade.SetActive(true);
        }
        else
        {
            processTxt.text = $"{currentCards}/{cardsRequire}";
            goProcess.SetActive(true);
        }
        if (audioClip != null)
        {
            AudioController.Instance.PlaySfx(audioClip);
        }

        LoadNameText();
    }
    async void LoadNameText()
    {
        nameTxt.text = await LocalizationManager.Instance.GetLocalizedText(weaponInGameData.ID);
    }
    public GameObject Spawn(WeaponInGameData data,KeyValue reward, Transform parent)
    {
        GameObject go = Instantiate(gameObject, parent);
        go.GetComponent<WeaponCardElement>().Init(data, reward);
        return go;
    }
    public override void BoostReward()
    {
        weaponInGameData.AddCard(reward.GetValueToInt());
        int value = reward.GetValueToInt();
        int target = value * 2;
        DOTween.To(() => value, x =>
        {
            rewardAmount.text = x.ToString();
        }, target, 1).SetEase(Ease.OutSine)
        .OnComplete(() =>
        {
            rewardAmount.text = target.ToString();
            reward.Value = target.ToString();
        });

    }

}
