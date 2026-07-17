namespace ExoLab
{
    using Assets.PersonalAssets.Scripts.Inventory.View;
    using UnityEngine;

    public class AssemblyInventoryController : InventoryControllerAbstract
    {
        [SerializeField]
        private AssemblyInventoryView view;
        
        protected override IInventoryView View => view;
    }
}
