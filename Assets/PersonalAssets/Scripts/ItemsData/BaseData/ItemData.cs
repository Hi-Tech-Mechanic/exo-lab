namespace Assets.PersonalAssets.Scripts.SuitComponents.ScriptableObjects
{
    using UnityEngine;

    /// <summary>
    /// Базовое хранилище данных для любого предмета
    /// </summary>
    [CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/Item data")]
    public class ItemData : ScriptableObject
    {
        [Header("Базовая информация о предмете")]
        [Space(5)]
        [Tooltip("Имя")]
        public string Name;
        [Tooltip("Описание")]
        public string Description;
        [Tooltip("Вес")]
        public double Weight;
        [Tooltip("Иконка")]
        public Sprite Icon;
    }
}
