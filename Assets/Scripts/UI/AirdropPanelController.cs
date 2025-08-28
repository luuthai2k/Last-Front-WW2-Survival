using DG.Tweening;
using InviGiant.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AirdropPanelController : MonoBehaviour
{
    private AirdropRate[] airdropRates;
    public Image weaponIcon;
    public GameObject goWeaponContent;
    public ChestElement[] chestElements;
    public Image[] keyIcons;
    public Image[] keyBtnIcons;
    public bool canClick = false;
    public int currentTurn;
    public int multi;
    public TextMeshProUGUI cashTxt, goldTxt, metalTxt;
    public GameObject goSkipItBtn,goContinueBtn, goKeys,goGetMoreKeyBtn;
    public int moreKeyCount;
    private GrenadeLootView grenadeLootView;
    private WeaponInGameData weaponInGameData;
    public RectTransform targetGoldRectTrans, targetCashRectTrans;
    private void Start()
    {
        multi = 1;
        airdropRates = DataDefine.GetAirdropRate();
        Init();
        UpdateKey();
        UpdateText();
    }
    public GameObject Spawm()
    {
        return Instantiate(gameObject);
    }
    public void UpdateText()
    {
        goldTxt.text = ToolHelper.FormatLong2(DataController.Instance.Gold);
        cashTxt.text = ToolHelper.FormatLong2(DataController.Instance.Cash);
        metalTxt.text = ToolHelper.FormatLong2(DataController.Instance.Metal);
    }
   public void Init()
    {
        weaponInGameData = DataController.Instance.GetRandomAirdropWeapons();
        if (weaponInGameData == null)
        {
            goWeaponContent.SetActive(false);
        }
        else
        {
            UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<UnityEngine.U2D.SpriteAtlas>("Weapon_Icon.spriteatlas").Completed += (UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<UnityEngine.U2D.SpriteAtlas> obj) =>
            {
                weaponIcon.sprite = obj.Result.GetSprite("Icon_" + weaponInGameData.ID);
                weaponIcon.gameObject.SetActive(true);
            };
        }
       
        StartCoroutine(DisplayChest());
    }
    IEnumerator DisplayChest()
    {
        for (int i = 0; i < airdropRates.Length; i++)
        {
            chestElements[i].transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(0.15f);
        canClick = true;
    }
    public void OnBtnChestClick(int index)
    {
        if (!canClick||DataController.Instance.Key<=0) return;
        KeyValue reward = RandomReward();

        switch (reward.Key)
        {
            case "Cash":
                DataController.Instance.AddCash(reward.GetValueToInt());
                SpawmCashsEffect(chestElements[index].transform.position, 10);
                chestElements[index].OnOpen(reward);
                break;
            case "Gold":
                DataController.Instance.AddGold(reward.GetValueToInt());
                SpawmGoldsEffect(chestElements[index].transform.position,reward.GetValueToInt());
                chestElements[index].OnOpen(reward);
                break;
            case "Grenade":
                DataController.Instance.AddGrenade(reward.GetValueToInt());
                SpawmGrenadeEffect(chestElements[index].transform.position, reward.GetValueToInt());
                chestElements[index].OnOpen(reward);
                break;
            case "Weapon":
                if (weaponInGameData == null)
                {
                    reward.Key = "Cash";
                    reward.Value = (Random.Range(200, 376) * multi).ToString();
                    DataController.Instance.AddCash(reward.GetValueToInt());
                    SpawmCashsEffect(chestElements[index].transform.position, 10);
                    chestElements[index].OnOpen(reward);
                }
                else
                {
                    weaponInGameData.isOwned = true;
                    UIManager.Instance.SpawmWeapon(weaponInGameData);
                    chestElements[index].OnOpen(reward, weaponIcon.sprite);
                }
              
                break;

        }
        DataController.Instance.AddKey(-1);
        currentTurn++;
        if (currentTurn == 9)
        {
            goKeys.SetActive(false);
            goContinueBtn.SetActive(true);
          
        }
        else
        {
            UpdateMulti();
            UpdateKey();
        }
       
    }
    public void UpdateMulti()
    {
        switch (currentTurn)
        {
            case 0:
            case 1:
            case 2:
                multi = 1;
                break;
            case 3:
            case 4:
            case 5:
                multi = 3;
                break;
            case 6:
                multi = 5;
                break;
            case 7:
                multi = 7;
                break;
            case 8:
                multi = 10;
                break;
        }
        for(int i = 0; i < chestElements.Length; i++)
        {
            chestElements[i].UpdateMultiTxt(multi);
        }
    }
    public void UpdateKeysRewardAds()
    {
        switch (currentTurn)
        {
         
            case 6:
            case 7:
            case 8:
                moreKeyCount = 1;
                break;
           default:
                moreKeyCount = 3;
                break;
        }
    }
    public KeyValue RandomReward()
    {
        int rate = Random.Range(1, 101);
        int cumulative = 0;
        KeyValue reward = new KeyValue(airdropRates[currentTurn].reward[0].Key,"0");
        Debug.Log(rate);
        for (int i = 0; i < airdropRates[currentTurn].reward.Length; i++)
        {
            cumulative += airdropRates[currentTurn].reward[i].GetValueToInt();
            if (rate <= cumulative)
            {
                reward.Key = airdropRates[currentTurn].reward[i].Key;
                break;
            }
        }
        switch (reward.Key)
        {
            case "Cash":
                reward.Value = (Random.Range(200, 376)*multi).ToString();
                break;
            case "Gold":
                rate = Random.Range(1, 101);
                if (rate <= 50)
                {
                    reward.Value = multi.ToString();
                }
                else if (rate <= 70)
                {
                    reward.Value = (2*multi).ToString();
                }
                else if (rate <= 85)
                {
                    reward.Value = (3 * multi).ToString();
                }
                else if (rate <= 95)
                {
                    reward.Value = (4 * multi).ToString();
                }
                else
                {
                    reward.Value = (5 * multi).ToString();
                }
                break;
            default:
                reward.Value = multi.ToString();
                break;

        }
        return reward;
     
    }
public void UpdateKey()
    {
      
        if (DataController.Instance.Key == 0)
        {
            UpdateKeysRewardAds();
            goKeys.SetActive(false);
            goGetMoreKeyBtn.SetActive(true);
            goSkipItBtn.SetActive(true);
            for (int i = 0; i < keyBtnIcons.Length; i++)
            {
                if (i < moreKeyCount)
                {
                    keyBtnIcons[i].gameObject.SetActive(true);
                }
                else
                {
                    keyBtnIcons[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            goKeys.SetActive(true);
            goGetMoreKeyBtn.SetActive(false);
            goSkipItBtn.SetActive(false);
            for (int i = 0; i < keyIcons.Length; i++)
            {
                if (i <= DataController.Instance.Key - 1)
                {
                    keyIcons[i].color = Color.white;
                }
                else
                {
                    keyIcons[i].color = Color.black;
                }
            }
        }
    }
    public void OnClickGetMoreKeysBtn()
    {
        ManagerAds.ins.ShowRewarded((x) =>
        {
            if (x)
            {
                DataController.Instance.AddKey(moreKeyCount);
                UpdateKey();
            }
        });
    }
    public void OnClickSkipItBtn()
    {
        gameObject.SetActive(false);
    }
    public void SpawmCashsEffect(Vector3 spawnPos,int amount)
    {
        amount = System.Math.Min(amount, 20);
        AudioController.Instance.PlaySfx(GameConstain.REWARD_POP);
        for (int i = 0; i < amount; i++)
        {
            GameObject coin = SmartPool.Instance.Spawn(UIManager.Instance.cashItemPropPrefab, transform);
            coin.transform.position = spawnPos;
            coin.GetComponent<ItemProp>().MoveTween(targetCashRectTrans.position, spawnPos, new Vector2(150,150), targetCashRectTrans.sizeDelta, 0.8f, i * 0.05f + 0.05f,
                (() =>
                {

                    SmartPool.Instance.Despawn(coin);
                }));
        }
        UpdateCashTextUseTween();
    }
    public void SpawmGoldsEffect(Vector3 spawnPos,int amount)
    {
       
        amount = System.Math.Min(amount, 20);
        Debug.LogError(amount);
        AudioController.Instance.PlaySfx(GameConstain.REWARD_POP);
        for (int i = 0; i < amount; i++)
        {
            GameObject gold = SmartPool.Instance.Spawn(UIManager.Instance.goldItemPropPrefab, transform);
            gold.transform.position = spawnPos;
            gold.GetComponent<ItemProp>().MoveTween(targetGoldRectTrans.position, spawnPos, new Vector2(150, 150), targetGoldRectTrans.sizeDelta, 0.8f, i * 0.05f + 0.05f,
                (() =>
                {
                    SmartPool.Instance.Despawn(gold);
                }));
        }
        UpdateGoldTextUseTween();
    }
    public void UpdateCashTextUseTween()
    {
        int from = ToolHelper.ParseFormattedStringToInt(cashTxt.text);
        DOVirtual.Int(from, DataController.Instance.Cash, 0.5f, (x) =>
        {
            cashTxt.text = ToolHelper.FormatInt2(x);
        }).SetDelay(0.9f);
    }
    public void UpdateGoldTextUseTween()
    {
        int from = ToolHelper.ParseFormattedStringToInt(goldTxt.text);
        DOVirtual.Int(from, DataController.Instance.Gold, 0.5f, (x) =>
        {
            goldTxt.text = ToolHelper.FormatInt2(x);
        }).SetDelay(0.9f);
    }
    public void SpawmGrenadeEffect( Vector3 spawnPos, int amount)
    {
        if (grenadeLootView == null)
        {
            grenadeLootView = UIManager.Instance.SpawmGrenadeLootView(transform).GetComponent<GrenadeLootView>();
        }
        if (!grenadeLootView.gameObject.activeSelf)
        {
            grenadeLootView.gameObject.SetActive(true);
        }
        grenadeLootView.SpawmGrenadeEffect(spawnPos, amount);

    }

}
