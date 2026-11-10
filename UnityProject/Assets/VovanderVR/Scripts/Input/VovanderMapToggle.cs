using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace VovanderVR
{
    /// <summary>Bind toggleMap to the right controller B / secondaryButton action.</summary>
    public sealed class VovanderMapToggle : MonoBehaviour
    {
        [SerializeField] private InputActionReference toggleMap;
        [SerializeField] private GameObject mapPanel;
        [SerializeField] private UnityEvent<bool> onMapVisibilityChanged;

        private void OnEnable()
        {
            if (toggleMap != null && toggleMap.action != null)
            {
                toggleMap.action.performed += OnToggle;
                toggleMap.action.Enable();
            }
        }

        private void OnDisable()
        {
            if (toggleMap != null && toggleMap.action != null)
            {
                toggleMap.action.performed -= OnToggle;
                toggleMap.action.Disable();
            }
        }

        private void OnToggle(InputAction.CallbackContext context)
        {
            bool visible = mapPanel != null && !mapPanel.activeSelf;
            if (mapPanel != null) mapPanel.SetActive(visible);
            onMapVisibilityChanged?.Invoke(visible);
        }
    }
}
