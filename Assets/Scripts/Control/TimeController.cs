using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    public bool isSlowMotion;
    private bool canKillTween;
    Tween tween;
    public void SetTimeScale(float targetScale, float duration)
    {
        DOTween.To(() => Time.timeScale, x =>
           {
               Time.timeScale = x;
               Time.fixedDeltaTime = 0.02f * x;
           }, targetScale, duration);
    }
    public void SetTimeScale(float targetScale)
    {
        Time.timeScale = targetScale;
        Time.fixedDeltaTime = 0.02f * targetScale;
    }
    public void DoSlowMotion(float targetScale, float duration, float holdTime,bool canKillTween=true)
    {
        isSlowMotion = true;
        this.canKillTween = canKillTween;
        tween = DOTween.To(() => Time.timeScale, x =>
        {
            Time.timeScale = x;
            Time.fixedDeltaTime = 0.02f / x;
        }, targetScale, duration).OnComplete(() =>
        {
            DOVirtual.DelayedCall(holdTime, () =>
            {
                DOTween.To(() => Time.timeScale, x =>
                {
                    Time.timeScale = x;
                    Time.fixedDeltaTime = 0.02f / x;
                }, 1f, duration).OnComplete(() => { isSlowMotion = false; });
            }, ignoreTimeScale: true);
        });
    }
    public void DoKillSlowMotion()
    {
        Debug.Log("DoKillSlowMotion");
        if (!canKillTween) return;
        if (tween != null && tween.IsActive()) tween.Kill();
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
        isSlowMotion = false;
    }
}
