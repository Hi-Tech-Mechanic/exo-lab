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
        public GameObject Prefab { get; protected set; }

        protected override void InitializeItemData()
        {
            base.InitializeItemData();
            this.Prefab = this.TypedItemData.Prefab;
        }
    }
}
