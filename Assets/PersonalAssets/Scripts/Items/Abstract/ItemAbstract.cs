namespace ExoLab
{
    using System;
    using ExoLab.Data;

    /// <summary>
    /// Класс описывающий абстрактный предмет
    /// </summary>
    public abstract class ItemAbstract<T> : ItemBase where T : ItemData
    {
        /// <summary>
        /// Типизированная информация о компоненте, содержит полную инфомацию о нём
        /// </summary>
        public T TypedItemData => (T)this.itemData;

        protected virtual void Start()
        {
            this.CheckItemData();
        }

        /// <summary>
        /// Инициализация данных предмета из ScriptableObject
        /// </summary>
        private void CheckItemData()
        {
            if (this.itemData == null)
            {
                throw new NullReferenceException($"Не заданы данные для предмета [{this.gameObject.name}]");
            }

            if (this.TypedItemData == null)
            {
                throw new NullReferenceException($"Не типизированы данные для предмета [{this.gameObject.name}]");
            }
        }
    }
}