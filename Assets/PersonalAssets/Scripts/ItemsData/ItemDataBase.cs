using ExoLab.Data;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Реестр всех предметов
/// </summary>
[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Item Database")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] private List<ItemData> allItems = new List<ItemData>();

    // Кэш по ID
    private Dictionary<int, ItemData> itemCache;

    public ItemData GetItemById(int id)
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

    public ItemData GetItemByName(string name)
    {
        if (itemCache == null) 
            BuildCache();

        foreach (var kvp in itemCache)
        {
            if (kvp.Value.Name == name)
                return kvp.Value;
        }
        return null;
    }

    private void BuildCache()
    {
        itemCache = new Dictionary<int, ItemData>();
        foreach (var item in allItems)
        {
            if (item != null)
            {
                int id = item.GetInstanceID();
                if (!itemCache.ContainsKey(id))
                    itemCache[id] = item;
            }
        }
    }

#if UNITY_EDITOR

    //// Автоматически собрать все ItemData из проекта (только в редакторе)
    //[ContextMenu("Auto Collect All Items")]
    //public void AutoCollectItems()
    //{
    //    allItems.Clear();
    //    string[] guids = UnityEditor.AssetDatabase.FindAssets("t:ItemData");
    //    foreach (string guid in guids)
    //    {
    //        string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
    //        ItemData item = UnityEditor.AssetDatabase.LoadAssetAtPath<ItemIData>(path);
    //        if (item != null && !allItems.Contains(item))
    //            allItems.Add(item);
    //    }
    //    Debug.Log($"Collected {allItems.Amount} items into database.");
    //    UnityEditor.EditorUtility.SetDirty(this);
    //}

#endif
}