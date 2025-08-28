using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class IntroPanelManager : MonoBehaviour
{
    public TextMeshProUGUI dateTxt,contentTxt;
    public AudioClip[] audioClips;
    public string date,content;
    public System.Action onComplete;
    public void Spawn(string date, string content, System.Action onComplete)
    {
        var introPanel = Instantiate(this);
        introPanel.date = date;
        introPanel.content = content;
        introPanel.onComplete = onComplete;
    }
    IEnumerator Start()
    {
        dateTxt.text = null;
        contentTxt.text = null;
        yield return new WaitForSeconds(0.5f);
        dateTxt.DOText(date, date.Length * 0.1f);
        StartCoroutine(PlaySfx( date.Length * 0.1f));
        yield return new WaitForSeconds(date.Length * 0.1f + 0.5f);
        contentTxt.DOText(content, content.Length * 0.1f);
        StartCoroutine(PlaySfx( content.Length * 0.1f));
        yield return new WaitForSeconds(content.Length * 0.1f+0.5f);
        onComplete?.Invoke();
        gameObject.SetActive(false);
    }
    IEnumerator PlaySfx(float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += 0.1f;
            AudioController.Instance.PlaySfx(audioClips[Random.Range(0, audioClips.Length)]);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
