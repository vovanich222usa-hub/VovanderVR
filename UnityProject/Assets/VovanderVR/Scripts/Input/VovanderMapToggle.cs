using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace VovanderVR
{
    /// <summary>
    /// Показывает или скрывает виртуальную карту по нажатию назначенной кнопки.
    /// По умолчанию сюда назначается кнопка B правого контроллера.
    /// </summary>
    public sealed class VovanderMapToggle : MonoBehaviour
    {
        [Header("Кнопка открытия карты")]
        [SerializeField] private InputActionReference toggleMap;

        [Header("Окно карты")]
        [SerializeField] private GameObject mapPanel;

        [Header("Сообщение об изменении видимости")]
        [SerializeField] private UnityEvent<bool> onMapVisibilityChanged;

        /// <summary>
        /// Подписывается на нажатие кнопки и включает обработку действия.
        /// </summary>
        private void OnEnable()
        {
            if (toggleMap != null && toggleMap.action != null)
            {
                toggleMap.action.performed += OnToggle;
                toggleMap.action.Enable();
            }
        }

        /// <summary>
        /// Отписывается от нажатия кнопки, чтобы событие не вызывалось после выключения объекта.
        /// </summary>
        private void OnDisable()
        {
            if (toggleMap != null && toggleMap.action != null)
            {
                toggleMap.action.performed -= OnToggle;
                toggleMap.action.Disable();
            }
        }

        /// <summary>
        /// Меняет видимость карты на противоположную и сообщает об этом другим элементам интерфейса.
        /// </summary>
        /// <param name="context">Сведения о нажатии кнопки, переданные системой управления Unity.</param>
        private void OnToggle(InputAction.CallbackContext context)
        {
            bool visible = mapPanel != null && !mapPanel.activeSelf;

            if (mapPanel != null)
                mapPanel.SetActive(visible);

            onMapVisibilityChanged?.Invoke(visible);
        }
    }
}
