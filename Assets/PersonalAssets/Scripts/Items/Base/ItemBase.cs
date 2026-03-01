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
        /// Делать return в переопределении если не надо подбирать предмет
        /// </summary>
        public virtual void Pickup()
        {
            GameEvents.RaiseItemCollected(this.itemData);
            Destroy(this.gameObject);
        }
    }
}
