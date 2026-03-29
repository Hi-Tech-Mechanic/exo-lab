namespace ExoLab
{
    using System;
    using ExoLab.Data;

    /// <summary>
    /// Класс описывающий абстрактный предмет
    /// </summary>
    public abstract class ItemAbstract<T> : ItemBase where T : ItemData
    {
        [NonSerialized] private string? id;
        [NonSerialized] private string? name;
        [NonSerialized] private string? description;
        [NonSerialized] private double? weight;
        [NonSerialized] private int? maxStackSize;

        /// <summary>
        /// Типизированная информация о компоненте, содержит полную инфомацию о нём
        /// </summary>
        public T TypedItemData => (T)this.itemData;

        /// <summary>
        /// Уникальный номер предмета, желательно использовать GUID
        /// </summary>
        public override string Id
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

        public override string Name
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

        public override string Description
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

        public override double Weight
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

        public override int MaxStackSize
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