namespace ExoLab.Input
{
    using DG.Tweening;
    using ExoLab.Constants;
    using ExoLab.Notifications;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.InputSystem;

    /// <summary>
    /// Controller only for interaction events
    /// </summary>
    internal class InteractionInputController : InputControllerBase
    {
        private List<Vector3> targetPosition = new();
        private List<Quaternion> targetEulerAngles = new();
        private List<Transform> parentsTransforms = new();

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

        public static event Action OnInteractPressed;
        public static event Action OnEscapePressed;
        public static event Action OnAssemblyModePressed;
        public static event Action OnPressedKeyboard_1;
        public static event Action OnPressedKeyboard_2;
        public static event Action OnPressedKeyboard_3;
        public static event Action OnPressedKeyboard_4;
        public static event Action OnPressedKeyboard_5;
        public static event Action OnPressedKeyboard_6;
        public static event Action OnPressedKeyboard_7;
        public static event Action OnPressedKeyboard_8;
        public static event Action OnPressedKeyboard_9;

        internal InteractionInputController(InputControllersManager inputController, PlayerControls controls)
            : base(inputController, controls) { }

        public override void Init(PlayerControls controls)
        {
            base.Init(controls);
            this.InitComponents();
        }

        public override void SubscribeEvents()
        {
            this.Interact.performed += ctx => OnInteractPressed?.Invoke();
            this.AssemblyMode.performed += ctx => OnAssemblyModePressed?.Invoke();
            this.Escape.performed += ctx => OnEscapePressed?.Invoke();
            this.Inventory.performed += ctx => GameEvents.UserEvents.RaiseInventoryToggle();

            this.Keyboard_1.performed += ctx => OnPressedKeyboard_1?.Invoke();
            this.Keyboard_2.performed += ctx => OnPressedKeyboard_2?.Invoke();
            this.Keyboard_3.performed += ctx => OnPressedKeyboard_3?.Invoke();
            this.Keyboard_4.performed += ctx => OnPressedKeyboard_4?.Invoke();
            this.Keyboard_5.performed += ctx => OnPressedKeyboard_5?.Invoke();
            this.Keyboard_6.performed += ctx => OnPressedKeyboard_6?.Invoke();
            this.Keyboard_7.performed += ctx => OnPressedKeyboard_7?.Invoke();
            this.Keyboard_8.performed += ctx => OnPressedKeyboard_8?.Invoke();
            this.Keyboard_9.performed += ctx => OnPressedKeyboard_9?.Invoke();

            OnPressedKeyboard_4 += this.ShowStats;
            OnPressedKeyboard_8 += this.InvokeSuitDestruction;
            OnPressedKeyboard_9 += this.InvokeSuitRegenerate;
        }

        public override void UnsubscribeEvents()
        {
            this.Interact.performed -= ctx => OnInteractPressed?.Invoke();
            this.AssemblyMode.performed -= ctx => OnAssemblyModePressed?.Invoke();
            this.Escape.performed -= ctx => OnEscapePressed?.Invoke();
            this.Inventory.performed -= ctx => GameEvents.UserEvents.RaiseInventoryToggle();

            this.Keyboard_1.performed -= ctx => OnPressedKeyboard_1?.Invoke();
            this.Keyboard_2.performed -= ctx => OnPressedKeyboard_2?.Invoke();
            this.Keyboard_3.performed -= ctx => OnPressedKeyboard_3?.Invoke();
            this.Keyboard_4.performed -= ctx => OnPressedKeyboard_4?.Invoke();
            this.Keyboard_5.performed -= ctx => OnPressedKeyboard_5?.Invoke();
            this.Keyboard_6.performed -= ctx => OnPressedKeyboard_6?.Invoke();
            this.Keyboard_7.performed -= ctx => OnPressedKeyboard_7?.Invoke();
            this.Keyboard_8.performed -= ctx => OnPressedKeyboard_8?.Invoke();
            this.Keyboard_9.performed -= ctx => OnPressedKeyboard_9?.Invoke();

            OnPressedKeyboard_4 -= this.ShowStats;
            OnPressedKeyboard_8 -= this.InvokeSuitDestruction;
            OnPressedKeyboard_9 -= this.InvokeSuitRegenerate;
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

        private void ShowStats()
        {
            this.InputController.PlayerArmature.SetActive(!this.InputController.stats.activeInHierarchy);
        }

        private void InvokeSuitDestruction()
        {
            IEnumerator c = DescroySuit();
            this.InputController.StartCoroutine(c);
            NotificationController.Instance.ShowCritical("Разрушение экзоскелета", "Запущено разрушение экзоскелета");
        }

        private void InvokeSuitRegenerate()
        {
            IEnumerator c = RepairSuit();
            this.InputController.StartCoroutine(c);
            NotificationController.Instance.ShowWarning("Регенерация экзоскелета", "Запущена регенерация экзоскелета"); //todo поменять на info
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
