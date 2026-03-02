namespace ExoLab
{
    using ExoLab.Data;
    using UnityEngine;

    /// <summary>
    /// Базовый физичный предмет в игровом пространстве, 
    /// в основном данный класс нужен для приведения
    /// </summary>
    public abstract class ItemBase : MonoBehaviour
    {
        /// <summary>
        /// Поле защищено, так как для получения информации надо
        /// использовать типизированные данные <see cref="ItemAbstract{T}.TypedItemData"/>>
        /// </summary>
        [Tooltip("Ссылка на характеристики компонента (ScriptableObject)")]
        [SerializeField]
        protected ItemData itemData;

        /// <summary>
        /// Получить самый стандартный (минимальный) объем данных об объекте
        /// </summary>
        /// <returns></returns>
        public ItemData GetBaseItemData()
        {
            return this.itemData;
        }
    }
}
