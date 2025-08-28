using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class MapControl : MonoBehaviour, IMessageHandle
{
    public RegionElement[] regionElements;
    private float[] data;
    public Material greenMaterial, blackMaterial, processMaterial;
    private void Start()
    {
        data = DataController.Instance.GetMapProcessDatas();
        SetUpLastRegion();
        MessageManager.Instance.AddSubcriber(TeeMessageType.OnSceneLoaded, this);
    }
    public void Handle(Message message)
    {
        switch (message.type)
        {
            case TeeMessageType.OnSceneLoaded:
                StartCoroutine(UpdateProcessRegion());
                break;
        }
    }
    IEnumerator UpdateProcessRegion()
    {
        yield return new WaitForSeconds(0.5f);
        data = DataController.Instance.GetMapProcessDatas();
        int lastRegion = (int)data[Mathf.Max(DataController.Instance.Level - 2, 0)];
        float lastProcess = data[Mathf.Max(DataController.Instance.Level - 2, 0)] - lastRegion;
        float lastTarget = data[Mathf.Max(DataController.Instance.Level - 1, 0)] - lastRegion;
        processMaterial.SetFloat("_LeftRegionEnd", lastProcess);
        processMaterial.SetFloat("_RightRegionStart", lastTarget);
        for (int i = 0; i < regionElements.Length; i++)
        {
            if (i < lastRegion)
            {
                regionElements[i].SetUp(greenMaterial, false, true);
            }
            else if (i == lastRegion)
            {
                regionElements[i].SetUp(processMaterial, false, true);
                MainMenuCameraController.Instance.SetTargetCam(regionElements[i].spriteRenderer.transform.position);
            }
            else
            {
                regionElements[i].SetUp(blackMaterial, true, false);
            }

        }

        int region = (int)data[DataController.Instance.Level - 1];
        float process = data[DataController.Instance.Level - 1] - region;
        float target = data[DataController.Instance.Level] - region;
        Debug.LogWarning(process);
        DOTween.To(() => processMaterial.GetFloat("_LeftRegionEnd"), x => processMaterial.SetFloat("_LeftRegionEnd", x), lastTarget, 0.5f).SetEase(Ease.OutSine)
            .OnComplete(() =>
            {
                processMaterial.SetFloat("_LeftRegionEnd", process);
                processMaterial.SetFloat("_RightRegionStart", process);
                DOTween.To(() => processMaterial.GetFloat("_RightRegionStart"), x => processMaterial.SetFloat("_RightRegionStart", x), target, 0.5f).SetEase(Ease.OutSine);
                for (int i = 0; i < regionElements.Length; i++)
                {
                    if (i < region)
                    {
                        regionElements[i].SetUp(greenMaterial, false, true);
                    }
                    else if (i == region)
                    {
                        regionElements[i].SetUp(processMaterial, false, true);
                        MainMenuCameraController.Instance.SetTargetCam(regionElements[i].spriteRenderer.transform.position, 0.5f);
                    }
                    else
                    {
                        regionElements[i].SetUp(blackMaterial, true, false);
                    }

                }
            });

    }
    public void SetUpLastRegion()
    {
        int lastRegion = (int)data[Mathf.Max(DataController.Instance.Level - 2, 0)];
        float lastProcess = data[Mathf.Max(DataController.Instance.Level - 2, 0)] - lastRegion;
        float lastTarget = data[Mathf.Max(DataController.Instance.Level - 1, 0)] - lastRegion;
        processMaterial.SetFloat("_LeftRegionEnd", lastProcess);
        processMaterial.SetFloat("_RightRegionStart", lastTarget);
        for (int i = 0; i < regionElements.Length; i++)
        {
            if (i < lastRegion)
            {
                regionElements[i].SetUp(greenMaterial, false, true);
            }
            else if (i == lastRegion)
            {
                regionElements[i].SetUp(processMaterial, false, true);
                MainMenuCameraController.Instance.SetTargetCam(regionElements[i].spriteRenderer.transform.position);
            }
            else
            {
                regionElements[i].SetUp(blackMaterial, true, false);
            }

        }
    }
    void OnDestroy()
    {
        MessageManager.Instance.RemoveSubcriber(TeeMessageType.OnSceneLoaded, this);
    }
}
