using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSetFreeCrateBtnPosition : MonoBehaviour
{
    public Transform maskTrans, interactBtnTrans;
    IEnumerator Start()
    {
        yield return null;
        maskTrans.position = MainMenuUIManager.Instance.storePanelController.goFreeCrateBtn.transform.position;
        interactBtnTrans.position = MainMenuUIManager.Instance.storePanelController.goFreeCrateBtn.transform.position;
    }

}
