using UnityEngine;
using UnityEngine.InputSystem;

namespace VovanderVR
{
    /// <summary>
    /// Управляет перемещением игрока левым стиком:
    /// влево и вправо — поворот, вверх и вниз — движение вперёд и назад.
    /// </summary>
    public sealed class VovanderLocomotion : MonoBehaviour
    {
        [Header("Управление")]
        [SerializeField] private InputActionReference leftStick;

        [Header("Скорость движения")]
        [SerializeField, Min(0f)] private float moveSpeed = 2.0f;
        [SerializeField, Min(0f)] private float turnSpeed = 90.0f;
        [SerializeField, Range(0f, 0.5f)] private float deadZone = 0.15f;
        [SerializeField] private Transform head;

        /// <summary>
        /// Включает чтение левого стика, когда объект игрока становится активным.
        /// </summary>
        private void OnEnable()
        {
            if (leftStick != null && leftStick.action != null)
                leftStick.action.Enable();
        }

        /// <summary>
        /// Отключает чтение стика, когда объект игрока выключается.
        /// Это предотвращает лишнюю обработку ввода в неактивной сцене.
        /// </summary>
        private void OnDisable()
        {
            if (leftStick != null && leftStick.action != null)
                leftStick.action.Disable();
        }

        /// <summary>
        /// Каждый кадр считывает положение левого стика и применяет поворот или движение.
        /// Небольшие случайные отклонения стика игнорируются с помощью мёртвой зоны.
        /// </summary>
        private void Update()
        {
            if (leftStick == null || leftStick.action == null)
                return;

            Vector2 stick = leftStick.action.ReadValue<Vector2>();
            float turn = Mathf.Abs(stick.x) < deadZone ? 0f : stick.x;
            float forward = Mathf.Abs(stick.y) < deadZone ? 0f : stick.y;

            // Горизонтальная ось стика поворачивает игрока вокруг вертикальной оси.
            transform.Rotate(0f, turn * turnSpeed * Time.deltaTime, 0f, Space.World);

            // Движение идёт туда, куда смотрит голова, но без подъёма и опускания камеры.
            Vector3 forwardDirection = head != null
                ? Vector3.ProjectOnPlane(head.forward, Vector3.up).normalized
                : transform.forward;

            if (forwardDirection.sqrMagnitude < 0.001f)
                forwardDirection = transform.forward;

            transform.position += forwardDirection * (forward * moveSpeed * Time.deltaTime);
        }
    }
}
