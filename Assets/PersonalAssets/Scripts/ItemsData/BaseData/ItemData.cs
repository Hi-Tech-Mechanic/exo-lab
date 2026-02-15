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

        [Tooltip("Создать GUID для объекта если таковой не задан")]
        [ContextMenu("Create GUID")]
        public void SetItemGuidIfNotExist()
        {
            if (this.Id == null || this.Id == string.Empty)
                return;

            this.Id = CreateGUID();
            UnityEditor.EditorUtility.SetDirty(this);
        }

#endif

    }
}
