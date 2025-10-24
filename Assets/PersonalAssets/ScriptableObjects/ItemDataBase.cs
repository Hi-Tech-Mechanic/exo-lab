namespace Assets.PersonalAssets.Scripts.SuitComponents.ScriptableObjects
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "ItemDataBase", menuName = "Inventory/Item data base")]
    public class ItemDataBase : ScriptableObject
    {
        public string Name;
        public string Description;
        public double Durability;
        public double Weight;
        public IMaterial.MaterialType Material;
    }
}
