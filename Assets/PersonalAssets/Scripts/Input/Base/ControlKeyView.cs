using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Assets.PersonalAssets.Scripts.Input.Base
{
    /// <summary>
    /// Клавиша в настройках управления
    /// инициализирует сразу свои компоненты при создании:
    /// <see cref="ActionName"/>, <see cref="ButtonText"/>> и <see cref="Button"/> 
    /// </summary>
    internal class ControlKeyView
    {
        protected const string KeyTextPattern = "Text";

        public TextMeshProUGUI ActionName;
        public TextMeshProUGUI ButtonText;
        public Button Button;
        public InputAction? InputAction;

        internal ControlKeyView(GameObject parent, string actionName, InputAction? inputAction = null)
        {
            this.ActionName = this.InitKeyNameComponent(parent);
            (this.Button, this.ButtonText) = InitButtonsComponents(parent);

            this.ActionName.text = actionName;
            this.InputAction = inputAction;
        }

        private TextMeshProUGUI InitKeyNameComponent(GameObject parentObject)
        {
            var textComponents = parentObject.GetComponentsInChildren<TextMeshProUGUI>();
            var component = textComponents.Where(component => component.name == KeyTextPattern).First();
            return component;
        }

        private (Button, TextMeshProUGUI) InitButtonsComponents(GameObject parentObject)
        {
            var button = parentObject.GetComponentInChildren<Button>();
            var buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            return (button, buttonText);
        }
    }
}
