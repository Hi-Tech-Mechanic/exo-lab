namespace ExoLab.UI
{
    using UnityEngine.InputSystem;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using ExoLab.Input;

    public class SettingsUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private Button _btnChangeJump;
        [SerializeField] private TextMeshProUGUI _txtJumpKey;

        [SerializeField] private Button _btnChangeInteract;
        [SerializeField] private TextMeshProUGUI _txtInteractKey;

        [SerializeField] private Button _btnReset;

        private bool _isRebinding = false;

        private void Start()
        {
            // Обновляем текст при открытии меню
            UpdateKeyTexts();
        }

        private void OnEnable()
        {
            // Подписываемся на кнопки
            _btnChangeJump.onClick.AddListener(() => StartRebindProcess(InputController.Instance.GetJumpAction(), _txtJumpKey));
            _btnChangeInteract.onClick.AddListener(() => StartRebindProcess(InputController.Instance.GetInteractAction(), _txtInteractKey));
            _btnReset.onClick.AddListener(ResetBindings);

            // Слушаем изменения от менеджера
            InputController.Instance.OnBindingChanged += UpdateKeyTexts;
        }

        private void OnDisable()
        {
            InputController.Instance.OnBindingChanged -= UpdateKeyTexts;
        }

        // Нужно добавить публичные геттеры в InputManager для доступа к действиям
        // Или передавать их иначе. Для примера предположим, что они есть.

        private void StartRebindProcess(InputAction action, TextMeshProUGUI textField)
        {
            if (_isRebinding) return; // Защита от двойного нажатия

            _isRebinding = true;
            textField.text = "Нажмите кнопку...";
            _btnChangeJump.interactable = false;
            _btnChangeInteract.interactable = false;

            InputController.Instance.StartRebind(action, (newKey) =>
            {
                _isRebinding = false;
                _btnChangeJump.interactable = true;
                _btnChangeInteract.interactable = true;
                // Текст обновится через событие OnBindingChanged
            });
        }

        private void UpdateKeyTexts(string _ = null)
        {
            // Предполагаем, что в InputManager добавлены методы GetJumpAction/GetInteractAction
            // Или передаем ссылки через конструктор/инспектор
            _txtJumpKey.text = InputController.Instance.GetBindingName(InputController.Instance.GetJumpAction());
            _txtInteractKey.text = InputController.Instance.GetBindingName(InputController.Instance.GetInteractAction());
        }

        private void ResetBindings()
        {
            InputController.Instance.ResetToDefaults();
            UpdateKeyTexts();
        }
    }
}
