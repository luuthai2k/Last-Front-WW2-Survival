using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;

public class AutoDeactiveWithLevel : MonoBehaviour
{
    public int levelRequire;
    public LocalizeStringEvent localizeStringEvent;
    void Start()
    {
        if (DataController.Instance.Level >= levelRequire)
        {
            gameObject.SetActive(false);
        }
        else
        {
            if (localizeStringEvent != null)
            {
                LoadLevelRequierText();
            }
          
        }
    }
 
    void LoadLevelRequierText()
    {
        int index = levelRequire - DataController.Instance.Level;
        var localizedString = localizeStringEvent.StringReference;
        localizedString.Arguments = new object[] { index };
        localizeStringEvent.RefreshString();
    }
  

}
