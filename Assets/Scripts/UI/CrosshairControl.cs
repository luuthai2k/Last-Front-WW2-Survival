using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairControl : Singleton<CrosshairControl>
{
    [SerializeField] private CanvasGroup hitCrosshairImg;
    [SerializeField] private Sprite crosshair, shootCrosshair;
    public Crosshair[] crosshairs;
    private Crosshair currentCrosshair;
    [SerializeField]  private Image crosshairTopLineImg, crosshairBotLineImg, crosshairLeftLineImg, crosshairRightLineImg;
   Coroutine coroutineOnShoot, coroutineDeactiveHit;

    public void Start()
    {
        currentCrosshair = crosshairs[0];
    }
    private void OnEnable()
    {
        hitCrosshairImg.alpha = 0;
      
    }
   public void ChangeCrosshair(int index)
    {
        if (currentCrosshair != null)
        {
            currentCrosshair.gameObject.SetActive(false);
        }
        currentCrosshair = crosshairs[index];
        currentCrosshair.gameObject.SetActive(true);
    }
    public void ToggleCrossHair(bool active)
    {
        currentCrosshair.gameObject.SetActive(active);
    }
    public void OnShoot()
    {
        if (!gameObject.activeSelf) return;
        currentCrosshair.Shoot();
     
    }
   
    public void GetHitCrosshair()
    {
        if (!gameObject.activeInHierarchy) return;
        if (coroutineDeactiveHit != null)
        {
            StopCoroutine(coroutineDeactiveHit);
        }
        hitCrosshairImg.DOKill();
        hitCrosshairImg.alpha = 1;
        coroutineDeactiveHit = StartCoroutine(CoroutineDeactiveHit());
    }
    IEnumerator CoroutineDeactiveHit()
    {
        yield return new WaitForSeconds(0.5f);
        hitCrosshairImg.DOFade(0, 0.5f);

    }
   
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
