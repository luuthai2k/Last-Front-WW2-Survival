using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using UnityEngine.UI;

public class BattlePassLevel : MonoBehaviour
{
    public int level;
    public GameObject[] btnClaims;
    public GameObject[] collects;
    public Sprite[] levelIcons;
    public BattlePassLevelData data = new BattlePassLevelData();
#if UNITY_EDITOR
    public TextMeshProUGUI levelTxt;
    public Image freeRewardIcon;
    public TextMeshProUGUI freeamountTxt;
    public Image vipRewardIcon;
    public TextMeshProUGUI vipamountTxt;
    [ContextMenu("Load Data")]
    public void LoadData()
    {
        data = BattlePassHelper.GetLevelData(level);
        levelTxt.text = (level+1).ToString();
        LoadReward();
    }
    public void LoadReward()
    {
       
        if (data.freeRewards.Crates.Length > 0)
        {
            LoadSpriteAsync(freeRewardIcon, data.freeRewards.Crates[0].Key);
            freeamountTxt.text = data.freeRewards.Crates[0].Value;
        }
        if (data.freeRewards.Items.Length > 0)
        {
            LoadSpriteAsync(freeRewardIcon, data.freeRewards.Items[0].Key);
            freeamountTxt.text = "x" + data.freeRewards.Items[0].Value;
        }
        if (data.vipRewards.Crates.Length > 0)
        {
            LoadSpriteAsync(vipRewardIcon, data.vipRewards.Crates[0].Key);
            vipamountTxt.text = data.vipRewards.Crates[0].Value;
        }
        if (data.vipRewards.Items.Length > 0)
        {
            LoadSpriteAsync(vipRewardIcon, data.vipRewards.Items[0].Key);
            vipamountTxt.text = "x"+data.vipRewards.Items[0].Value;
        }
    }
    private void LoadSpriteAsync(Image icon, string key)
    {

        string path = $"Assets/TextureUI/RewardIcon/icon_{key}.png";
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);

        if (sprite != null)
        {
            icon.sprite = sprite;
            Debug.Log($"Loaded sprite: {key}");
        }
        else
        {
            Debug.LogError($"Sprite not found at: {path}");
        }

    }
#endif
    public void CanClaimRevardFree()
    {
        btnClaims[0].gameObject.SetActive(true);
    }
    public void CanClaimRevardVip()
    {
        btnClaims[1].gameObject.SetActive(true);
    }
    public void OnCollectedFreeReward()
    {
        btnClaims[0].gameObject.SetActive(false);
        collects[0].gameObject.SetActive(true);
    }
    public void OnCollectedVipReward()
    {
        btnClaims[1].gameObject.SetActive(false);
        collects[1].gameObject.SetActive(true);
    }
    public void ClamFreeReward()
    {
        data = BattlePassHelper.GetLevelData(level);
        DataController.Instance.GetGameData().battlePassDataSave.rewardFreeCanCollect.Remove(level);
        DataController.Instance.SaveData();
        UIManager.Instance.GetPackReward(data.freeRewards, () =>
        {
            MainMenuUIManager.Instance.battlePassPanelController.gameObject.SetActive(true);
        });
        MainMenuUIManager.Instance.battlePassPanelController.gameObject.SetActive(false);
        OnCollectedFreeReward();
        MainMenuUIManager.Instance.battlePassPanelController.CheckRewardNotification(DataController.Instance.GetGameData().battlePassDataSave);

    }
    public void ClamVipReward()
    {
        data = BattlePassHelper.GetLevelData(level);
        DataController.Instance.GetGameData().battlePassDataSave.rewardVipCanCollect.Remove(level);
        DataController.Instance.SaveData();
        UIManager.Instance.GetPackReward(data.vipRewards, () =>
        {
            MainMenuUIManager.Instance.battlePassPanelController.gameObject.SetActive(true);
        });
        MainMenuUIManager.Instance.battlePassPanelController.gameObject.SetActive(false);
        OnCollectedVipReward();
        MainMenuUIManager.Instance.battlePassPanelController.CheckRewardNotification(DataController.Instance.GetGameData().battlePassDataSave);
    }
}
