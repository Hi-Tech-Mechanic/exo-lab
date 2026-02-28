namespace ExoLab
{
    using Assets.PersonalAssets.Scripts.UI;
    using StarterAssets;
    using UnityEngine;

    /// <summary>
    /// Любой интерактивный объект
    /// </summary>
    public class InteractiveObject : MonoBehaviour
    {
        public static InteractiveObject Instance;

        [SerializeField]
        private string tooltipText = "Взаимодействовать";

        protected string KeyName => this.GetKeyName();
        protected string TooltipText => this.tooltipText;

        public delegate void KeypressDelegate();

        public KeypressDelegate keypressDelegate;

        protected void Awake()
        {
            Instance = this;
        }

        private InteractiveObject()
        {
            this.keypressDelegate = new KeypressDelegate(this.KeypressEvent);
        }

        /// <summary>
        /// Сделать что либо, переопределять вкладывая сюда метод с какой-либо логикой
        /// для взаимодействия
        /// </summary>
        public virtual void DoAction()
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
        /// Вернуть имя кнопки 
        /// </summary>
        /// <returns></returns>
        protected virtual string GetKeyName()
        {
            return Constants.Constants.InputButtons.InteractiveButton;
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
