namespace ExoLab
{
    using ExoLab.Data;
    using UnityEngine;

    /// <summary>
    /// Базовый физичный предмет в игровом пространстве, 
    /// в основном данный класс нужен для приведения
    /// </summary>
    public class ItemBase : MonoBehaviour
    {
        /// <summary>
        /// Поле защищено, так как для получения информации надо
        /// использовать типизированные данные <see cref="ItemAbstract{T}.TypedItemData"/>>
        /// </summary>
        [Tooltip("Ссылка на характеристики компонента (ScriptableObject)")]
        [SerializeField] protected ItemData itemData;

        public IItemData ItemData
        {
            get
            {
                if (this.itemData == null)
                {
                    this.itemData = new ItemData();
                }

                return this.itemData;
            }
        }

        /// <summary>
        /// Обновить данные у предмета
        /// </summary>
        /// <param name="itemData"></param>
        public void SetItemData(IItemData itemData)
        {
            this.itemData = (ItemData)itemData;
        }
    }
}
