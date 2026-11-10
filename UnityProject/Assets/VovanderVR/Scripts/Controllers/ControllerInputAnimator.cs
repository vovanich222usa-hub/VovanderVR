using UnityEngine;
using UnityEngine.InputSystem;

namespace VovanderVR
{
    /// <summary>
    /// Дополнительно анимирует подвижные части модели контроллера.
    /// Использовать только в том случае, если выбранная модель сама не анимирует кнопки и стики.
    /// </summary>
    public sealed class ControllerInputAnimator : MonoBehaviour
    {
        [Header("Действия системы управления")]
        [SerializeField] private InputActionReference trigger;
        [SerializeField] private InputActionReference grip;
        [SerializeField] private InputActionReference thumbstick;

        [Header("Подвижные части модели")]
        [SerializeField] private Transform triggerPivot;
        [SerializeField] private Transform gripPivot;
        [SerializeField] private Transform thumbstickPivot;

        [Header("Настройки анимации")]
        [SerializeField] private Vector3 triggerPressedEuler = new Vector3(-18f, 0f, 0f);
        [SerializeField] private Vector3 gripPressedEuler = new Vector3(-12f, 0f, 0f);
        [SerializeField] private float stickTiltDegrees = 12f;
        [SerializeField, Min(1f)] private float followSpeed = 18f;

        // Запоминаем исходное положение деталей, чтобы возвращать их после отпускания кнопок.
        private Quaternion triggerRest, gripRest, stickRest;

        /// <summary>
        /// Запоминает исходный поворот триггера, рукоятки и стика на модели контроллера.
        /// </summary>
        private void Awake()
        {
            if (triggerPivot != null) triggerRest = triggerPivot.localRotation;
            if (gripPivot != null) gripRest = gripPivot.localRotation;
            if (thumbstickPivot != null) stickRest = thumbstickPivot.localRotation;
        }

        /// <summary>
        /// Включает чтение значений триггера, рукоятки и стика при запуске объекта.
        /// </summary>
        private void OnEnable()
        {
            Enable(trigger);
            Enable(grip);
            Enable(thumbstick);
        }

        /// <summary>
        /// Отключает чтение элементов управления при выключении объекта.
        /// </summary>
        private void OnDisable()
        {
            Disable(trigger);
            Disable(grip);
            Disable(thumbstick);
        }

        /// <summary>
        /// Каждый кадр получает значения нажатий и плавно двигает соответствующие детали модели.
        /// </summary>
        private void Update()
        {
            float triggerValue = ReadFloat(trigger);
            float gripValue = ReadFloat(grip);
            Vector2 stickValue = ReadVector2(thumbstick);

            Animate(triggerPivot, triggerRest, Quaternion.Euler(triggerPressedEuler * triggerValue));
            Animate(gripPivot, gripRest, Quaternion.Euler(gripPressedEuler * gripValue));

            // Наклоняем основание стика в ту же сторону, куда игрок отклоняет его пальцем.
            Quaternion stickOffset = Quaternion.Euler(
                -stickValue.y * stickTiltDegrees,
                0f,
                -stickValue.x * stickTiltDegrees);

            Animate(thumbstickPivot, stickRest, stickOffset);
        }

        /// <summary>
        /// Плавно поворачивает указанную подвижную часть к нужному положению.
        /// </summary>
        /// <param name="pivot">Деталь модели, которую нужно анимировать.</param>
        /// <param name="rest">Исходное положение детали без нажатия.</param>
        /// <param name="offset">Дополнительный поворот, зависящий от нажатия.</param>
        private void Animate(Transform pivot, Quaternion rest, Quaternion offset)
        {
            if (pivot == null)
                return;

            Quaternion target = rest * offset;
            pivot.localRotation = Quaternion.Slerp(
                pivot.localRotation,
                target,
                Time.deltaTime * followSpeed);
        }

        /// <summary>
        /// Возвращает силу нажатия кнопки или ноль, если действие ещё не назначено.
        /// </summary>
        private static float ReadFloat(InputActionReference reference)
        {
            return reference != null && reference.action != null
                ? reference.action.ReadValue<float>()
                : 0f;
        }

        /// <summary>
        /// Возвращает положение стика или нейтральное положение, если действие ещё не назначено.
        /// </summary>
        private static Vector2 ReadVector2(InputActionReference reference)
        {
            return reference != null && reference.action != null
                ? reference.action.ReadValue<Vector2>()
                : Vector2.zero;
        }

        /// <summary>
        /// Включает выбранное действие, если оно назначено в настройках Unity.
        /// </summary>
        private static void Enable(InputActionReference reference)
        {
            if (reference?.action != null)
                reference.action.Enable();
        }

        /// <summary>
        /// Отключает выбранное действие, если оно назначено в настройках Unity.
        /// </summary>
        private static void Disable(InputActionReference reference)
        {
            if (reference?.action != null)
                reference.action.Disable();
        }
    }
}
