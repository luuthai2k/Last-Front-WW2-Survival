using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TopBarController : Singleton<TopBarController>
{
    public TextMeshProUGUI cashTxt, goldTxt, metalTxt;
    private void Start()
    {
        UpdateText();
    }
    public void UpdateText()
    {
        goldTxt.text =ToolHelper.FormatLong2( DataController.Instance.Gold);
        cashTxt.text = ToolHelper.FormatLong2(DataController.Instance.Cash);
        metalTxt.text = ToolHelper.FormatLong2(DataController.Instance.Metal);
    }
}
