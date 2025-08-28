using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
public class EnemyLK : MonoBehaviour
{
    public BaseEnemyController enemyController;
    public Transform leftHandIKTarget;
    public Transform chestBone, upperChestBone;
    [SerializeField] public float armsRotation = 0f;
    [Range(0, 1f)]
    public float leftHandIKWeight = 1f;
    [SerializeField]
    public Animator animator;
   private Quaternion targetRotation;
    private void Start()
    {
        upperChestBone = animator.GetBoneTransform(HumanBodyBones.UpperChest);
        chestBone = animator.GetBoneTransform(HumanBodyBones.Chest);
    }
    private void OnAnimatorIK(int layerIndex)
    {
        //if (enemyController.currentState == EnemyState.Dead) return;
        //if (leftHandIKTarget != null)
        //{
        //    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandIKWeight);
        //    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandIKWeight);
        //    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIKTarget.position);
        //    animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIKTarget.rotation);
        //}
        if (enemyController.currentState == EnemyState.Move|| enemyController.currentState == EnemyState.Dead) return;
        if (enemyController.patrolPoint.coverType == CoverType.None||enemyController.currentState == EnemyState.Shoot)
        {
            Vector3 direction = PlayerController.Instance.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            targetRotation = Quaternion.AngleAxis(lookRotation.eulerAngles.x + armsRotation, this.transform.right);
            Quaternion slerpRot = Quaternion.Slerp(upperChestBone.localRotation, Quaternion.Inverse(chestBone.rotation) * targetRotation * chestBone.rotation, 40 * Time.deltaTime);
            animator.SetBoneLocalRotation(HumanBodyBones.UpperChest, slerpRot);
        }
    }
    public void SetTargetRotation(float xRot)
    {
        targetRotation = Quaternion.AngleAxis(xRot + armsRotation, this.transform.right);
    }
}
