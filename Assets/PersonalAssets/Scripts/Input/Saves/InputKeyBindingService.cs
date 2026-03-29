namespace ExoLab.Input
{
    using System;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using System.IO;

    /// <summary>
    /// Сервис по переназначению клавиш ввода, их сохранению и загрузки
    /// из json по пути <see cref="BindingSavePath"/>>
    /// </summary>
    internal class InputKeyBindingService
    {
        private const string BindingSavePath = "bindings.json";

        private PlayerControls controls;

        public event Action<string> OnBindingChanged;

        internal InputKeyBindingService(PlayerControls controls)
        {
            this.controls = controls;
        }

        /// <summary>
        /// Ребинд для составного действия (Move WASD)
        /// WARN не работает переназначение движения, не вызывает Callback после нажатия желаемой клавиши, не знаю как исправить
        /// </summary>
        /// <param name="moveAction"></param>
        /// <param name="bindingIndex"></param>
        /// <param name="onComplete"></param>
        [Obsolete("Возможность переназначения отключена")]
        public void StartRebindMove(InputAction moveAction, int bindingIndex, Action<string> onComplete)
        {
            var bindingId = moveAction.bindings[bindingIndex].id;
            var bindingMask = new InputBinding { id = bindingId };

            // Warn если не отключать будет вызывать ошибку
            moveAction.Disable();

            moveAction.PerformInteractiveRebinding()
                .WithBindingMask(bindingMask)
                .WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(callback =>
                {
                    string newBindingName = callback.action.controls[0].displayName;
                    callback.Dispose();

                    callback.action.Enable();

                    SaveBindings();
                    OnBindingChanged?.Invoke(newBindingName);
                    onComplete?.Invoke(newBindingName);
                })
                .Start();
        }

        /// <summary>
        /// Запускает процесс ожидания нажатия клавиши для конкретного действия
        /// </summary>
        /// <param name="action">Какое действие меняем/></param>
        /// <param name="onComplete">Коллбек, когда игрок нажал новую кнопку</param>
        public void StartRebind(InputAction action, Action<string> onComplete)
        {
            action.Disable();
            action.PerformInteractiveRebinding()
                .WithControlsExcluding("Mouse")  // Игнорируем мышь, если меняем клавиатуру
                .OnMatchWaitForAnother(0.1f)  // Ждем, если нажато несколько кнопок
                .OnComplete(callback =>
                {
                    // Получаем имя новой клавиши для UI
                    string newBindingName = callback.action.controls[0].displayName;
                    // Завершаем ребиндинг
                    callback.Dispose();
                    // Включаем действие обратно
                    action.Enable();
                    // Сохраняем изменения
                    SaveBindings();
                    // Сообщаем UI и вызываем коллбек
                    this.OnBindingChanged?.Invoke(newBindingName);
                    onComplete?.Invoke(newBindingName);
                })
                .Start();
        }

        public void ResetToDefaults()
        {
            this.controls.asset.RemoveAllBindingOverrides();
            SaveBindings();
            Debug.Log("Управление сброшено к стандартному");
        }

        public void LoadBindings()
        {
            string path = Path.Combine(Application.persistentDataPath, BindingSavePath);

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                this.controls.asset.LoadBindingOverridesFromJson(json);
                Debug.Log("Настройки управления загружены");
            }
        }

        public void SaveBindings()
        {
            string json = this.controls.asset.SaveBindingOverridesAsJson();
            string path = Path.Combine(Application.persistentDataPath, BindingSavePath);
            File.WriteAllText(path, json);
            Debug.Log("Настройки управления сохранены");
        }
    }
}
