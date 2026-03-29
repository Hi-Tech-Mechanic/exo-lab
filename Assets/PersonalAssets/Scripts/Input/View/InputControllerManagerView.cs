namespace ExoLab.UI
{
    using UnityEngine.InputSystem;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using ExoLab.Input;
    using System;
   
    /// <summary>
    /// Менеджер агрегирующий в себе визуализаторы клавиш ввода
    /// </summary>
    public class InputControllerManagerView : MonoBehaviour, ISubsribable
    {
        private const string rebindingMessage = "Нажмите кнопку...";

        [Header("Control")]
        [Space(5)]
        [SerializeField] private Movement movementButtons;
        [Space(5)]
        [SerializeField] private Interaction interactionbuttons;
        [Space(5)]
        [SerializeField] private Button btnReset;

        private bool isRebinding = false;

        private InputControllersManager inputManager;

        private MovementKeysView movementControllerView;
        private InteractKeysView interactKeysView;

        internal Movement MovementButtons => this.movementButtons;
        internal Interaction InteractionButtons => this.interactionbuttons;

        public void Init()
        {
            this.movementControllerView = new MovementKeysView(this);
            this.interactKeysView = new InteractKeysView(this);
            this.inputManager = InputControllersManager.Instance;

            this.UpdateKeyTexts();
        }

        public void SubscribeEvents()
        {
            this.movementControllerView.SetListeners();
            this.interactKeysView.SetListeners();

            this.btnReset.onClick.AddListener(ResetBindings);

            // Слушаем изменения от менеджера
            this.inputManager.KeyBindingStorage.OnBindingChanged += UpdateKeyTexts;
        }

        public void UnsubscribeEvents()
        {
            this.inputManager.KeyBindingStorage.OnBindingChanged -= UpdateKeyTexts;
        }

        // Нужно добавить публичные геттеры в InputManager для доступа к действиям
        // Или передавать их иначе. Для примера предположим, что они есть.
        public void StartRebindProcess(InputAction action, TextMeshProUGUI textField)
        {
            if (this.isRebinding)
            {
                return; // Защита от двойного нажатия
            }

            this.isRebinding = true;
            textField.text = rebindingMessage;
            this.SetAllButtonsInteractable(false);

            this.inputManager.KeyBindingStorage.StartRebind(action, (newKey) =>
            {
                this.isRebinding = false;
                this.SetAllButtonsInteractable(true);
            });
        }

        [Obsolete("Возможность переназначения отключена")]
        public void StartRebindMove(int bindingIndex, TextMeshProUGUI textField)
        {
            if (this.isRebinding)
            {
                return;
            }

            this.isRebinding = true;
            textField.text = rebindingMessage;
            this.SetAllButtonsInteractable(false);

            this.inputManager.KeyBindingStorage.StartRebindMove(
                this.inputManager.Movement.Move,
                bindingIndex,
                (newKey) => {
                    this.isRebinding = false;
                    this.SetAllButtonsInteractable(true);
                }
            );
        }

        public void UpdateKeyTexts(string _ = null)
        {
            this.movementControllerView.UpdateKeyTexts();
            this.interactKeysView.UpdateKeyTexts();
        }

        private void SetAllButtonsInteractable(bool interactable)
        {
            this.movementControllerView.SetAllButtonsInteractable(interactable);
            this.interactKeysView.SetAllButtonsInteractable(interactable);
        }

        private void ResetBindings()
        {
            this.inputManager.KeyBindingStorage.ResetToDefaults();
            this.UpdateKeyTexts();
        }

        [Serializable]
        internal struct Movement
        {
            [Tooltip("Заголовок раздела")]
            public TextMeshProUGUI PageHeader;
            [Space(10)]
            public GameObject ChangeMoveForward;    
            public GameObject ChangeMoveBack;
            public GameObject ChangeMoveLeft;
            public GameObject ChangeMoveRight;
            [Space(5)]
            public GameObject ChangeJump;
            public GameObject ChangeSprint;
        }

        [Serializable]
        internal struct Interaction
        {
            [Tooltip("Заголовок раздела")]
            public TextMeshProUGUI PageHeader;
            [Space(10)]
            public GameObject ChangeInteract;
            public GameObject ChangeSuitRegeneration;
            public GameObject ChangeSuitDestruction;
            public GameObject ChangeShowStats;
            public GameObject ChangeFirstPersonCamera;
            public GameObject ChangeBackCamera;
            public GameObject ChangeForwardCamera;
        }
    }
}
