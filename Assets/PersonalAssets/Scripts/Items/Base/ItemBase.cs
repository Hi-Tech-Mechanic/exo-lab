namespace ExoLab
{
    using ExoLab.Data;
    using ExoLab.Helpers;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Базовый физичный предмет в игровом пространстве, 
    /// в основном данный класс нужен для приведения
    /// </summary>
    public class ItemBase : 
        MonoBehaviour,
        IName,
        IDescription,
        IWeight
    {
        /// <summary>
        /// Поле защищено, так как для получения информации надо
        /// использовать типизированные данные <see cref="ItemAbstract{T}.TypedItemData"/>>
        /// </summary>
        [Tooltip("Ссылка на характеристики компонента (ScriptableObject)")]
        [SerializeField]
        protected ItemData itemData;

        [NonSerialized] private string? id;
        [NonSerialized] private string? name;
        [NonSerialized] private string? description;
        [NonSerialized] private double? weight;
        [NonSerialized] private int? maxStackSize;

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

                this.id = this.itemData.Id;
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

                this.name = this.itemData.Name;
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

                this.description = this.itemData.Description;
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

                this.weight = this.itemData.Weight;
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

                this.maxStackSize = this.itemData.MaxStackSize;
                return (int)this.maxStackSize;
            }

            protected set
            {
                this.maxStackSize = value;
            }
        }

        public virtual GameObject Prefab
        {
            get => this.itemData.Prefab;
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
        /// Получить самый стандартный (минимальный) объем данных об объекте
        /// </summary>
        /// <returns></returns>
        public ItemData GetBaseItemData()
        {
            return this.itemData;
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
    }
}
