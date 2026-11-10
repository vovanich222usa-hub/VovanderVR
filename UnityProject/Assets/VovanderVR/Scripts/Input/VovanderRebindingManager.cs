using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VovanderVR
{
    /// <summary>
    /// Runtime rebinding prototype. Assign every rebindable action in actions.
    /// A physical control path can only be assigned to one action in this list.
    /// Binding overrides are persisted locally with PlayerPrefs.
    /// </summary>
    public sealed class VovanderRebindingManager : MonoBehaviour
    {
        [SerializeField] private List<InputActionReference> actions = new List<InputActionReference>();
        private const string OverridesKey = "VovanderVR.InputOverrides.v1";
        private InputActionRebindingExtensions.RebindingOperation activeOperation;

        private void Awake()
        {
            foreach (InputActionReference reference in actions)
                if (reference != null && reference.action != null)
                    reference.action.LoadBindingOverridesFromJson(PlayerPrefs.GetString(OverridesKey, "{}"));
        }

        public void BeginRebind(int actionListIndex, int bindingIndex)
        {
            if (actionListIndex < 0 || actionListIndex >= actions.Count) return;
            InputAction action = actions[actionListIndex] != null ? actions[actionListIndex].action : null;
            if (action == null || bindingIndex < 0 || bindingIndex >= action.bindings.Count) return;

            activeOperation?.Cancel();
            activeOperation?.Dispose();

            action.Disable();
            activeOperation = action.PerformInteractiveRebinding(bindingIndex)
                .WithCancelingThrough("<Keyboard>/escape")
                .OnComplete(operation =>
                {
                    string proposedPath = operation.selectedControl != null ? operation.selectedControl.path : null;
                    operation.Dispose();
                    activeOperation = null;

                    if (string.IsNullOrEmpty(proposedPath) || IsUsedByAnotherAction(action, bindingIndex, proposedPath))
                    {
                        action.RemoveBindingOverride(bindingIndex);
                        Debug.LogWarning("Vovander VR: this control is already assigned. Choose a unique control.");
                    }
                    else
                    {
                        action.ApplyBindingOverride(bindingIndex, proposedPath);
                        SaveOverrides();
                    }

                    action.Enable();
                })
                .OnCancel(operation =>
                {
                    operation.Dispose();
                    activeOperation = null;
                    action.Enable();
                });

            activeOperation.Start();
        }

        private bool IsUsedByAnotherAction(InputAction currentAction, int currentBinding, string proposedPath)
        {
            foreach (InputActionReference reference in actions)
            {
                if (reference == null || reference.action == null) continue;
                InputAction candidate = reference.action;
                for (int i = 0; i < candidate.bindings.Count; i++)
                {
                    if (candidate == currentAction && i == currentBinding) continue;
                    string effectivePath = candidate.bindings[i].effectivePath;
                    if (!string.IsNullOrEmpty(effectivePath) &&
                        string.Equals(effectivePath, proposedPath, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            return false;
        }

        private void SaveOverrides()
        {
            string json = "{}";
            foreach (InputActionReference reference in actions)
            {
                if (reference == null || reference.action == null) continue;
                json = reference.action.actionMap.asset.SaveBindingOverridesAsJson();
                break;
            }
            PlayerPrefs.SetString(OverridesKey, json);
            PlayerPrefs.Save();
        }

        private void OnDestroy()
        {
            activeOperation?.Cancel();
            activeOperation?.Dispose();
        }
    }
}
