using DG.Tweening;
using System;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
public class PlayerIK : MonoBehaviour
{
    public Transform leftHandIKTarget;
    public Transform leftHandBone, rightHandBone, chestBone,upperChestBone;
    [SerializeField]  private float armsRotation = 0f;
    [Range(0, 1f)]
    public float leftHandIKWeight = 1f, upperChestIKWeight=0;
    [SerializeField]
    public Animator animator;
    Tween leftHandIKWeightTween,upperChestIKWeightTween;
    private void Start()
    {
        leftHandBone = animator.GetBoneTransform(HumanBodyBones.LeftHand);
        rightHandBone = animator.GetBoneTransform(HumanBodyBones.RightHand);
        upperChestBone = animator.GetBoneTransform(HumanBodyBones.UpperChest);  
        chestBone = animator.GetBoneTransform(HumanBodyBones.Chest);
    }
    public void SetLK(Transform leftHandIKTarget)
    {
        this.leftHandIKTarget = leftHandIKTarget;
    }
    public void LeftHandIKWeight(float weight, float duration)
    {
        if (leftHandIKWeightTween != null)
        {
            leftHandIKWeightTween.Kill();
        }
        leftHandIKWeightTween =DOTween.To(() => leftHandIKWeight, x => leftHandIKWeight = x, weight, duration);
    }
    public void UpperChestIKWeight(float weight, float duration)
    {
        if (upperChestIKWeightTween != null)
        {
            upperChestIKWeightTween.Kill();
        }
        upperChestIKWeightTween=DOTween.To(() => upperChestIKWeight, x => upperChestIKWeight = x, weight, duration);
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (layerIndex != 0) return;
        if (PlayerController.Instance.currentweaponType == WeaponType.Machinegun) return;

        if (leftHandIKTarget != null)
        {
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandIKWeight);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandIKWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIKTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIKTarget.rotation);
        }
        if (PlayerController.Instance.currentState == PlayerState.Move|| PlayerController.Instance.currentState == PlayerState.Ready) return;
        if (upperChestBone != null)
        {
            float xCamRot = CameraManager.Instance.GetTargetRotation().eulerAngles.x;
            if (PlayerController.Instance.GetCoverType() == CoverType.LowCover)
            {
                armsRotation = 3f;
            }
            else
            {
                armsRotation = 3.75f;
            }
            Quaternion targetRot = Quaternion.AngleAxis(xCamRot + armsRotation, this.transform.right);
            targetRot *= chestBone.rotation;
            Quaternion slerpRot = Quaternion.Slerp(upperChestBone.localRotation, Quaternion.Inverse(chestBone.rotation) * targetRot, upperChestIKWeight);
            animator.SetBoneLocalRotation(HumanBodyBones.UpperChest, slerpRot);
        }

    }


}
