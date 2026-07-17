namespace ExoLab
{
    using Assets.PersonalAssets.Scripts.Input.Helpers;
    using Assets.PersonalAssets.Scripts.Inventory.View;
    using UnityEngine;

    public class CharacterInventoryController : InventoryControllerAbstract
    {
        [SerializeField]
        private CharacterInventoryView view;
        
        protected override IInventoryView View => view;

        public override void SubscribeEvents()
        {
            base.SubscribeEvents();
            GameEvents.UserEvents.OnInventoryToggle += ToggleInventoryHandler;
        }

        public override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            GameEvents.UserEvents.OnInventoryToggle -= ToggleInventoryHandler;
        }

        private void ToggleInventoryHandler()
        {
            this.view.ToggleWindow();
            this.view.DeleteAllInfoPanels();

            CursorStateController.Instance.ToggleCursor();
        }
    }
}
