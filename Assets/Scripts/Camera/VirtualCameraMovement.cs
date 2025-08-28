using UnityEngine;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;
using Lean.Common;

namespace Lean.Touch
{
    [HelpURL(LeanTouch.HelpUrlPrefix + "Virtual Camera Movement")]
    [AddComponentMenu(LeanTouch.ComponentPathPrefix + "Virtual Camera Movement")]
    public class VirtualCameraMovement : MonoBehaviour
    {
        public Transform virtualCameraTarget;
        public LeanFingerFilter Use = new LeanFingerFilter(true);
        public Camera Camera { set { _camera = value; } get { return _camera; } }
        [FSA("Camera")][SerializeField] private Camera _camera;
        public float Sensitivity { set { sensitivity = value; } get { return sensitivity; } }
        [FSA("Sensitivity")][SerializeField] private float sensitivity = 1.0f;
        private Vector3 remainingTranslation;
        public void AddFinger(LeanFinger finger)
        {
            Use.AddFinger(finger);
        }
        public void RemoveFinger(LeanFinger finger)
        {
            Use.RemoveFinger(finger);
        }
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
            Use.UpdateRequiredSelectable(gameObject);
        }
        protected virtual void Update()
        {
            var fingers = Use.UpdateAndGetFingers();
            var screenDelta = LeanGesture.GetScreenDelta(fingers);
            if (screenDelta != Vector2.zero)
            {
                if (transform is RectTransform)
                {
                    TranslateUI(screenDelta);
                }
                else
                {
                    if (m_BoundingShapeTarget2D.OverlapPoint((Vector2)virtualCameraTarget.position))
                        Translate(screenDelta);
                }
            }
            if (fingers.Count == 0)
            {
                if (!m_BoundingShapeTarget2D.OverlapPoint((Vector2)virtualCameraTarget.position))
                {
                    var closestPoint = m_BoundingShape2D.ClosestPoint((Vector2)virtualCameraTarget.position);
                    Vector3 target = Vector3.Slerp(virtualCameraTarget.position, closestPoint, 50 * Time.deltaTime);
                    virtualCameraTarget.position = target;
                }
            }
        }
        [Tooltip("The 2D shape within which the camera is to be contained")]
        public Collider2D m_BoundingShape2D;
        [Tooltip("The 2D shape within which the camera target is to be contained")]
        public Collider2D m_BoundingShapeTarget2D;
        private void TranslateUI(Vector2 screenDelta)
        {
            var camera = this._camera;

            if (camera == null)
            {
                var canvas = transform.GetComponentInParent<Canvas>();

                if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
                {
                    camera = canvas.worldCamera;
                }
            }

            var screenPoint = RectTransformUtility.WorldToScreenPoint(camera, virtualCameraTarget.position);
            screenPoint.x += screenDelta.x * Sensitivity;
            screenPoint.y += screenDelta.y * Sensitivity;
            var worldPoint = default(Vector3);
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(virtualCameraTarget.parent as RectTransform, screenPoint, camera, out worldPoint) == true)
            {
                virtualCameraTarget.position = worldPoint;
            }
        }

        private void Translate(Vector2 screenDelta)
        {
            var camera = LeanHelper.GetCamera(this._camera, virtualCameraTarget.gameObject);
            if (camera != null)
            {
                var screenPoint = camera.WorldToScreenPoint(virtualCameraTarget.position);
                screenPoint += (Vector3)screenDelta * Sensitivity;
                Vector3 target = Vector3.Slerp(virtualCameraTarget.position, camera.ScreenToWorldPoint(screenPoint), 50 * Time.deltaTime);
                target.z = 0;
                virtualCameraTarget.position = target;
            }
            else
            {
                Debug.LogError("Failed to find camera. Either tag your camera as MainCamera, or set one in this component.", this);
            }
        }
    }
}

