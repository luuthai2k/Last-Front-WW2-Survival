using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using TMPro;
using Samples.Purchasing.Core.IAPManager;
//[ExecuteInEditMode]
public class TMPPrice : MonoBehaviour
{
    [Tooltip("This pack id will automatic sync with the IAP Button pack ")]
    [SerializeField] private string productId;
    [SerializeField] private TextMeshProUGUI priceText;
    void OnEnable()
    {
        string priceStr= IAPManager.Instance.GetLocalPrice(productId);
        Debug.LogError(priceStr);
        if (priceStr != null)
        {
            priceText.text = priceStr;
        }
        else
        {
            priceText.text = IAPPackHelper.GetPackPrice(productId).ToString() + "$";
        }
       
    }
}
