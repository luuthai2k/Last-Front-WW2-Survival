using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;
using System;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class RegionElement : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public GameObject goLock, goInfo;
    public int unlockLevel;
    public LocalizeStringEvent localizeStringEvent;
   
    public void Start()
    {
        if (unlockLevel > GameConstain.MAXLEVEL)
        {

            LoadComingSoonText();
        }
        else if (DataController.Instance.Level < unlockLevel)
        {
            LoadLevelRequierText();
        }
    }
    void LoadLevelRequierText()
    {
        int index = unlockLevel-DataController.Instance.Level;
        var localizedString = localizeStringEvent.StringReference;
        localizedString.Arguments = new object[] { index };
        localizeStringEvent.RefreshString(); 
    }
    void LoadComingSoonText()
    {
        localizeStringEvent.StringReference.TableEntryReference = "Coming_soon";
        localizeStringEvent.RefreshString();
    }
    public void SetUp(Material material, bool activeGoLock, bool activeGoInfo)
    {
        spriteRenderer.material = material;
        goLock.SetActive(activeGoLock);
        goInfo.SetActive(activeGoInfo);
    }

}
