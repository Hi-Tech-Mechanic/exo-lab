namespace ExoLab.Input
{
    using Assets.PersonalAssets.Scripts.Input.Helpers;
    using DG.Tweening;
    using ExoLab.Constants;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.InputSystem;

    internal class InteractionInputController : InputControllerBase
    {
        private Camera? lastEnabledCamera;

        private List<Vector3> targetPosition = new();
        private List<Quaternion> targetEulerAngles = new();
        private List<Transform> parentsTransforms = new();

        private bool _assemblyModeEnabled = false;
        private bool AssemblyModeEnabled
        {
            get => _assemblyModeEnabled;
            set
            {
                this._assemblyModeEnabled = value;

                this.InputController.PlayerArmature.SetActive(!value);
                this.lastEnabledCamera?.gameObject.SetActive(!value);

                this.InputController.AssemblyMenu.SetActive(value);
                this.InputController.AssemblyProps.SetActive(value);
            }
        }

        public InputAction Interact { get; private set; }
        public InputAction AssemblyMode { get; private set; }
        public InputAction Inventory { get; private set; }
        public InputAction Escape { get; private set; }
        public InputAction Keyboard_1 { get; private set; }
        public InputAction Keyboard_2 { get; private set; }
        public InputAction Keyboard_3 { get; private set; }
        public InputAction Keyboard_4 { get; private set; }
        public InputAction Keyboard_5 { get; private set; }
        public InputAction Keyboard_6 { get; private set; }
        public InputAction Keyboard_7 { get; private set; }
        public InputAction Keyboard_8 { get; private set; }
        public InputAction Keyboard_9 { get; private set; }

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

        internal InteractionInputController(InputControllersManager inputController, PlayerControls controls)
            : base(inputController, controls) { }

        public override void Init(PlayerControls controls)
        {
            base.Init(controls);
            this.InitComponents();
        }

        public override void SubscribeEvents()
        {
            this.Interact.performed += ctx => this.OnInteractPressed?.Invoke();
            this.AssemblyMode.performed += ctx => this.OnAssemblyModePressed?.Invoke();
            this.Escape.performed += ctx => this.OnEscapePressed?.Invoke();
            this.Inventory.performed += ctx => this.OnInventoryPressed?.Invoke();

            this.Keyboard_1.performed += ctx => this.OnPressedKeyboard_1?.Invoke();
            this.Keyboard_2.performed += ctx => this.OnPressedKeyboard_2?.Invoke();
            this.Keyboard_3.performed += ctx => this.OnPressedKeyboard_3?.Invoke();
            this.Keyboard_4.performed += ctx => this.OnPressedKeyboard_4?.Invoke();
            this.Keyboard_5.performed += ctx => this.OnPressedKeyboard_5?.Invoke();
            this.Keyboard_6.performed += ctx => this.OnPressedKeyboard_6?.Invoke();
            this.Keyboard_7.performed += ctx => this.OnPressedKeyboard_7?.Invoke();
            this.Keyboard_8.performed += ctx => this.OnPressedKeyboard_8?.Invoke();
            this.Keyboard_9.performed += ctx => this.OnPressedKeyboard_9?.Invoke();

            this.OnPressedKeyboard_1 += this.EnableFirstPersonCamera;
            this.OnPressedKeyboard_2 += this.EnableBackCamera;
            this.OnPressedKeyboard_3 += this.EnableForwardCamera;
            this.OnPressedKeyboard_4 += this.ShowStats;
            this.OnPressedKeyboard_8 += this.InvokeSuitDestruction;
            this.OnPressedKeyboard_9 += this.InvokeSuitRegenerate;
            this.OnAssemblyModePressed += this.ToggleAssemblyModeHandler;
            this.OnInventoryPressed += this.ShowInventory;
            this.OnEscapePressed += this.EscapeHandler;
        }

        public override void UnsubscribeEvents()
        {
            this.Interact.performed -= ctx => this.OnInteractPressed?.Invoke();
            this.AssemblyMode.performed -= ctx => this.OnAssemblyModePressed?.Invoke();
            this.Escape.performed -= ctx => this.OnEscapePressed?.Invoke();
            this.Inventory.performed -= ctx => this.OnInventoryPressed?.Invoke();

            this.Keyboard_1.performed -= ctx => this.OnPressedKeyboard_1?.Invoke();
            this.Keyboard_2.performed -= ctx => this.OnPressedKeyboard_2?.Invoke();
            this.Keyboard_3.performed -= ctx => this.OnPressedKeyboard_3?.Invoke();
            this.Keyboard_4.performed -= ctx => this.OnPressedKeyboard_4?.Invoke();
            this.Keyboard_5.performed -= ctx => this.OnPressedKeyboard_5?.Invoke();
            this.Keyboard_6.performed -= ctx => this.OnPressedKeyboard_6?.Invoke();
            this.Keyboard_7.performed -= ctx => this.OnPressedKeyboard_7?.Invoke();
            this.Keyboard_8.performed -= ctx => this.OnPressedKeyboard_8?.Invoke();
            this.Keyboard_9.performed -= ctx => this.OnPressedKeyboard_9?.Invoke();

            this.OnPressedKeyboard_1 -= this.EnableFirstPersonCamera;
            this.OnPressedKeyboard_2 -= this.EnableBackCamera;
            this.OnPressedKeyboard_3 -= this.EnableForwardCamera;
            this.OnPressedKeyboard_4 -= this.ShowStats;
            this.OnPressedKeyboard_8 -= this.InvokeSuitDestruction;
            this.OnPressedKeyboard_9 -= this.InvokeSuitRegenerate;
            this.OnAssemblyModePressed -= this.ToggleAssemblyModeHandler;
            this.OnInventoryPressed -= this.ShowInventory;
            this.OnEscapePressed -= this.EscapeHandler;
        }


        protected override void InitBindings(PlayerControls controls)
        {
            this.Interact = controls.Player.Interact;
            this.Escape = controls.Player.Escape;
            this.AssemblyMode = controls.Player.AssemblyMode;
            this.Inventory = controls.Player.Inventory;
            this.Keyboard_1 = controls.Player.Keyboard_1;
            this.Keyboard_2 = controls.Player.Keyboard_2;
            this.Keyboard_3 = controls.Player.Keyboard_3;
            this.Keyboard_4 = controls.Player.Keyboard_4;
            this.Keyboard_5 = controls.Player.Keyboard_5;
            this.Keyboard_6 = controls.Player.Keyboard_6;
            this.Keyboard_7 = controls.Player.Keyboard_7;
            this.Keyboard_8 = controls.Player.Keyboard_8;
            this.Keyboard_9 = controls.Player.Keyboard_9;
        }

        private void InitComponents()
        {
            // Координаты частей экзоскелета

            foreach (var component in this.InputController.SuitComponents)
            {
                this.targetPosition.Add(component.transform.localPosition);
                this.targetEulerAngles.Add(component.transform.localRotation);
                this.parentsTransforms.Add(component.transform.parent);
            }
        }

        private void EnableBackCamera()
        {
            this.EnableCamera(this.InputController.CameraBack);
        }

        private void EnableForwardCamera()
        {
            this.EnableCamera(this.InputController.CameraForward);
        }

        private void EnableFirstPersonCamera()
        {
            this.EnableCamera(this.InputController.FirstPersonCamera);
        }

        private void EnableCamera(Camera camera)
        {
            List<Camera> allCameras = new() 
            { 
                this.InputController.FirstPersonCamera,
                this.InputController.CameraBack, 
                this.InputController.CameraForward
            };

            foreach (var localCamera in allCameras)
            {
                if (localCamera.Equals(camera) == false)
                {
                    localCamera.gameObject.SetActive(false);
                }
            }

            camera.gameObject.SetActive(true);

            this.lastEnabledCamera = camera;
            this.ToggleMainMenu(false);
            this.ToggleAssemblyMode(false);
            CursorStateController.Instance.ToggleCursor(false);
        }

        private void ShowInventory()
        {
            this.InputController.AssemblyProps.SetActive(!this.InputController.AssemblyProps.activeInHierarchy);
        }

        private void ShowStats()
        {
            this.InputController.PlayerArmature.SetActive(!this.InputController.stats.activeInHierarchy);
        }

        private void InvokeSuitDestruction()
        {
            IEnumerator c = DescroySuit();
            this.InputController.StartCoroutine(c);
            Notifications.InvokeWarnNotify("Разрушение экзоскелета запущено", TransformDirections.RectDirection.Center);
        }

        private void InvokeSuitRegenerate()
        {
            IEnumerator c = RepairSuit();
            this.InputController.StartCoroutine(c);
            Notifications.InvokeStandardNotify("Регенерация экзоскелета запущена", TransformDirections.RectDirection.TopCenter);
        }
        
        private void ToggleAssemblyModeHandler()
        {
            this.ToggleAssemblyMode();
        }

        private void EscapeHandler()
        {
            this.ToggleMainMenu();
            CursorStateController.Instance.ToggleCursor();
        }

        private void ToggleAssemblyMode(bool? enabled = null)
        {
            if (enabled == null)
            {
                enabled = !this.AssemblyModeEnabled;
            }

            this.AssemblyModeEnabled = (bool)enabled;
            GameEvents.RaiseAssemblyModeEnabled(this.AssemblyModeEnabled);
        }

        private void ToggleMainMenu(bool? isOpen = null)
        {
            if (isOpen == null)
            {
                isOpen = !this.InputController.MainMenu.activeInHierarchy;
            }

            this.InputController.MainMenu.SetActive((bool)isOpen);
        }

        IEnumerator DescroySuit()
        {
            var suitComponents = this.InputController.SuitComponents;

            foreach (var component in suitComponents)
            {
                component.SetParent(this.InputController.RootSceneTransform);
                component.GetComponent<Rigidbody>().isKinematic = false;
                component.GetComponent<Rigidbody>().useGravity = true;

                yield return new WaitForSeconds(0.5f);
            }
        }

        IEnumerator RepairSuit()
        {
            var suitComponents = this.InputController.SuitComponents;

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
    }
}
