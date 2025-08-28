using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FailPopUp : MonoBehaviour
{
    public void MainMenu()
    {
        if (DataController.Instance.Key >= 3)
        {
            GameObject go = UIManager.Instance.SpawnAirdropPanel();
            go.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
            {
                SceneController.Instance.LoadScene(GameConstain.MainMenu, "Main_menu_loading");
            });
        }
        else
        {
            SceneController.Instance.LoadScene(GameConstain.MainMenu, "Main_menu_loading");
        }
      
    }
  
    public void TryAgain()
    {
        SceneController.Instance.LoadScene(GameConstain.GamePlay, "Mission_loading", false);
    }

}
