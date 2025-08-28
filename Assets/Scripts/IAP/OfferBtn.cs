using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class OfferBtn : MonoBehaviour
{
    public string packId;
    public void OnClickBtn()
    {
       _= UIManager.Instance.GetOfferPanel(packId);
        FirebaseServiceController.Instance.LogEvent($"ENTER_{packId}_MAIN");
    }
}
