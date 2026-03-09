namespace ExoLab.Interaction
{
    using Assets.PersonalAssets.Scripts.UI;
    using ExoLab.Input;
    using StarterAssets;
    using Unity.Tutorials.Core.Editor;
    using UnityEngine;

    /// <summary>
    /// Любой интерактивный объект
    /// </summary>
    public class InteractiveObject : MonoBehaviour
    {
        public static InteractiveObject Instance;

        [SerializeField]
        private string tooltipText = string.Empty;

        protected string KeyName => this.GetKeyName();
        protected string TooltipText => this.tooltipText;

        public delegate void KeypressDelegate();

        public KeypressDelegate keypressDelegate;

        protected void Awake()
        {
            Instance = this;
            this.keypressDelegate = new KeypressDelegate(this.KeypressEvent);
        }

        private void OnValidate()
        {
            if (this.tooltipText.IsNullOrEmpty() == true)
            {
                this.tooltipText = this.GetTooltipText();
            }
        }

        /// <summary>
        /// Сделать что либо, переопределять вкладывая сюда метод с какой-либо логикой
        /// для взаимодействия
        /// </summary>
        public virtual void Interact()
        {
            Debug.Log("TestAction");
        }

        public void DisplayMessage()
        {
            HUD.Instance.DisplayTooltipText($"{this.TooltipText} - {this.KeyName}");
        }

        public void HideMessage()
        {
            HUD.Instance.DisplayTooltipText(string.Empty);
        }

        /// <summary>
        /// Вернуть текст подсказки
        /// </summary>
        /// <returns></returns>
        protected virtual string GetTooltipText()
        {
            return "Взаимодействовать";
        }

        /// <summary>
        /// Вернуть имя кнопки 
        /// </summary>
        /// <returns></returns>
        protected virtual string GetKeyName()
        {
            var action = InputController.Instance.GetInteractAction();
            return InputController.Instance.GetBindingName(action);
        }

        /// <summary>
        /// Событие которое должно происходить после нажатия клавиши
        /// </summary>
        protected virtual void KeypressEvent()
        {
            StarterAssetsInputs.Instance.ToggleCursorInputForLook();
            StarterAssetsInputs.Instance.ToggleCursorLocked();
        }
    }
}
