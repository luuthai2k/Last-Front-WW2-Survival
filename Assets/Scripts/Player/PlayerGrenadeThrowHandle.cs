using DG.Tweening;
using InviGiant.Tools;
using System.Collections;
using UnityEngine;

public class PlayerGrenadeThrowHandle : MonoBehaviour
{

    public float force = 20;
    public Transform /*grenadeTransParent,*/ leftHandTarget,rightHandTarget;
    private Rigidbody grenadeRb;
    //[SerializeField] private LineRenderer LineRenderer;
    //[SerializeField][Range(10, 100)] private int LinePoints = 25;
    //[SerializeField][Range(0.01f, 0.25f)] private float TimeBetweenPoints = 0.1f;
    //[SerializeField] private Transform ReleasePosition;
    //public float side;
    Vector3 targetPoint;
    float timeToTarget;
    public void SetTargetPoint(Vector3 target)
    {
        targetPoint = target;
    }
    public void SpawmGrenade()
    {
       var direction = PlayerController.Instance.GetCoverDirection();
        if (direction == CoverDirection.Right)
        {
            grenadeRb = ResourceHelper.Instance.GetGrenade(rightHandTarget).GetComponent<Rigidbody>();
        }
        else
        {
            grenadeRb = ResourceHelper.Instance.GetGrenade(leftHandTarget).GetComponent<Rigidbody>();
        }
        grenadeRb.isKinematic = true;
        grenadeRb.transform.localPosition = Vector3.zero;
        grenadeRb.transform.localRotation = Quaternion.identity;
    }

    //public void GrenadeEquip()
    //{
    //    direction = PlayerController.Instance.GetCoverDirection();
    //    grenadeRb.transform.parent = handTarget;
    //    grenadeRb.transform.localPosition = Vector3.zero;
    //    grenadeRb.transform.localRotation = Quaternion.identity;
    //}
    public void GrenadeThow()
    {
        ReleaseGrenade(grenadeRb);
        grenadeRb = null;
        DataController.Instance.AddGrenade(-1);
        GamePlayUIManager.Instance.gamePlayMenu.UpdateGrenadeText();

    }
    //public void GrenadeCancer()
    //{
    //    grenadeRb.transform.parent = grenadeTransParent;
    //    grenadeRb.transform.localPosition = Vector3.zero;
    //    grenadeRb.transform.localRotation = Quaternion.identity;
    //}
    public void GrenadeThowEnd()
    {
        PlayerController.Instance.OnEndGrenade();

    }
    //public void Update()
    //{
    //    if (PlayerController.Instance.currentState == PlayerState.Grenade)
    //    {
    //        if (PlayerUIControl.Instance.isAim)
    //        {
    //            DrawProjection();
    //        }
    //        else
    //        {
    //            LineRenderer.enabled = false;
    //        }
    //    }
    //    else
    //    {
    //        LineRenderer.enabled = false;
    //    }
    //}
    //private void DrawProjection()
    //{
    //    LineRenderer.enabled = true;
    //    LineRenderer.positionCount = Mathf.CeilToInt(LinePoints / TimeBetweenPoints) + 1;
    //    Vector3 localPosition = Vector3.up;
    //    if (direction == CoverDirection.Left)
    //    {
    //        localPosition.x = -0.65f;
    //    }
    //    else
    //    {
    //        localPosition.x = 0.65f;

    //    }
    //    Vector3 startPosition = transform.TransformPoint(localPosition);
    //    targetPoint = CameraManager.Instance.GetAimTargetPosition();
    //    if (CameraManager.Instance.GetAimHit().transform == null)
    //    {
    //        if (Physics.Raycast(targetPoint, Vector3.down, out RaycastHit hit))
    //        {
    //            targetPoint = hit.point;
    //        }
    //    }
    //    Vector3 gravity = Physics.gravity;
    //    timeToTarget = Vector3.Distance(targetPoint,startPosition)/force;
    //    timeToTarget = Mathf.Clamp(timeToTarget, 0, 1.5f);
    //    Vector3 velocity = (targetPoint - startPosition - 0.5f * gravity * timeToTarget * timeToTarget) / timeToTarget;

    //    int i = 0;
    //    LineRenderer.SetPosition(i, startPosition);

    //    for (float time = 0; time < LinePoints; time += TimeBetweenPoints)
    //    {
    //        i++;
    //        Vector3 point = startPosition + velocity * time + 0.5f * gravity * time * time;
    //        LineRenderer.SetPosition(i, point);

    //        Vector3 lastPoint = LineRenderer.GetPosition(i - 1);

    //        if (Physics.Raycast(lastPoint, (point - lastPoint).normalized,out RaycastHit hit, (point - lastPoint).magnitude))
    //        {
    //            LineRenderer.SetPosition(i, hit.point);
    //            LineRenderer.positionCount = i + 1;
    //            targetPoint = hit.point;
    //            return;
    //        }
    //    }
    //}
    private void ReleaseGrenade(Rigidbody rb)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = false;
        rb.freezeRotation = false;
        rb.transform.SetParent(null);

        //    timeToTarget = Mathf.Clamp(timeToTarget, 0, 1.5f);
        Vector3 startPosition = rb.transform.position;
        timeToTarget = Vector3.Distance(targetPoint, startPosition) / force;
        Vector3 gravity = Physics.gravity;
        Vector3 velocity = (targetPoint - startPosition - 0.5f * gravity * timeToTarget * timeToTarget) / timeToTarget;
        rb.AddForce(velocity * rb.mass, ForceMode.Impulse);
        StartCoroutine(DelayActiveGrenade(rb.gameObject,timeToTarget));
        
    }
    IEnumerator DelayActiveGrenade(GameObject goGrenade,float timeToTarget)
    {
        yield return new WaitForSeconds(timeToTarget);
        ResourceHelper.Instance.GetEffect(EffectType.GrenadeExplosion, goGrenade.transform.position, goGrenade.transform.rotation);
        SmartPool.Instance.Despawn(goGrenade);
    }


}
