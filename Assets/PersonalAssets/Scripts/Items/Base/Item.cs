namespace ExoLab
{
    using ExoLab.Data;
    using UnityEngine;

    /// <summary>
    /// Базовый физичный предмет в игровом пространстве
    /// </summary>
    public class Item : ItemAbstract<ItemData>
    {
        /// <summary>
        /// Префаб объекта
        /// </summary>
        public GameObject Prefab { get => this.TypedItemData.Prefab; }

    }
}
