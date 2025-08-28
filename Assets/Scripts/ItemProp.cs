using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class ItemProp : MonoBehaviour
{
    [SerializeField] private Image icon;
    public RectTransform imageRectTrans;
    public AnimationCurve animationCurve;
    public Vector3 destination, start;
    private Vector2 targetSizeDelta;
    private float duration;
    private UnityAction CallBack;

    [SerializeField] private float delay;
    bool moveRandomPost = true;
    public void MoveTween(Sprite sprite,Vector3 Target, Vector3 startPos, Vector2 startSizeDelta, Vector2 targetSizeDelta, float duration, float delayTime, UnityAction CallBack = null, bool moveRandomPost = true)
    {
        icon.sprite = sprite;
        transform.position = startPos;
        start = startPos;
        destination = Target;
        this.duration = duration;
        this.CallBack = CallBack;
        this.delay = delayTime;
        this.moveRandomPost = moveRandomPost;
        this.targetSizeDelta = targetSizeDelta;
        imageRectTrans.sizeDelta = startSizeDelta;
        StartCoroutine(CoroutineMove());
    }
    public void MoveTween(Vector3 Target, Vector3 startPos, Vector2 startSizeDelta, Vector2 targetSizeDelta, float duration, float delayTime, UnityAction CallBack = null, bool moveRandomPost = true)
    {
        transform.position = startPos;
        start = startPos;
        destination = Target;
        this.duration = duration;
        this.CallBack = CallBack;
        this.delay = delayTime;
        this.moveRandomPost = moveRandomPost;
        this.targetSizeDelta = targetSizeDelta;
        imageRectTrans.sizeDelta = startSizeDelta;
        StartCoroutine(CoroutineMove());
    }
    IEnumerator CoroutineMove()
    {
        if (moveRandomPost)
        {
            //imageRectTrans.DOSizeDelta(targetSizeDelta, 0.25f).SetEase(Ease.InQuad);
            transform.DOPunchScale(Vector3.one*0.5f, 0.35f,1,1).SetEase(Ease.InQuad);
            transform.DOMove(start + new Vector3(Random.Range(-150f, 150f), Random.Range(-150f, 150f), 0), 0.25f).SetEase(Ease.OutQuad);
            yield return new WaitForSeconds(0.35f);
        }
        float time = 0f;
        float g = 0f;
        start = transform.position;
        yield return new WaitForSeconds(delay);
        imageRectTrans.DOSizeDelta(targetSizeDelta, duration).SetEase(Ease.OutQuad);
        //transform.DOScale(1, duration).SetEase(Ease.InQuad);
        while (time < duration)
        {
            time += Time.deltaTime * g;
            g += 0.12f;
            float normalizedTimeOnCurve = time / duration;
            float yValueOfCurve = animationCurve.Evaluate(normalizedTimeOnCurve);
            transform.position = Vector2.Lerp(start, destination, normalizedTimeOnCurve) + new Vector2(yValueOfCurve, 0f);
            yield return null;
        }
        CallBack?.Invoke();
    }
}
