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
        /// 3D-модель объекта
        /// </summary>
        public GameObject object3dModel { get; protected set; }
    }
}
