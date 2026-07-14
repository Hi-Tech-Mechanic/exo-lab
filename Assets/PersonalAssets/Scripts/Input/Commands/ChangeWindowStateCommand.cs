using UnityEngine;

namespace ExoLab.Input
{
    public class ChangeWindowStateCommand : ICommand
    {
        private GameObject window;
        private bool newState;
        private bool oldState;

        public ChangeWindowStateCommand(GameObject window, bool newState)
        {
            this.window = window;
            this.newState = newState;
            this.oldState = window.activeInHierarchy;
        }

        public void Execute()
        {
            this.window.SetActive(this.newState);
        }

        public void Undo()
        {
            this.window.SetActive(this.oldState);
        }
    }
}
