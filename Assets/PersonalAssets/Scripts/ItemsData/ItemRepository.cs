namespace ExoLab
{
    using ExoLab.Data;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Реестр всех предметов
    /// </summary>
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Item Database")]
    public class ItemRepository : ScriptableObject
    {
        [Header("Перечень всех предметов")]
        [SerializeField]
        private List<ItemData> allItems = new List<ItemData>();

        /// <summary>
        /// Кэш по ID
        /// </summary>
        private Dictionary<string, ItemData> itemCache;

        public List<ItemData> GetAllItems()
        {
            return allItems;
        }

        public ItemData? GetItemById(string id)
        {
            if (itemCache == null)
            {
                BuildCache();
            }

            if (itemCache.TryGetValue(id, out ItemData item))
                return item;

            Debug.LogWarning($"Item with ID {id} not found in ItemDatabase!");
            return null;
        }

        public ItemData? GetItemByName(string name)
        {
            if (itemCache == null)
            {
                BuildCache();
            }

            foreach (var kvp in itemCache)
            {
                if (kvp.Value.Name.Equals(name))
                    return kvp.Value;
            }

            return null;
        }

        private void BuildCache()
        {
            itemCache = new Dictionary<string, ItemData>();

            foreach (var item in allItems)
            {
                if (item == null)
                    continue;

                var id = item.Id;
                if (itemCache.ContainsKey(id) == false)
                    itemCache[id] = item;
            }
        }

#if UNITY_EDITOR

        /// <summary>
        /// Автоматически собрать все <see cref="ItemData"/>> из проекта (только в редакторе)
        /// </summary>
        [ContextMenu("Auto Collect All Items")]
        public void AutoCollectItems()
        {
            allItems.Clear();
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:ItemData");
            foreach (string guid in guids)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                ItemData item = UnityEditor.AssetDatabase.LoadAssetAtPath<ItemData>(path);
                if (item != null && !allItems.Contains(item))
                    allItems.Add(item);
            }
            Debug.Log($"Collected {allItems.Count} itemsInSlots into database.");
            UnityEditor.EditorUtility.SetDirty(this);
        }

        [ContextMenu("Создать GUID для всех предметов у которых его нет")]
        public void CreateGUIDsForAllItemsIfTheyAreEmpty()
        {
            foreach (var item in allItems)
            {
                if (item.Id == null || item.Id == string.Empty)
                    item.Id = item.CreateGUID();
            }

            UnityEditor.EditorUtility.SetDirty(this);
        }

#endif
    }
}
