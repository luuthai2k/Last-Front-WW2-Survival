using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
public class CurveMove : MonoBehaviour
{
    public AnimationCurve animationCurve;
    public Vector3 destination;
    private Vector3 start;
    private float duration;
    private UnityAction CallBack;

    [SerializeField] private float Delay;
    bool moveRandomPost = true;

    
    public void MoveTween(Vector3 Target, Vector3 startPos, float duration, float delayTime, UnityAction CallBack = null, bool moveRandomPost = true)
    {
        transform.position = startPos;
        start = startPos;
        destination = Target;
        this.duration = duration;
        this.CallBack = CallBack;
        this.Delay = delayTime;
        this.moveRandomPost = moveRandomPost;
        StartCoroutine(CoroutineMove());
    }
    IEnumerator CoroutineMove()
    {
        if (moveRandomPost)
        {
            Vector3 CurrentScale = transform.localScale;
            transform.localScale = Vector3.zero;
            transform.DOScale(CurrentScale * 1.2f, 0.2f).OnComplete(
                () =>
                {
                    transform.DOScale(CurrentScale, 0.1f);
                }
            );
            transform.DOMove(start + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1f, 1f), 0), 0.2f);
        }
        yield return new WaitForSeconds(Delay);
        float time = 0f;
        float g = 0f;
        start = transform.position;
        Vector2 end = destination;
        Vector2 dir = Vector2.zero;
        dir.x = end.x - start.x;
        dir.y = end.y - start.y;
        Vector3 tmpPos = new Vector3(start.x - 0.02f * dir.normalized.x, start.y - 0.02f * dir.normalized.y, 0);
        transform.DOMove(tmpPos, 0.2f);
        yield return new WaitForSeconds(0.2f);
        while (time < duration)
        {
            time += Time.deltaTime * g;
            g += 0.12f;
            float normalizedTimeOnCurve = time / duration;
            float yValueOfCurve = animationCurve.Evaluate(normalizedTimeOnCurve);

            transform.position = Vector2.Lerp(start, end, normalizedTimeOnCurve) + new Vector2(yValueOfCurve, 0f);
            yield return null;
        }
        CallBack?.Invoke();
    }
}
