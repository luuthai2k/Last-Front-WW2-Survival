using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextAnimation : MonoBehaviour
{
    public float Time;
    private TextMeshProUGUI txt;
    public bool OnShowAtStart = false;
    private string ShowText = "Loading";
    public bool isUseLanguage = false;
    public string Label;
    private void Start()
    {
        if (OnShowAtStart)
        {
            OnShow();
        }
    }
    private void OnEnable()
    {
        OnShow();
    }

    public void SetText(string txt)
    {
        ShowText = txt;
        OnShow();
    }

    public void OnShow()
    {
        if (isUseLanguage)
        {
            //ShowText = Lean.Localization.LeanLocalization.GetTranslationText(Label);
        }
        StopAllCoroutines();
        StartCoroutine(Animation());
    }

    public void OnHide()
    {
        txt.text = "";
        StopAllCoroutines();
    }

    IEnumerator Animation()
    {
        txt = GetComponent<TextMeshProUGUI>();
        txt.text = ShowText + "   ";
        yield return new WaitForSeconds(Time);
        txt.text = ShowText + ".  ";
        yield return new WaitForSeconds(Time);
        txt.text = ShowText + ".. ";
        yield return new WaitForSeconds(Time);
        txt.text = ShowText + "...";
        yield return new WaitForSeconds(Time);
        txt.text = ShowText + ".. ";
        yield return new WaitForSeconds(Time);
        txt.text = ShowText + ".  ";
        yield return new WaitForSeconds(Time);
        txt.text = ShowText + "   ";
        yield return new WaitForSeconds(Time);
        StartCoroutine(Animation());
    }
}
