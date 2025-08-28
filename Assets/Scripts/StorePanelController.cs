using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class StorePanelController : MonoBehaviour
{
    public RectTransform contentRectTrans, cratesHeaderRectTrans, suppliesHeaderRectTrans, premiumWeaponsHeaderRectTrans, goldsHeaderRectTrans;
    public Scrollbar scrollbarVertical;
    public GameObject goFreeCrateBtn;
    public void Start()
    {
        CheckFreeCrate();
    }
    public void CheckFreeCrate()
    {
        if (PlayerPrefs.GetInt("Free_Crate", 0) == 0&&DataController.Instance.Level>=6)
        {
            goFreeCrateBtn.gameObject.SetActive(true);
        }
    }
    public void Forcus(string forcus,bool useTween=true)
    {
        switch (forcus)
        {
            case "Crates":
                SetVerticalBar(cratesHeaderRectTrans,useTween);
                break;
            case "Supplies":
                Debug.LogWarning(suppliesHeaderRectTrans.anchoredPosition.y);
                SetVerticalBar(suppliesHeaderRectTrans, useTween);
                break;
            case "PremiumWeapons":
                SetVerticalBar(premiumWeaponsHeaderRectTrans, useTween);
                break;
            case "Gold":
            case "Money":
                SetVerticalBar(goldsHeaderRectTrans, useTween);
                break;
        }
    }
    public void SetVerticalBar(RectTransform childRectTrans,bool useTween)
    {
       
        if (childRectTrans.anchoredPosition.y == 0)
        {
            Canvas.ForceUpdateCanvases();
        }
        Debug.Log(childRectTrans.anchoredPosition.y);
        float value =Mathf.Clamp01( 1 - (Mathf.Abs(childRectTrans.anchoredPosition.y+50) / contentRectTrans.sizeDelta.y));
        if (useTween)
        {
            DOTween.To(() => scrollbarVertical.value, x => scrollbarVertical.value = x, value, 0.5f)
          .SetEase(Ease.OutQuad);

        }
        else
        {
            scrollbarVertical.value = value;
        }
      
    }
    public void OnClickFreeCrateBtn()
    {
        MainMenuUIManager.Instance.gameObject.SetActive(false);
        GameObject go = UIManager.Instance.SpawnLootBox("SmallCrate"); ;
        go.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
        {

            MainMenuUIManager.Instance.gameObject.SetActive(true);

        });
        goFreeCrateBtn.gameObject.SetActive(false);
        PlayerPrefs.SetInt("Free_Crate", 1);

    }

}
