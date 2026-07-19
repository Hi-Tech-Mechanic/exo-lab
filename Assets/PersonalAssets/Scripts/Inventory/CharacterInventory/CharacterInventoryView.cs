namespace Assets.PersonalAssets.Scripts.Inventory.View
{
    using ExoLab.UI;

    public class CharacterInventoryView : InventoryViewAbstract
    {
        public void ToggleWindow()
        {
            var state = !this.Window.activeInHierarchy;
            this.Window.SetActive(state);
        }
    }
}
