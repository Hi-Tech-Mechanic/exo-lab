namespace ExoLab.UI
{
    using Assets.PersonalAssets.Scripts.Input.Base;

    /// <summary>
    /// Визуализатор клавиш взаимодействия
    /// </summary>
    internal class InteractKeysView : InputKeysViewBase
    {
        private ControlKeyView interact;
        private ControlKeyView suitDestruction;
        private ControlKeyView suitRegeneration;
        private ControlKeyView showStats;

        private ControlKeyView EnableFirstPersonCamera;
        private ControlKeyView EnableBackCamera;
        private ControlKeyView EnableForwardCamera;

        internal InteractKeysView(InputControllerManagerView parentView) : base(parentView)
        { }

        protected override void InitKeyComponents()
        {
            if (this.Initialized)
            {
                return;
            }

            var interactionButtons = this.ParentView.InteractionButtons;

            this.SetHeaderText(interactionButtons.PageHeader, "Взаимодействие");

            this.interact = new ControlKeyView(interactionButtons.ChangeInteract, "Взять/Использовать", this.InputManager.Interaction.Interact);
            this.suitDestruction = new ControlKeyView(interactionButtons.ChangeSuitDestruction, "Разрушить экзоскелет", this.InputManager.Interaction.Keyboard_8);
            this.suitRegeneration = new ControlKeyView(interactionButtons.ChangeSuitRegeneration, "Собрать экзоскелет", this.InputManager.Interaction.Keyboard_9);
            this.showStats = new ControlKeyView(interactionButtons.ChangeShowStats, "Собрать экзоскелет", this.InputManager.Interaction.Keyboard_4);
            this.EnableFirstPersonCamera = new ControlKeyView(interactionButtons.ChangeFirstPersonCamera, "Камера от первого лица", this.InputManager.Interaction.Keyboard_1);
            this.EnableBackCamera = new ControlKeyView(interactionButtons.ChangeBackCamera, "Камера от второго лица сзади", this.InputManager.Interaction.Keyboard_2);
            this.EnableForwardCamera = new ControlKeyView(interactionButtons.ChangeForwardCamera, "Камера от второго лица спереди", this.InputManager.Interaction.Keyboard_3);

            this.standardKeys.Add(interact);
            this.standardKeys.Add(suitDestruction);
            this.standardKeys.Add(suitRegeneration);
            this.standardKeys.Add(showStats);
            this.standardKeys.Add(EnableFirstPersonCamera);
            this.standardKeys.Add(EnableBackCamera);
            this.standardKeys.Add(EnableForwardCamera);

            this.Initialized = true;
        }
    }
}
