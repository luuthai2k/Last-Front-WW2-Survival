using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelBoneRefrences : MonoBehaviour
{
    public PlayerIK playerIK;
    public Transform  rightHandPivot, leftHandPivot;
    [Range(0f, 1f)]
    public float rightHandWeight = 1f;
    [Range(0f, 1f)]
    public float leftHandWeight = 1f;
    private void LateUpdate()
    {
        if (playerIK == null) return;
        if (rightHandPivot != null && playerIK.rightHandBone != null)
        {
            rightHandPivot.position = Vector3.Lerp(rightHandPivot.position, playerIK.rightHandBone.position, rightHandWeight);
            rightHandPivot.rotation = Quaternion.Slerp(rightHandPivot.rotation, playerIK.rightHandBone.rotation, rightHandWeight);
        }

        if (leftHandPivot != null && playerIK.leftHandBone != null)
        {
            leftHandPivot.position = Vector3.Lerp(leftHandPivot.position, playerIK.leftHandBone.position, leftHandWeight);
            leftHandPivot.rotation = Quaternion.Slerp(leftHandPivot.rotation, playerIK.leftHandBone.rotation, leftHandWeight);
        }
    }
    public void RightHandWeight(float weight)
    {
        rightHandWeight = weight;
    }
    public void LeftHandWeight(float weight)
    {
        leftHandWeight = weight;
    }
    public void RightHandWeight(float weight,float duration)
    {
        DOTween.To(() => rightHandWeight, x => rightHandWeight = x, weight, duration);
    }
    public void LeftHandWeight(float weight, float duration)
    {
        DOTween.To(() => leftHandWeight, x => leftHandWeight = x, weight, duration);
    }
}
