namespace Assets.PersonalAssets.Scripts.Inventory.View
{
    using ExoLab.UI;
    using UnityEngine;

    public class CharacterInventoryView : InventoryViewAbstract
    {
        [Tooltip("Непосредственно окно инвентаря")]
        [SerializeField]
        private GameObject window;

        public void ToggleWindow()
        {
            var state = !this.window.activeInHierarchy;
            this.window.SetActive(state);
        }
    }
}
