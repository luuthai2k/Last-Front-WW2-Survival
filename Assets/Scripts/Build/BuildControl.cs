using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildControl : MonoBehaviour
{
    public BuildStep[] steps;
    BuildProcessInGameData processInGameData;
    private float currentFillAmount;
  
    public void UpdateProcess()
    {
        if (processInGameData == null)
        {
            processInGameData = DataController.Instance.GetBuildProcessInGameData();
        }
        for (int i = 0; i < steps.Length; i++)
        {
            if (i < processInGameData.indexInData)
            {
                steps[i].OnComplete();
            }
           else if (i == processInGameData.indexInData)
            {
                currentFillAmount = (float)processInGameData.process / processInGameData.cost;
                steps[i].UpdateProcess(currentFillAmount);
            }
            else
            {
                steps[i].UpdateProcess(0);
            }
        }
    }
    public void OnComplete()
    {
        for (int i = 0; i < steps.Length; i++)
        {
            steps[i].OnComplete();
        }
    }
   public void UpdateFill(float duration)
    {
       
        DOVirtual.Float(currentFillAmount, (float)processInGameData.process / processInGameData.cost, duration, (x) =>
        {
            currentFillAmount = x;
            steps[processInGameData.indexInData].UpdateProcess(currentFillAmount);

        }).SetEase(Ease.Linear);
    }
    public void OnFilled()
    {
        currentFillAmount = 0;
    }
    public Vector3  GetSnapTargetPoint()
    {
        if (processInGameData == null)
        {
            processInGameData = DataController.Instance.GetBuildProcessInGameData();
        }
        return steps[processInGameData.indexInData].snapTarget.position;
    }
}
