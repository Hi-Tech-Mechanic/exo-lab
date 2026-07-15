namespace ExoLab.Input
{
    using ExoLab.UI;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.InputSystem;

    /// <summary>
    /// Элемент агрегирующий в себе системы ввода, отввечающие за разные группы действий
    /// </summary>
    internal class InputControllersManager : MonoBehaviour
    {
        public static InputControllersManager Instance { get; private set; }

        [SerializeField] private InputControllerManagerView inputControllerManagerView;

        [Header("Костюм")]
        [SerializeField] private List<Transform> suitComponents;
        [SerializeField] private GameObject playerArmature;
        [Tooltip("Ссылка на корневой элемент сцене")]
        [SerializeField] private Transform rootSceneTransform;

        public GameObject inventory;
        public GameObject stats;

        private PlayerControls controls;

        public InputKeyBindingService KeyBindingStorage { get; private set; }
        public MovementInputController Movement { get; private set; }
        public InteractionInputController Interaction { get; private set; }

        #region Getters

        public List<Transform> SuitComponents => this.suitComponents;
        public GameObject PlayerArmature => this.playerArmature;
        public Transform RootSceneTransform => this.rootSceneTransform;

        #endregion

        private void Awake()
        {
            this.Init();
        }

        private void OnEnable()
        {
            this.SubscribeEvents();
        }

        private void OnDisable()
        {
            this.UnsubscribeEvents();
        }

        // Хелпер для UI, чтобы узнать текущую клавишу
        public string GetBindingName(InputAction action)
        {
            return action.controls[0].displayName;
        }

        public string GetBindingName(InputAction action, int bindingIndex = 0)
        {
            if (action == null || action.bindings.Count == 0)
                return "Not Bound";

            if (bindingIndex >= action.bindings.Count)
                return "Not Bound";

            // Получаем имя бинда по индексу
            var binding = action.controls[bindingIndex]; // todo не робит
            return binding.name;
        }

        private void Init()
        {
            // Защита от дублирования синглтона
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            
            this.controls = new PlayerControls();

            this.Movement = new MovementInputController(this, this.controls);
            this.Interaction = new InteractionInputController(this, this.controls);
            this.KeyBindingStorage = new InputKeyBindingService(this.controls);

            this.KeyBindingStorage.LoadBindings();
            this.inputControllerManagerView.Init();
        }

        private void SubscribeEvents()
        {
            // Включаем карту действий
            this.controls.Player.Enable();

            this.Movement.SubscribeEvents();
            this.Interaction.SubscribeEvents();
            this.inputControllerManagerView.SubscribeEvents();
        }

        private void UnsubscribeEvents()
        {
            this.Movement.UnsubscribeEvents();
            this.Interaction.UnsubscribeEvents();
            this.inputControllerManagerView.UnsubscribeEvents();

            // Выключаем карту действий
            this.controls.Player.Disable();
        }
    }
}
