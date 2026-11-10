using UnityEngine;
using UnityEngine.InputSystem;

namespace VovanderVR
{
    /// <summary>
    /// Drives optional moving parts on the Meta runtime controller model.
    /// Assign transforms for trigger, grip, and thumbstick pivots from the model hierarchy.
    /// This complements the runtime model; it does not replace Meta's model loader.
    /// </summary>
    public sealed class ControllerInputAnimator : MonoBehaviour
    {
        [Header("Input actions")]
        [SerializeField] private InputActionReference trigger;
        [SerializeField] private InputActionReference grip;
        [SerializeField] private InputActionReference thumbstick;

        [Header("Model pivots")]
        [SerializeField] private Transform triggerPivot;
        [SerializeField] private Transform gripPivot;
        [SerializeField] private Transform thumbstickPivot;

        [Header("Animation tuning")]
        [SerializeField] private Vector3 triggerPressedEuler = new Vector3(-18f, 0f, 0f);
        [SerializeField] private Vector3 gripPressedEuler = new Vector3(-12f, 0f, 0f);
        [SerializeField] private float stickTiltDegrees = 12f;
        [SerializeField, Min(1f)] private float followSpeed = 18f;

        private Quaternion triggerRest, gripRest, stickRest;

        private void Awake()
        {
            if (triggerPivot != null) triggerRest = triggerPivot.localRotation;
            if (gripPivot != null) gripRest = gripPivot.localRotation;
            if (thumbstickPivot != null) stickRest = thumbstickPivot.localRotation;
        }

        private void OnEnable()
        {
            Enable(trigger); Enable(grip); Enable(thumbstick);
        }

        private void OnDisable()
        {
            Disable(trigger); Disable(grip); Disable(thumbstick);
        }

        private void Update()
        {
            float triggerValue = ReadFloat(trigger);
            float gripValue = ReadFloat(grip);
            Vector2 stickValue = ReadVector2(thumbstick);

            Animate(triggerPivot, triggerRest, Quaternion.Euler(triggerPressedEuler * triggerValue));
            Animate(gripPivot, gripRest, Quaternion.Euler(gripPressedEuler * gripValue));
            Quaternion stickOffset = Quaternion.Euler(-stickValue.y * stickTiltDegrees, 0f, -stickValue.x * stickTiltDegrees);
            Animate(thumbstickPivot, stickRest, stickOffset);
        }

        private void Animate(Transform pivot, Quaternion rest, Quaternion offset)
        {
            if (pivot == null) return;
            Quaternion target = rest * offset;
            pivot.localRotation = Quaternion.Slerp(pivot.localRotation, target, Time.deltaTime * followSpeed);
        }

        private static float ReadFloat(InputActionReference reference) =>
            reference != null && reference.action != null ? reference.action.ReadValue<float>() : 0f;

        private static Vector2 ReadVector2(InputActionReference reference) =>
            reference != null && reference.action != null ? reference.action.ReadValue<Vector2>() : Vector2.zero;

        private static void Enable(InputActionReference reference) { if (reference?.action != null) reference.action.Enable(); }
        private static void Disable(InputActionReference reference) { if (reference?.action != null) reference.action.Disable(); }
    }
}
