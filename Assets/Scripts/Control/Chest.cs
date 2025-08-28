using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator animator;
    public Action onOpenChest, onDropChest;
    public KeyValueRandom[] KeyValue;
    [SerializeField] private List<KeyValue> keyValueCards, keyValueItems;
    public List<WeaponInGameData> weaponInGames;
    public int numberCard;
    public void Start()
    {
        animator.SetTrigger("start");
    }
    public void Init(bool isTutorial )
    {
        if (isTutorial)
        {
            keyValueItems.Add(new KeyValue("Cash", "400"));
            keyValueItems.Add(new KeyValue("Grenade", "1"));
            keyValueItems.Add(new KeyValue("MedKit", "2"));
            DataController.Instance.AddCash(400);
            DataController.Instance.AddGrenade(1);
            DataController.Instance.AddMedKit(2);
            var smg=DataController.Instance.GetCurrentWeaponInGameData(WeaponType.SMG);
            keyValueCards.Add(new KeyValue($"card_0_{smg.ID}", "20"));
            weaponInGames.Add(smg);
            smg.AddCard(20);
        }
        else
        {
            RamdomRewardItems();
            RandomCards();
        }
       
     
    }
    public void OnOpenChest()
    {
        onOpenChest?.Invoke();
        AudioController.Instance.PlaySfx(GameConstain.CHEST_OPEN);
    }
    public void OnDropChest()
    {
        onDropChest?.Invoke();
        AudioController.Instance.PlaySfx(GameConstain.CHEST_DROP);
    }
    public void RandomCards()
    {
        keyValueCards = new List<KeyValue>();
        weaponInGames = GetRandomWeaponInGameData(4);
        var listNumberCards = RandomNumberCard(weaponInGames.Count);
        for (int i = 0; i < weaponInGames.Count; i++)
        {
            weaponInGames[i].AddCard(listNumberCards[i]);
            keyValueCards.Add(new KeyValue($"Card_{(int)weaponInGames[i].weaponType}_{weaponInGames[i].ID}", listNumberCards[i].ToString()));
        }
    }
    public void RamdomRewardItems()
    {
        keyValueItems = new List<KeyValue>();
        for (int i=0;i< KeyValue.Length; i++)
        {
            if (DataController.Instance.Level < 7 && KeyValue[i].Key == "Metal") { }
            else
            {
                int value = KeyValue[i].GetRandomValueToInt();
                if (value > 0)
                {
                    KeyValue item = new KeyValue(KeyValue[i].Key, value.ToString());
                    keyValueItems.Add(item);
                    DataController.Instance.GetReward(item);
                }
            }
          
        }
    }
    public List<KeyValue> GetRewardCards()
    {
        return keyValueCards;
    }
    public List<KeyValue> GetRewardItems()
    {
        return keyValueItems;
    }
    public int[] RandomNumberCard(int numberPart)
    {
        int[] parts = new int[numberPart];
        int remaining = numberCard;
        for (int i = 0; i < numberPart - 1; i++)
        {
            parts[i] = UnityEngine.Random.Range(5, remaining/2);
            remaining -= parts[i];
        }
        parts[numberPart-1] = remaining;
        return parts;
    }
    public List<WeaponInGameData> GetRandomWeaponInGameData(int number)
    {
        var data = DataController.Instance.GetAllWeaponInGameDataCanUpdate();
        int n = data.Count;
        if (n < number)
        {
            Debug.LogError("List must contain at least 4 elements.");
            return data;
        }
        var copy = new List<WeaponInGameData>(data);
        for (int i = 0; i < number; i++)
        {
            int randIndex = UnityEngine.Random.Range(i, n);
            WeaponInGameData temp = copy[i];
            copy[i] = copy[randIndex];
            copy[randIndex] = temp;
        }
        return copy.GetRange(0, number);
    }
}
