namespace ExoLab
{
    using Assets.PersonalAssets.Scripts.UI;
    using StarterAssets;
    using UnityEngine;

    /// <summary>
    /// Любой интерактивный объект
    /// </summary>
    [RequireComponent(typeof(SphereCollider))]
    public class InteractiveObject : MonoBehaviour
    {
        public static InteractiveObject Instance;

        protected string KeyName => this.GetKeyName();
        protected string TooltipText => this.GetTooltipText();

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
        /// Вернуть имя кнопки 
        /// </summary>
        /// <returns></returns>
        protected virtual string GetKeyName()
        {
            return Constants.Constants.InputButtons.InteractiveButton;
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
        /// Событие которое должно происходить после нажатия клавиши
        /// </summary>
        protected virtual void KeypressEvent()
        {
            StarterAssetsInputs.Instance.ToggleCursorInputForLook();
            StarterAssetsInputs.Instance.ToggleCursorLocked();
        }

        private void DisplayInteractioveButton()
        {
            HUD.Instance.DisplayTooltipText($"{this.TooltipText} - {this.KeyName}");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag == Constants.Constants.Tags.Player)
            {
                this.DisplayInteractioveButton();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.tag == Constants.Constants.Tags.Player)
            {
                HUD.Instance.HideTooltipText();
            }
        }
    }
}
