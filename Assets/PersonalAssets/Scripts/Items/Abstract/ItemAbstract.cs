namespace ExoLab
{
    using System;
    using UnityEngine;
    using ExoLab.Data;
    using System.Collections.Generic;
    using ExoLab.Helpers;

    /// <summary>
    /// Класс описывающий абстрактный предмет
    /// </summary>
    public abstract class ItemAbstract<T> :
        MonoBehaviour,
        IName,
        IDescription,
        IWeight
        where T : ItemData
    {
        /// <summary>
        /// Поле скрыто, так как для получения информации надо использовать <see cref="TypedItemData"/>>
        /// </summary>
        [Tooltip("Ссылка на характеристики компонента (ScriptableObject)")]
        [SerializeField]
        protected ItemData itemData;

        /// <summary>
        /// Типизированная информация о компоненте, содержит полную инфомацию о нём
        /// </summary>
        public T TypedItemData => (T)this.itemData;

        public virtual string Name { get; protected set; }

        public virtual string Description { get; protected set; }

        public virtual double Weight { get; protected set; }

        public virtual int maxStackSize { get; protected set; }

        protected virtual void Awake()
        {
            this.CheckItemData();
            this.InitializeItemData();
        }

        /// <summary>
        /// Обновить данные у предмета
        /// </summary>
        /// <param name="itemData"></param>
        public void SetItemData(ItemData itemData)
        {
            this.itemData = itemData;
            this.InitializeItemData();
        }

        /// <summary>
        /// Вернуть все характеристики предмета
        /// </summary>
        /// <returns>Словарь: имя свойства - значение</returns>
        public virtual Dictionary<string, object> GetAllStats()
        {
            var result = new Dictionary<string, object>();

            result[nameof(this.Name)] = this.Name;
            result[nameof(this.Description)] = this.Description;
            result[nameof(this.maxStackSize)] = this.maxStackSize;
            result.AddRange(this.GetNumericStats());

            return result;
        }

        /// <summary>
        /// Тоже самое что <see cref="GetAllStats"/>,
        /// но названия характеристик переведены
        /// </summary>
        /// <returns>Словарь: имя свойства - значение</returns>
        public virtual Dictionary<string, object> GetTranslatedAllStats()
        {
            var result = new Dictionary<string, object>();

            result["Имя"] = this.Name;
            result["Описание"] = this.Description;
            result["Размер максимального стака"] = this.maxStackSize;
            result.AddRange(this.GetTranslatedNumericStats());

            return result;
        }

        /// <summary>
        /// Получить только числовые характеристики
        /// </summary>
        /// <param name="names"></param>
        /// <returns>Словарь: имя свойства - значение</returns>
        public virtual Dictionary<string, object> GetNumericStats()
        {
            var result = new Dictionary<string, object>();

            result[nameof(this.Weight)] = this.Weight;

            return result;
        }

        /// <summary>
        /// Тоже самое что <see cref="GetNumericStats"/>,
        /// но названия характеристик переведены
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, object> GetTranslatedNumericStats()
        {
            var result = new Dictionary<string, object>();

            result["Вес"] = this.Weight;

            return result;
        }

        /// <summary>
        /// Инициализация полей из полученного ScriptableObject
        /// </summary>
        protected virtual void InitializeItemData()
        {
            this.Name = this.TypedItemData.Name;
            this.Description = this.TypedItemData.Description;
            this.Weight = this.TypedItemData.Weight;
            this.maxStackSize = this.maxStackSize;
        }

        /// <summary>
        /// Инициализация данных предмета из ScriptableObject. Ищет по переданному пути из папки Resources
        /// </summary>
        private void CheckItemData()
        {
            if (this.itemData == null)
                throw new NullReferenceException($"Не заданы данные для предмета [{this.gameObject.name}]");

            if (this.TypedItemData == null)
                throw new NullReferenceException($"Не типизированы данные для предмета [{this.gameObject.name}]");
        }
    }
}