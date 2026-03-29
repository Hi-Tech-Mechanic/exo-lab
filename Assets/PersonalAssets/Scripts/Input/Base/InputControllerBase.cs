namespace ExoLab.Input
{
    /// <summary>
    /// Основа для всех обработчиков ввода
    /// </summary>
    internal abstract class InputControllerBase : ISubsribable
    {
        protected InputControllersManager InputController;

        protected InputControllerBase(InputControllersManager inputController, PlayerControls controls)
        {
            this.InputController = inputController;

            this.Init(controls);
        }

        public virtual void Init(PlayerControls controls)
        {
            this.InitBindings(controls);
        }

        protected abstract void InitBindings(PlayerControls controls);

        public abstract void SubscribeEvents();
        public abstract void UnsubscribeEvents();
    }
}
