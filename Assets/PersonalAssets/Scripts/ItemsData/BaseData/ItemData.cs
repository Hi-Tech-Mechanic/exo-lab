namespace ExoLab.Data
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
        [Tooltip("Максимальное количество в стаке")]
        public uint maxStackSize;
        public Sprite Icon;
        [Tooltip("Модель")]
        public GameObject Prefab;
    }
}
