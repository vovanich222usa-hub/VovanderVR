using UnityEngine;
using UnityEngine.InputSystem;

namespace VovanderVR
{
    /// <summary>
    /// Prototype locomotion: LEFT stick X rotates the player; LEFT stick Y moves forward/back.
    /// Assign a Vector2 action bound to <XRController>{LeftHand}/primary2DAxis.
    /// </summary>
    public sealed class VovanderLocomotion : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputActionReference leftStick;

        [Header("Movement")]
        [SerializeField, Min(0f)] private float moveSpeed = 2.0f;
        [SerializeField, Min(0f)] private float turnSpeed = 90.0f;
        [SerializeField, Range(0f, 0.5f)] private float deadZone = 0.15f;
        [SerializeField] private Transform head;

        private void OnEnable()
        {
            if (leftStick != null && leftStick.action != null) leftStick.action.Enable();
        }

        private void OnDisable()
        {
            if (leftStick != null && leftStick.action != null) leftStick.action.Disable();
        }

        private void Update()
        {
            if (leftStick == null || leftStick.action == null) return;

            Vector2 stick = leftStick.action.ReadValue<Vector2>();
            float turn = Mathf.Abs(stick.x) < deadZone ? 0f : stick.x;
            float forward = Mathf.Abs(stick.y) < deadZone ? 0f : stick.y;

            transform.Rotate(0f, turn * turnSpeed * Time.deltaTime, 0f, Space.World);

            Vector3 forwardDirection = head != null ? Vector3.ProjectOnPlane(head.forward, Vector3.up).normalized : transform.forward;
            if (forwardDirection.sqrMagnitude < 0.001f) forwardDirection = transform.forward;
            transform.position += forwardDirection * (forward * moveSpeed * Time.deltaTime);
        }
    }
}
