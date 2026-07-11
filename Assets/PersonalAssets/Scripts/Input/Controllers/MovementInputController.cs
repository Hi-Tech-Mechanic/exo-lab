namespace ExoLab.Input
{
    using System;
    using UnityEngine;
    using UnityEngine.InputSystem;

    internal class MovementInputController : InputControllerBase
    {
        private Vector2 moveValue;

        public InputAction Jump { get; private set; }
        public InputAction Move { get; private set; }
        public InputAction Sprint { get; private set; }

        public event Action OnJumpStarted;
        public event Action OnJumpCanceled;

        public event Action OnMove;

        public event Action OnSprint;

        internal MovementInputController(InputControllersManager inputController, PlayerControls controls) 
            : base (inputController, controls) { }

        protected override void InitBindings(PlayerControls controls)
        {
            this.Jump = controls.Player.Jump;
            this.Move = controls.Player.Move;
            this.Sprint = controls.Player.Sprint;
        }

        public override void SubscribeEvents()
        {
            this.Jump.started += ctx => this.OnJumpStarted?.Invoke();
            this.Jump.canceled += ctx => this.OnJumpCanceled?.Invoke();

            this.Move.performed += ctx => this.moveValue = ctx.ReadValue<Vector2>();
            this.Move.canceled += ctx => this.moveValue = Vector2.zero;

            this.Sprint.performed += ctx => this.OnSprint?.Invoke();
        }

        public override void UnsubscribeEvents()
        {
            this.Jump.started -= ctx => this.OnJumpStarted?.Invoke();
            this.Jump.canceled -= ctx => this.OnJumpCanceled?.Invoke();
        }

        public Vector2 GetMoveVector() => this.moveValue;
    }
}
