namespace ExoLab
{
    using Assets.PersonalAssets.Scripts.Inventory.View;
    using UnityEngine;

    public class AssemblyInventoryController : InventoryControllerAbstract<AssemblyInventoryModel>
    {
        [SerializeField]
        private AssemblyInventoryView view;
        
        protected override IInventoryView View => view;

        protected override AssemblyInventoryModel InitInventoryModel()
        {
            var model = new AssemblyInventoryModel(this.ItemRepository);
            return model;
        }
    }
}
