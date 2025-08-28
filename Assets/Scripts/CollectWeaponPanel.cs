using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectWeaponPanel : MonoBehaviour
{
    [SerializeField] protected Image weaponIcon;
    public TextMeshProUGUI nameTxt;
    private void OnEnable()
    {
        ManagerAds.ins.HideMrec();
    }
    public GameObject Spawm(WeaponInGameData data)
    {
        GameObject go = Instantiate(gameObject);
        go.GetComponent<CollectWeaponPanel>().Init(data);
        return go;
    }
    public GameObject Spawm(Sprite icon, string nameID)
    {
        GameObject go = Instantiate(gameObject);
        go.GetComponent<CollectWeaponPanel>().Init(icon, nameID);
        return go;
    }
    public void Init(WeaponInGameData data)
    {
        UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<UnityEngine.U2D.SpriteAtlas>("Weapon_Icon.spriteatlas").Completed += (UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<UnityEngine.U2D.SpriteAtlas> obj) =>
        {
            weaponIcon.sprite = obj.Result.GetSprite("Icon_" + data.ID);
        };
        LoadNameText(data.ID);
    }
    public void Init(Sprite icon, string nameID)
    {

        weaponIcon.sprite = icon;
        LoadNameText(nameID);
    }


    async void LoadNameText(string nameID)
    {
        nameTxt.text = await LocalizationManager.Instance.GetLocalizedText(nameID);
    }
    public void OnClickClose()
    {
        gameObject.SetActive(false);
    }
}
