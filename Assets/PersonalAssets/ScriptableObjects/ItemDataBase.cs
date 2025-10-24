namespace Assets.PersonalAssets.Scripts.SuitComponents.ScriptableObjects
{
    using UnityEngine;

    /// <summary>
    /// Базовое хранилище данных для любого компонента
    /// </summary>
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
