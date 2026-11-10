using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VovanderVR
{
    /// <summary>
    /// Позволяет игроку назначать кнопки и стики заново.
    /// Не разрешает назначить один и тот же физический элемент управления двум разным действиям.
    /// Выбранные настройки сохраняются в памяти самого шлема.
    /// </summary>
    public sealed class VovanderRebindingManager : MonoBehaviour
    {
        [Header("Список действий, которым разрешено менять кнопки")]
        [SerializeField] private List<InputActionReference> actions = new List<InputActionReference>();

        // Это имя локальной записи с настройками управления. В GitHub сами настройки игрока не попадают.
        private const string OverridesKey = "VovanderVR.InputOverrides.v1";
        private InputActionRebindingExtensions.RebindingOperation activeOperation;

        /// <summary>
        /// При запуске загружает ранее сохранённые назначения кнопок и стиков.
        /// </summary>
        private void Awake()
        {
            string savedOverrides = PlayerPrefs.GetString(OverridesKey, "{}");

            foreach (InputActionReference reference in actions)
            {
                if (reference != null && reference.action != null)
                    reference.action.LoadBindingOverridesFromJson(savedOverrides);
            }
        }

        /// <summary>
        /// Начинает ожидать нажатие новой кнопки или движение стика для выбранного действия.
        /// Номер действия и номер его назначения передаются кнопкой соответствующей строки настроек.
        /// </summary>
        /// <param name="actionListIndex">Порядковый номер действия в списке настроек.</param>
        /// <param name="bindingIndex">Номер назначения внутри выбранного действия.</param>
        public void BeginRebind(int actionListIndex, int bindingIndex)
        {
            if (actionListIndex < 0 || actionListIndex >= actions.Count)
                return;

            InputAction action = actions[actionListIndex] != null
                ? actions[actionListIndex].action
                : null;

            if (action == null || bindingIndex < 0 || bindingIndex >= action.bindings.Count)
                return;

            // Если игрок уже переназначал другую кнопку, сначала безопасно завершаем прежнее ожидание.
            activeOperation?.Cancel();
            activeOperation?.Dispose();

            action.Disable();
            activeOperation = action.PerformInteractiveRebinding(bindingIndex)
                .WithCancelingThrough("<Keyboard>/escape")
                .OnComplete(operation =>
                {
                    string proposedPath = operation.selectedControl != null
                        ? operation.selectedControl.path
                        : null;

                    operation.Dispose();
                    activeOperation = null;

                    // Если кнопка уже используется, отменяем новое назначение и оставляем прежнее управление.
                    if (string.IsNullOrEmpty(proposedPath) ||
                        IsUsedByAnotherAction(action, bindingIndex, proposedPath))
                    {
                        action.RemoveBindingOverride(bindingIndex);
                        Debug.LogWarning("Vovander VR: эта кнопка уже назначена другому действию. Выберите другую.");
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

        /// <summary>
        /// Проверяет, не занята ли выбранная кнопка или ось стика другим действием из списка.
        /// </summary>
        /// <param name="currentAction">Действие, для которого сейчас выбирается новая кнопка.</param>
        /// <param name="currentBinding">Номер назначения, которое сейчас меняется.</param>
        /// <param name="proposedPath">Путь к физической кнопке или оси, выбранной игроком.</param>
        /// <returns>Возвращает true, если такая кнопка уже назначена другому действию.</returns>
        private bool IsUsedByAnotherAction(InputAction currentAction, int currentBinding, string proposedPath)
        {
            foreach (InputActionReference reference in actions)
            {
                if (reference == null || reference.action == null)
                    continue;

                InputAction candidate = reference.action;

                for (int i = 0; i < candidate.bindings.Count; i++)
                {
                    if (candidate == currentAction && i == currentBinding)
                        continue;

                    string effectivePath = candidate.bindings[i].effectivePath;

                    if (!string.IsNullOrEmpty(effectivePath) &&
                        string.Equals(effectivePath, proposedPath, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Записывает текущие изменения назначений в локальные настройки приложения на шлеме.
        /// </summary>
        private void SaveOverrides()
        {
            string json = "{}";

            foreach (InputActionReference reference in actions)
            {
                if (reference == null || reference.action == null)
                    continue;

                json = reference.action.actionMap.asset.SaveBindingOverridesAsJson();
                break;
            }

            PlayerPrefs.SetString(OverridesKey, json);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// При закрытии объекта отменяет незавершённое переназначение, чтобы не оставить систему ввода заблокированной.
        /// </summary>
        private void OnDestroy()
        {
            activeOperation?.Cancel();
            activeOperation?.Dispose();
        }
    }
}
