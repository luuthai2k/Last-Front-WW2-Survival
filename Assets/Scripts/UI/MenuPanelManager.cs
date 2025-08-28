using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MenuPanelManager : MonoBehaviour
{
    public GameObject goSettingPanel;
    public LocalizeStringEvent rewardLocalizeStringEvent;
    public HorizontalLayoutGroup horizontalLocationGroup;
    public DifficultyTab[] difficultyTabs;
    SpriteAtlas spriteAtlas;
    private void Start()
    {
        float currentAspect = ((float)Screen.width) / ((float)Screen.height);
        float defaultAspect = 1080 / 1920;
        if (currentAspect > defaultAspect)
        {
            horizontalLocationGroup.padding.left = (int)(currentAspect*1920 / 2 - 109);
        }
    }
    public void OnEnable()
    {
        Init();
    }
    public async void Init()
    {
     
        DataController.Instance.levelSelect = Mathf.Min(DataController.Instance.Level, GameConstain.MAXLEVEL);
        var levelData = DataController.Instance.GetLevelData(DataController.Instance.levelSelect);
        DataController.Instance.difficultySelect = 0;
        var udd = DataController.Instance.GetUnlockDifficultyData(DataController.Instance.levelSelect);
        if (spriteAtlas == null)
        {
            AsyncOperationHandle<SpriteAtlas> obj = Addressables.LoadAssetAsync<UnityEngine.U2D.SpriteAtlas>("Reward_Icon.spriteatlas");
            await obj.Task;
            spriteAtlas = obj.Result;
        }
        if (levelData.difficultyBonus[DataController.Instance.difficultySelect].Items.Length > 0)
        {
            Sprite sprite = spriteAtlas.GetSprite(levelData.difficultyBonus[DataController.Instance.difficultySelect].Items[0].Key);
            string value = levelData.difficultyBonus[DataController.Instance.difficultySelect].Items[0].Value;
            SetUpPlayBtn(udd.difficultys, DataController.Instance.difficultySelect, value, sprite );
        }
        if (levelData.difficultyBonus[DataController.Instance.difficultySelect].Crates.Length > 0)
        {
            Sprite sprite = spriteAtlas.GetSprite(levelData.difficultyBonus[DataController.Instance.difficultySelect].Crates[0].Key);
            SetUpPlayBtn(udd.difficultys, DataController.Instance.difficultySelect, "",sprite);
        }
       
    }
    
    void LoadRewardAmountText(string value)
    {
        var localizedString = rewardLocalizeStringEvent.StringReference;
        localizedString.Arguments = new object[] { value };
        rewardLocalizeStringEvent.RefreshString();
    }
    public void OnClickSettingBtn()
    {
        goSettingPanel.SetActive(true);
    }
    public void SetUpPlayBtn(int[] difficultys, int difficultyTabSelect, string value,Sprite icon)
    {
        LoadRewardAmountText(value);
        for (int i = 0; i < difficultys.Length; i++)
        {
            if (difficultys[i] == 1)
            {
                difficultyTabs[i].Select(true);
                difficultyTabs[i].Lock(false);
            }
            else if (difficultys[i] == 0)
            {
                difficultyTabs[i].Select(false);
                difficultyTabs[i].Lock(false);
            }
            else
            {
                difficultyTabs[i].Lock(true);
            }
        }
    }
   
}
