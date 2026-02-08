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
        [Tooltip("Идентификационный номер - GUID")]
        public string Id;
        [Tooltip("Имя")]
        public string Name;
        [Tooltip("Описание")]
        public string Description;
        [Tooltip("Вес")]
        public double Weight;
        [Tooltip("Максимальное количество в стаке")]
        public int MaxStackSize;
        public Sprite Icon;
        [Tooltip("Модель")]
        public GameObject Prefab;

        public ItemData()
        {
            this.Id = this.CreateGUID();
        }

        public string CreateGUID()
        {
            return System.Guid.NewGuid().ToString();
        }

#if UNITY_EDITOR

        /// <summary>
        /// Автоматически собрать все <see cref="ItemData"/>> из проекта (только в редакторе)
        /// </summary>
        [ContextMenu("Create GUID")]
        public void AutoCollectItems()
        {
            this.Id = CreateGUID();
            UnityEditor.EditorUtility.SetDirty(this);
        }

#endif

    }
}
