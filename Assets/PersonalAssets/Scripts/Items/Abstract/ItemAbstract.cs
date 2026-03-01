namespace ExoLab
{
    using System;
    using ExoLab.Data;
    using System.Collections.Generic;
    using ExoLab.Helpers;

    /// <summary>
    /// Класс описывающий абстрактный предмет
    /// </summary>
    public abstract class ItemAbstract<T> :
        ItemBase,
        IName,
        IDescription,
        IWeight
        where T : ItemData
    {
        private string? id;
        private string? name;
        private string? description;
        private double? weight;
        private int? maxStackSize;

        /// <summary>
        /// Типизированная информация о компоненте, содержит полную инфомацию о нём
        /// </summary>
        public T TypedItemData => (T)this.itemData;

        /// <summary>
        /// Уникальный номер предмета, желательно использовать GUID
        /// </summary>
        public virtual string Id
        {
            get
            {
                if (this.id != null)
                {
                    return this.id;
                }

                this.id = this.TypedItemData.Id;
                return this.id;
            }

            protected set
            {
                this.id = value;
            }
        }

        public virtual string Name
        {
            get
            {
                if (this.name != null)
                {
                    return this.name;
                }

                this.name = this.TypedItemData.Name;
                return this.name;
            }

            protected set
            {
                this.name = value;
            }
        }

        public virtual string Description
        {
            get
            {
                if (this.description != null)
                {
                    return this.description;
                }

                this.description = this.TypedItemData.Description;
                return this.description;
            }

            protected set
            {
                this.description = value;
            }
        }

        public virtual double Weight
        {
            get
            {
                if (this.weight != null)
                {
                    return (double)this.weight;
                }

                this.weight = this.TypedItemData.Weight;
                return (double)this.weight;
            }

            protected set
            {
                this.weight = value;
            }
        }

        public virtual int MaxStackSize
        {
            get
            {
                if (this.maxStackSize != null)
                {
                    return (int)this.maxStackSize;
                }

                this.maxStackSize = this.TypedItemData.MaxStackSize;
                return (int)this.maxStackSize;
            }

            protected set
            {
                this.maxStackSize = value;
            }
        }

        protected virtual void Start()
        {
            this.CheckItemData();
        }

        /// <summary>
        /// Обновить данные у предмета
        /// </summary>
        /// <param name="itemData"></param>
        public void SetItemData(ItemData itemData)
        {
            this.itemData = itemData;
        }

        /// <summary>
        /// Вернуть все характеристики предмета
        /// Кроме Name и Description
        /// </summary>
        /// <returns>Словарь: имя свойства - значение</returns>
        public virtual Dictionary<string, object> GetAllStats()
        {
            var result = new Dictionary<string, object>();

            result.AddRange(this.GetNumericStats());
            result[nameof(this.MaxStackSize)] = this.MaxStackSize;

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

            result.AddRange(this.GetTranslatedNumericStats());
            result["Размер стака"] = this.MaxStackSize;

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
        /// Инициализация данных предмета из ScriptableObject
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