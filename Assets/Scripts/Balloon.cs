using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Balloon : MonoBehaviour, ITakeDameBullet
{
    public GameObject goBreakFx, goBalloon;
    public Collider collider;
    public UnityEvent onBreak;
    bool isBreak;
    public void TakeDamageBullet(Vector3 location, Vector3 normal, Vector3 direction, int damage, int maxDamage, out int damageRemain)
    {
        damageRemain = 0;
        Break();
    }
    public void Break()
    {
        if (isBreak) return;
        isBreak = true;
        goBreakFx.SetActive(true);
        goBalloon.SetActive(false);
        collider.enabled = false;
        onBreak?.Invoke();
    }
    public void FlyUp()
    {
        if (isBreak) return;
        transform.DOLocalMoveY(10, 3).SetEase(Ease.Linear).OnComplete(() =>
        {
            Break();
        });
    }
}
