namespace ExoLab.Input
{
    using Assets.PersonalAssets.Scripts.Input.Helpers;
    using UnityEngine;

    internal class AssemblyMenuInput : MonoBehaviour, ISubsribable
    {
        [SerializeField] private MainMenuInput mainMenu;

        [SerializeField] private bool _assemblyModeEnabled = false;
        [Space(5)]

        [SerializeField] private GameObject assemblyWindow;
        [SerializeField] private GameObject inventoryWindow;
        [SerializeField] private GameObject assemblyProps;

        private void OnEnable()
        {
            this.SubscribeEvents();
        }

        private void OnDisable()
        {
            this.UnsubscribeEvents();
        }

        public void SubscribeEvents()
        {
            InteractionInputController.OnInventoryPressed += this.ToggleInventoryHandler;   
            InteractionInputController.OnAssemblyModePressed += this.ToggleAssemblyModeHandler;
        }

        public void UnsubscribeEvents()
        {
            InteractionInputController.OnInventoryPressed -= this.ToggleInventoryHandler;
            InteractionInputController.OnAssemblyModePressed -= this.ToggleAssemblyModeHandler;
        }

        private void ToggleInventoryHandler()
        {
            this.ToggleInventory();
        }

        private void ToggleInventory()
        {
            var state = !this.inventoryWindow.activeInHierarchy;
            this.inventoryWindow.SetActive(state);
        }

        private void ToggleAssemblyModeHandler()
        {
            this.ToggleAssemblyMode();
        }

        private void ToggleAssemblyMode()
        {
            if (this.mainMenu.MainMenuIsOpen)
            {
                return;
            }

            var state = !this._assemblyModeEnabled;
            this._assemblyModeEnabled = state;

            InputControllersManager.Instance.PlayerArmature.SetActive(!state);
            CamerasInput.Instance.ActiveCamera.gameObject.SetActive(!state);
            CursorStateController.Instance.ToggleCursor(state);

            this.inventoryWindow.SetActive(state);
            this.assemblyWindow.SetActive(state);
            this.assemblyProps.SetActive(state);

            GameEvents.RaiseAssemblyModeEnabled(state);
        }
    }
}
