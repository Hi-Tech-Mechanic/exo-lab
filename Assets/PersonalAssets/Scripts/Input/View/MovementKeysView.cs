namespace ExoLab.UI
{
    using Assets.PersonalAssets.Scripts.Input.Base;
    using System;

    /// <summary>
    /// Визуализатор клавиш движения
    /// </summary>
    internal class MovementKeysView : InputKeysViewBase
    {
        private enum MovememntKeys
        {   
            moveForwardId = 0,
            moveBackId = 2,
            moveLeftId = 4,
            moveRightId = 6
        }

        private ControlKeyView jump;
        private ControlKeyView sprint;
        private ControlKeyView moveForward;
        private ControlKeyView moveBack;
        private ControlKeyView moveLeft;
        private ControlKeyView moveRight;
     
        internal MovementKeysView(InputControllerManagerView parentView) : base (parentView)
        { }

        public override void SetAllButtonsInteractable(bool interactable)
        {
            base.SetAllButtonsInteractable(interactable);
        }

        public override void SetListeners()
        {
            base.SetListeners();
        }   

        public override void UpdateKeyTexts()
        {
            base.UpdateKeyTexts();

            var movement = this.InputManager.Movement;
            this.moveForward.ButtonText.text = this.InputManager.GetBindingName(movement.Move, (int)MovememntKeys.moveForwardId);
            this.moveBack.ButtonText.text = this.InputManager.GetBindingName(movement.Move, (int)MovememntKeys.moveBackId);
            this.moveLeft.ButtonText.text = this.InputManager.GetBindingName(movement.Move, (int)MovememntKeys.moveLeftId);
            this.moveRight.ButtonText.text = this.InputManager.GetBindingName(movement.Move, (int)MovememntKeys.moveRightId);
        }

        protected override void InitKeyComponents()
        {
            if (this.Initialized)
            {
                return;
            }

            this.SetHeaderText(this.ParentView.MovementButtons.PageHeader, "Передвижение");

            this.jump = new ControlKeyView(this.ParentView.MovementButtons.ChangeJump, "Прыжок", this.InputManager.Movement.Jump);
            this.sprint = new ControlKeyView(this.ParentView.MovementButtons.ChangeSprint, "Бег", this.InputManager.Movement.Sprint);

            this.moveForward = new ControlKeyView(this.ParentView.MovementButtons.ChangeMoveForward, "Вперёд");
            this.moveBack = new ControlKeyView(this.ParentView.MovementButtons.ChangeMoveBack, "Назад");
            this.moveLeft = new ControlKeyView(this.ParentView.MovementButtons.ChangeMoveLeft, "Влево");
            this.moveRight = new ControlKeyView(this.ParentView.MovementButtons.ChangeMoveRight, "Вправо");

            this.standardKeys.Add(this.jump);
            this.standardKeys.Add(this.sprint);

            this.Initialized = true;
        }

        /// <summary>
        /// Выставить переназначение клавиши на действие которое имеет множество событий
        /// </summary>
        /// <param name="keyControl"></param>
        /// <param name="id"></param>
        [Obsolete("Возможность переназначения отключена")]
        private void SetRebindListenerToMoveAction(ControlKeyView keyControl, int id)
        {
            keyControl.Button.onClick.AddListener(() => this.ParentView.StartRebindMove(id, keyControl.ButtonText));
        }
    }
}
