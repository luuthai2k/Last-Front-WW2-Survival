using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
public class EnemyFlameThrowerLK : MonoBehaviour
{
    [SerializeField]
    public Animator animator;
    public FlameThrowerEnemyController enemyController;
    private void OnAnimatorIK(int layerIndex)
    {
        if (enemyController.currentState == EnemyState.Shoot)
        {
            Transform spineTransform = animator.GetBoneTransform(HumanBodyBones.Spine);
            Quaternion initialSpineLocalRotation = spineTransform.localRotation;

            float t = enemyController.timerToShoot / 4f;

            float angle = Mathf.Sin(t * Mathf.PI * 2) * 25;

            Quaternion rotationOffset = Quaternion.AngleAxis(angle, Vector3.up);

            Quaternion finalRotation = initialSpineLocalRotation * rotationOffset;
            animator.SetBoneLocalRotation(HumanBodyBones.Spine, finalRotation);
        }


    }
}
