using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Common;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;
using Lean.Touch;
[RequireComponent(typeof(Cinemachine.CinemachineVirtualCamera))]
public class VirtualCameraPinchScale : MonoBehaviour
{
    /// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
    public LeanFingerFilter Use = new LeanFingerFilter(true);
    /// <summary>The camera that will be used to calculate the zoom.
    /// None/null = MainCamera.</summary>
    public Camera Camera { set { _camera = value; } get { return _camera; } }
    [FSA("Camera")] [SerializeField] private Camera _camera;
    /// <summary>The sensitivity of the scaling.
    /// 1 = Default.
    /// 2 = Double.</summary>
    public float Sensitivity { set { sensitivity = value; } get { return sensitivity; } }
    [FSA("Sensitivity")] [SerializeField] private float sensitivity = 1.0f;
    /// <summary>If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.
    /// -1 = Instantly change.
    /// 1 = Slowly change.
    /// 10 = Quickly change.</summary>
    public float Damping { set { damping = value; } get { return damping; } }
    [FSA("Damping")] [FSA("Dampening")] [SerializeField] private float damping = 1.0f;
    private Cinemachine.CinemachineVirtualCamera virtualCamera;
    private float originCameraSize = 10f, targetCameraSize = 10, timeStamp;
    /// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually add a finger.</summary>
    public void AddFinger(LeanFinger finger)
    {
        Use.AddFinger(finger);
    }

    /// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove a finger.</summary>
    public void RemoveFinger(LeanFinger finger)
    {
        Use.RemoveFinger(finger);
    }

    /// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove all fingers.</summary>
    public void RemoveAllFingers()
    {
        Use.RemoveAllFingers();
    }

#if UNITY_EDITOR
    protected virtual void Reset()
    {
        Use.UpdateRequiredSelectable(gameObject);
    }
#endif

    protected virtual void Awake()
    {
        virtualCamera = GetComponent<Cinemachine.CinemachineVirtualCamera>();
        targetCameraSize = originCameraSize = virtualCamera.m_Lens.OrthographicSize;
        Use.UpdateRequiredSelectable(gameObject);
    }
    private int lastFingerCount = 0;
    protected virtual void Update()
    {
        // Get the fingers we want to use
        var fingers = Use.UpdateAndGetFingers();
        // Calculate pinch scale, and make sure it's valid
        var pinchScale = LeanGesture.GetPinchScale(fingers);
        if (fingers.Count == 2)
        {
            if (pinchScale != 1.0f)
            {
                pinchScale = Mathf.Pow(pinchScale, sensitivity);
                targetCameraSize = Mathf.Clamp(targetCameraSize * pinchScale, originCameraSize * 0.5f, originCameraSize * 1.75f);
                virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, targetCameraSize, 0.25f);
                timeStamp = Time.time;
            }
            else
            if ((Time.time - timeStamp) * damping < 1)
                virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, targetCameraSize, (Time.time - timeStamp) * damping);
        }
        else
        {
            if (lastFingerCount == 2)
            {
                timeStamp = Time.time;
                targetCameraSize = Mathf.Clamp(targetCameraSize, originCameraSize * 0.6f, originCameraSize * 1.4f);
            }
            if ((Time.time - timeStamp) * damping < 1)
                virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, targetCameraSize, (Time.time - timeStamp) * damping);
        }
        lastFingerCount = fingers.Count;
    }
}
#if UNITY_EDITOR
namespace Lean.Touch.Editor
{
    using TARGET = VirtualCameraPinchScale;

    [UnityEditor.CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(TARGET), true)]
    public class VirtualCameraPinchScale_Editor : LeanEditor
    {
        protected override void OnInspector()
        {
            TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

            Draw("Use");
            Draw("_camera", "The camera that will be used to calculate the zoom.\n\nNone/null = MainCamera.");
            Draw("sensitivity", "The sensitivity of the scaling.\n\n1 = Default.\n\n2 = Double.");
            Draw("damping", "If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.\n\n-1 = Instantly change.\n\n1 = Slowly change.\n\n10 = Quickly change.");
        }
    }
}
#endif
