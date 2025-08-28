using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardElement : MonoBehaviour
{
    public KeyValue reward;
    public Image rewardIcon;
    public TextMeshProUGUI rewardAmount;
    public AudioClip audioClip;
    public  void Init(KeyValue reward)
    {
        transform.localScale = Vector3.zero;
        this.reward = reward;
        if (reward.Value != "0")
        {
            rewardAmount.text = reward.Value;
        }
        UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<UnityEngine.U2D.SpriteAtlas>("Reward_Icon.spriteatlas").Completed += (UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<UnityEngine.U2D.SpriteAtlas> obj) =>
        {
            rewardIcon.sprite = obj.Result.GetSprite("Icon_" + reward.Key);
            transform.localScale = Vector3.one * 1.5f;
            transform.DOScale(Vector3.one, 0.35f).SetEase(Ease.InOutBack);
        };
        if (audioClip != null)
        {
            AudioController.Instance.PlaySfx(audioClip);
        }
    }
    public void Init(KeyValue reward,Sprite sprite)
    {
        this.reward = reward;
        if (reward.Value != "0")
        {
            rewardAmount.text = reward.Value;
        }
        rewardIcon.sprite = sprite;
        transform.localScale = Vector3.one * 1.5f;
        transform.DOScale(Vector3.one, 0.35f).SetEase(Ease.InOutBack);
        if (audioClip != null)
        {
            AudioController.Instance.PlaySfx(audioClip);
        }
    }
    public GameObject Spawn(KeyValue reward, Transform parent)
    {
        GameObject go = Instantiate(gameObject, parent);
        go.GetComponent<RewardElement>().Init(reward);
        return go;
    }
    public Texture GetSprite()
    {
        return rewardIcon.sprite.texture;
    }
    public virtual void BoostReward()
    {
        DataController.Instance.GetReward(reward);
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
    public KeyValue GetReward()
    {
        return reward;
    }
}
