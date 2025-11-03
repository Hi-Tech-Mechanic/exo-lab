namespace Assets.PersonalAssets.Scripts.SuitComponents.ScriptableObjects
{
    using UnityEngine;

    /// <summary>
    /// Базовое хранилище данных для любого предмета
    /// </summary>
    [CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/Item data")]
    public class ItemData : ScriptableObject
    {
        public string Name;
        public string Description;
        public double Weight;
    }
}
