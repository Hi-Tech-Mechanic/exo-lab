namespace ExoLab.UI
{
    using Assets.PersonalAssets.Scripts.Input.Base;
    using ExoLab.Input;
    using System.Collections.Generic;
    using TMPro;

    /// <summary>
    /// Визуальная часть клавиш ввода
    /// </summary>
    internal abstract class InputKeysViewBase
    {
        protected InputControllersManager InputManager = InputControllersManager.Instance;
        
        protected InputControllerManagerView ParentView;
        
        /// <summary>
        /// Клавиши которые не имеют составные действия
        /// </summary>
        protected List<ControlKeyView> standardKeys = new List<ControlKeyView>();

        protected bool Initialized;

        protected InputKeysViewBase(InputControllerManagerView parentView)
        {
            this.ParentView = parentView;

            this.InitKeyComponents();
        }

        public virtual void SetAllButtonsInteractable(bool interactable)
        {
            foreach (var key in this.standardKeys)
            {
                key.Button.interactable = interactable;
            }
        }

        public virtual void SetListeners()
        {
            foreach (var key in this.standardKeys)
            {
                this.SetRebindListenerToStandardAction(key);
            }
        }
        public virtual void UpdateKeyTexts()
        {
            foreach (var key in this.standardKeys)
            {
                this.UpdateKeyButtonText(key);
            }
        }

        protected abstract void InitKeyComponents();

        /// <summary>
        /// Выставить переназначение клавиши на действие которое имеет одно событие
        /// </summary>
        protected void SetRebindListenerToStandardAction(ControlKeyView keyControl)
        {
            keyControl.Button.onClick.AddListener(() => this.ParentView.StartRebindProcess(keyControl.InputAction, keyControl.ButtonText));
        }

        protected void UpdateKeyButtonText(ControlKeyView controlKey)
        {
            controlKey.ButtonText.text = this.InputManager.GetBindingName(controlKey.InputAction);
        }

        protected void SetHeaderText(TextMeshProUGUI header, string text)
        {
            header.text = text;
        }
    }
}
