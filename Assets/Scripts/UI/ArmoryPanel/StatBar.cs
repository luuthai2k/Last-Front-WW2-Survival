using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class StatBar : MonoBehaviour
{
    public Image frontFillImg, backFillImg;
    public TextMeshProUGUI statTxt;
    public void SetUp(int currentValue, int updateValue,int maxValue)
    {
        if (currentValue < updateValue)
        {
            frontFillImg.fillAmount = (float)currentValue/maxValue;
            backFillImg.fillAmount = (float)updateValue /maxValue;
            backFillImg.color = Color.green;
            statTxt.text = currentValue.ToString() + "<color=green> + " + (updateValue-currentValue).ToString() + "</color>";
        }
        else if(currentValue == updateValue)
        {
            frontFillImg.fillAmount = (float)currentValue / maxValue;
            backFillImg.fillAmount = 0;
            statTxt.text = currentValue.ToString();
        }
        else
        {
            frontFillImg.fillAmount = (float)updateValue /maxValue;
            backFillImg.fillAmount = (float)currentValue /maxValue;
            backFillImg.color = Color.red;
            statTxt.text = currentValue.ToString() + "<color=red> - " + (currentValue- updateValue).ToString() + "</color>";

        }
    }
   
}
