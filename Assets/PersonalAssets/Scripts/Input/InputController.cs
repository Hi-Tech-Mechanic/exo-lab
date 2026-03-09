namespace ExoLab.Input
{
    using DG.Tweening;
    using ExoLab.Constants;
    using StarterAssets;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;
    using UnityEngine.InputSystem;

    internal class InputController : MonoBehaviour
    {
        private const string BindingSavePath = "bindings.json";

        public static InputController Instance { get; private set; }

        [SerializeField] private Camera firstPersonCamera;
        [SerializeField] private Camera cameraBack;
        [SerializeField] private Camera cameraForward;

        [SerializeField] private List<Transform> suitComponents;

        [SerializeField] private GameObject assemblyMenu;
        [SerializeField] private GameObject assemblyProps;

        [SerializeField] private GameObject mainMenu;

        private PlayerControls controls;

        private List<Vector3> targetPosition = new();
        private List<Quaternion> targetEulerAngles = new();
        private List<Transform> parentsTransforms = new();

        public GameObject inventory;
        public GameObject stats;

        private Camera? lastEnabledCamera;

        private GameObject playerArmature;

        private bool _assemblyMode = false;
        private bool AssemblyMode
        {
            get => _assemblyMode;
            set
            {
                this._assemblyMode = value;

                this.playerArmature.SetActive(!value);
                this.lastEnabledCamera?.gameObject.SetActive(!value);

                this.assemblyMenu.SetActive(value);
                this.assemblyProps.SetActive(value);

                StarterAssetsInputs.Instance.ToggleCursorInputForLook(!value);
                StarterAssetsInputs.Instance.ToggleCursorLocked(!value);
            }
        }

        public event Action<string> OnBindingChanged;

        #region Input Actions

        private InputAction jumpAction;
        private InputAction interactAction;
        private InputAction assemblyModeAction;
        private InputAction inventoryAction;
        private InputAction escapeAction;
        private InputAction keyboard_1;
        private InputAction keyboard_2;
        private InputAction keyboard_3;
        private InputAction keyboard_4;
        private InputAction keyboard_5;
        private InputAction keyboard_6;
        private InputAction keyboard_7;
        private InputAction keyboard_8;
        private InputAction keyboard_9;

        public event Action OnJumpStarted;
        public event Action OnJumpCanceled;
        public event Action OnInteractPressed;
        public event Action OnInventoryPressed;
        public event Action OnEscapePressed;
        public event Action OnAssemblyModePressed;
        public event Action OnPressedKeyboard_1;
        public event Action OnPressedKeyboard_2;
        public event Action OnPressedKeyboard_3;
        public event Action OnPressedKeyboard_4;
        public event Action OnPressedKeyboard_5;
        public event Action OnPressedKeyboard_6;
        public event Action OnPressedKeyboard_7;
        public event Action OnPressedKeyboard_8;
        public event Action OnPressedKeyboard_9;

        #endregion

        private void Awake()
        {
            this.InitBindings();
            this.InitComponents();
        }

        private void OnEnable()
        {
            // Включаем карту действий
            this.controls.Player.Enable();

            this.jumpAction.started += ctx => this.OnJumpStarted?.Invoke();
            this.jumpAction.canceled += ctx => this.OnJumpCanceled?.Invoke();

            this.interactAction.performed += ctx => this.OnInteractPressed?.Invoke();
            this.assemblyModeAction.performed += ctx => this.OnAssemblyModePressed?.Invoke();
            this.escapeAction.performed += ctx => this.OnEscapePressed?.Invoke();
            this.inventoryAction.performed += ctx => this.OnInventoryPressed?.Invoke();

            this.keyboard_1.performed += ctx => this.OnPressedKeyboard_1?.Invoke();
            this.keyboard_2.performed += ctx => this.OnPressedKeyboard_2?.Invoke();
            this.keyboard_3.performed += ctx => this.OnPressedKeyboard_3?.Invoke();
            this.keyboard_4.performed += ctx => this.OnPressedKeyboard_4?.Invoke();
            this.keyboard_5.performed += ctx => this.OnPressedKeyboard_5?.Invoke();
            this.keyboard_6.performed += ctx => this.OnPressedKeyboard_6?.Invoke();
            this.keyboard_7.performed += ctx => this.OnPressedKeyboard_7?.Invoke();
            this.keyboard_8.performed += ctx => this.OnPressedKeyboard_8?.Invoke();
            this.keyboard_9.performed += ctx => this.OnPressedKeyboard_9?.Invoke();

            this.OnPressedKeyboard_1 += this.EnableFirstPersonCamera;
            this.OnPressedKeyboard_2 += this.EnableBackCamera;
            this.OnPressedKeyboard_3 += this.EnableForwardCamera;
            this.OnPressedKeyboard_4 += this.ShowStats;
            this.OnPressedKeyboard_8 += this.InvokeSuitDestruction;
            this.OnPressedKeyboard_9 += this.InvokeSuitRegenerate;
            this.OnAssemblyModePressed += this.ToggleAssemblyMode;
            this.OnInventoryPressed += this.ShowInventory;
            this.OnEscapePressed += this.GoBack;
        }

        private void OnDisable()
        {
            this.jumpAction.started -= ctx => this.OnJumpStarted?.Invoke();
            this.jumpAction.canceled -= ctx => this.OnJumpCanceled?.Invoke();

            this.interactAction.performed -= ctx => this.OnInteractPressed?.Invoke();
            this.assemblyModeAction.performed -= ctx => this.OnAssemblyModePressed?.Invoke();
            this.escapeAction.performed -= ctx => this.OnEscapePressed?.Invoke();
            this.inventoryAction.performed -= ctx => this.OnInventoryPressed?.Invoke();

            this.keyboard_1.performed -= ctx => this.OnPressedKeyboard_1?.Invoke();
            this.keyboard_2.performed -= ctx => this.OnPressedKeyboard_2?.Invoke();
            this.keyboard_3.performed -= ctx => this.OnPressedKeyboard_3?.Invoke();
            this.keyboard_4.performed -= ctx => this.OnPressedKeyboard_4?.Invoke();
            this.keyboard_5.performed -= ctx => this.OnPressedKeyboard_5?.Invoke();
            this.keyboard_6.performed -= ctx => this.OnPressedKeyboard_6?.Invoke();
            this.keyboard_7.performed -= ctx => this.OnPressedKeyboard_7?.Invoke();
            this.keyboard_8.performed -= ctx => this.OnPressedKeyboard_8?.Invoke();
            this.keyboard_9.performed -= ctx => this.OnPressedKeyboard_9?.Invoke();

            this.OnPressedKeyboard_1 -= this.EnableFirstPersonCamera;
            this.OnPressedKeyboard_2 -= this.EnableBackCamera;
            this.OnPressedKeyboard_3 -= this.EnableForwardCamera;
            this.OnPressedKeyboard_4 -= this.ShowStats;
            this.OnPressedKeyboard_8 -= this.InvokeSuitDestruction;
            this.OnPressedKeyboard_9 -= this.InvokeSuitRegenerate;
            this.OnAssemblyModePressed -= this.ToggleAssemblyMode;
            this.OnInventoryPressed -= this.ShowInventory;
            this.OnEscapePressed -= this.GoBack;

            this.controls.Player.Disable();
        }

        private void InitBindings()
        {
            // Защита от дублирования синглтона
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;

            this.controls = new PlayerControls();

            this.jumpAction = this.controls.Player.Jump;
            this.interactAction = this.controls.Player.Interact;
            this.escapeAction = this.controls.Player.Escape;
            this.assemblyModeAction = this.controls.Player.AssemblyMode;
            this.inventoryAction = this.controls.Player.Inventory;
            this.keyboard_1 = this.controls.Player.Keyboard_1;
            this.keyboard_2 = this.controls.Player.Keyboard_2;
            this.keyboard_3 = this.controls.Player.Keyboard_3;
            this.keyboard_4 = this.controls.Player.Keyboard_4;
            this.keyboard_5 = this.controls.Player.Keyboard_5;
            this.keyboard_6 = this.controls.Player.Keyboard_6;
            this.keyboard_7 = this.controls.Player.Keyboard_7;
            this.keyboard_8 = this.controls.Player.Keyboard_8;
            this.keyboard_9 = this.controls.Player.Keyboard_9;
        }

        private void InitComponents()
        {
            this.playerArmature = this.transform.GetChild(0).gameObject;

            // Координаты частей экзоскелета
            foreach (var component in suitComponents)
            {
                this.targetPosition.Add(component.transform.localPosition);
                this.targetEulerAngles.Add(component.transform.localRotation);
                this.parentsTransforms.Add(component.transform.parent);
            }
        }

        private void EnableBackCamera()
        {
            this.EnableCamera(this.cameraBack);
        }

        private void EnableForwardCamera()
        {
            this.EnableCamera(this.cameraForward);
        }

        private void EnableFirstPersonCamera()
        {
            this.EnableCamera(this.firstPersonCamera);
        }

        private void EnableCamera(Camera camera)
        {
            List<Camera> allCameras = new (){ this.cameraBack , this.cameraForward, this.firstPersonCamera };

            foreach (var localCamera in allCameras)
            {
                if (localCamera.Equals(camera) == false)
                {
                    localCamera.gameObject.SetActive(false);
                }
            }

            if (camera.gameObject.activeInHierarchy == false)
            {
                camera.gameObject.SetActive(true);
            }

            this.lastEnabledCamera = camera;
            this.ToggleMainMenu(false);
            this.AssemblyModeToggle(false);
        }

        private void ShowInventory()
        {
            this.inventory.SetActive(!this.inventory.activeInHierarchy);
        }

        private void ShowStats()
        {
            this.stats.SetActive(!this.stats.activeInHierarchy);
        }

        private void InvokeSuitDestruction()
        {
            IEnumerator c = DescroySuit();
            StartCoroutine(c);
            Notifications.InvokeWarnNotify("Разрушение экзоскелета запущено", TransformDirections.RectDirection.Center);
        }

        private void InvokeSuitRegenerate()
        {
            IEnumerator c = RepairSuit();
            StartCoroutine(c);
            Notifications.InvokeStandardNotify("Регенерация экзоскелета запущена", TransformDirections.RectDirection.TopCenter);
        }

        private void ToggleAssemblyMode()
        {
            this.AssemblyModeToggle();
        }

        private void GoBack()
        {
            this.AssemblyModeToggle(false);
            this.ToggleMainMenu();
        }

        private void AssemblyModeToggle(bool? state = null)
        {
            if (state != null)
            {
                this.AssemblyMode = (bool)state;
            }
            else
            {
                this.AssemblyMode = !this.AssemblyMode;
            }

            GameEvents.RaiseAssemblyModeEnabled(this.AssemblyMode);
        }

        private void ToggleMainMenu(bool? state = null)
        {
            if (state != null)
            {
                this.mainMenu.SetActive((bool)state);
                return;
            }

            this.mainMenu.gameObject.SetActive(!this.mainMenu.activeInHierarchy);
        }
        IEnumerator DescroySuit()
        {
            foreach (var component in suitComponents)
            {
                component.SetParent(this.transform.parent);
                component.GetComponent<Rigidbody>().isKinematic = false;
                component.GetComponent<Rigidbody>().useGravity = true;

                yield return new WaitForSeconds(0.5f);
            }
        }

        IEnumerator RepairSuit()
        {
            for (int i = 0; i < suitComponents.Count; i++)
            {
                var component = suitComponents[i];

                component.transform.SetParent(parentsTransforms[i]);
                component.transform.DOLocalMove(targetPosition[i], 0.7f);
                component.transform.DOLocalRotateQuaternion(targetEulerAngles[i], 0.7f);

                component.GetComponent<Rigidbody>().isKinematic = true;
                component.GetComponent<Rigidbody>().useGravity = false;

                yield return new WaitForSeconds(0.5f);
            }
        }

        /// <summary>
        /// Запускает процесс ожидания нажатия клавиши для конкретного действия
        /// </summary>
        /// <param name="action">Какое действие меняем (например, _jumpAction)</param>
        /// <param name="onComplete">Коллбек, когда игрок нажал новую кнопку</param>
        public void StartRebind(InputAction action, Action<string> onComplete)
        {
            // Отключаем действие, чтобы во время настройки оно не срабатывало в игре
            action.Disable();

            action.PerformInteractiveRebinding()
                .WithControlsExcluding("Mouse") // Игнорируем мышь, если меняем клавиатуру (опционально)
                .OnMatchWaitForAnother(0.1f)    // Ждем, если нажато несколько кнопок (комбо)
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
                    OnBindingChanged?.Invoke(newBindingName);
                    onComplete?.Invoke(newBindingName);
                })
                .Start();
        }

        public void SaveBindings()
        {
            string json = controls.asset.SaveBindingOverridesAsJson();
            string path = Path.Combine(Application.persistentDataPath, BindingSavePath);
            File.WriteAllText(path, json);
            Debug.Log("Настройки управления сохранены");
        }

        public void LoadBindings()
        {
            string path = Path.Combine(Application.persistentDataPath, BindingSavePath);
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                controls.asset.LoadBindingOverridesFromJson(json);
                Debug.Log("Настройки управления загружены");
            }
        }

        public void ResetToDefaults()
        {
            this.controls.asset.RemoveAllBindingOverrides();
            SaveBindings();
            Debug.Log("Управление сброшено к стандартному");
        }

        // Хелпер для UI, чтобы узнать текущую клавишу
        public string GetBindingName(InputAction action)
        {
            return action.controls[0].displayName;
        }

        internal InputAction GetJumpAction()
        {
            return this.jumpAction;
        }

        internal InputAction GetInteractAction()
        {
            return this.interactAction;
        }
    }
}
