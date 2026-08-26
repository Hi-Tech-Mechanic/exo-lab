namespace ExoLab.Data
{
    using ExoLab.Helpers;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Serialization;

    /// <summary>
    /// Базовое хранилище данных для любого предмета
    /// </summary>
    [CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/Item data")]
    public class ItemData : ScriptableObject, IItemData
    {
        [Header("Base information about item")]
        [Space(5)]
        [Tooltip("Identification number - GUID")]
        [FormerlySerializedAs("Id")]
        [SerializeField] private string id;
        [FormerlySerializedAs("Name")]
        [SerializeField] private string itemName;
        [FormerlySerializedAs("Description")]
        [SerializeField] private string description;
        [FormerlySerializedAs("Weight")]
        [SerializeField] private double weight;
        [FormerlySerializedAs("MaxStackSize")]
        [SerializeField] private int maxStackSize;
        [FormerlySerializedAs("Icon")]
        [SerializeField] private Sprite icon;
        [FormerlySerializedAs("Prefab")]
        [SerializeField] private GameObject prefab;

        private WeightProperty? weightProperty;
        private MaxStackSizeProperty? maxStackSizeProperty;
        private List<IStatistic> characteristics;

        public string Id { get { return this.id; } set { this.id = value; } }

        public string Name => this.itemName;

        public string Description => this.description;

        public WeightProperty Weight
        {
            get
            {
                if (this.weightProperty == null)
                {
                    this.weightProperty = new WeightProperty();
                    this.weightProperty.Value = this.weight;
                }

                return this.weightProperty;
            }
        }

        public MaxStackSizeProperty MaxStackSize
        {
            get
            {
                if (this.maxStackSizeProperty == null)
                {
                    this.maxStackSizeProperty = new MaxStackSizeProperty();
                    this.maxStackSizeProperty.Value = this.maxStackSize;
                }

                return this.maxStackSizeProperty;
            }
        }

        public Sprite Icon => this.icon;

        public GameObject Prefab => this.prefab;

        public virtual List<IStatistic> Characteristics
        {
            get
            {
                if (this.characteristics == null || this.characteristics.Count == 0)
                {
                    this.characteristics = new()
                    {
                        Weight,
                        MaxStackSize
                    };
                }
                
                return this.characteristics;
            }
        }

        /// <summary>
        /// Returns all numerical characteristics as boxed <see cref="object"/> references.
        /// Use <see cref="NumericValueExtensions"/> to extract values.
        /// </summary>
        public virtual List<NumericalProperty> NumericalCharacteristics
        {
            get
            {
                var result = new List<NumericalProperty>();

                var weightProperty = new NumericalProperty(
                    this.Weight.Name, 
                    this.Weight.Value,
                    this.Weight.Type,
                    this.Weight.UnitOfMeasurement);

                var maxStackSizeProperty = new NumericalProperty(
                    this.MaxStackSize.Name,
                    this.MaxStackSize.Value,
                    this.MaxStackSize.Type,
                    this.MaxStackSize.UnitOfMeasurement);

                result.Add(weightProperty);
                result.Add(maxStackSizeProperty);

                return result;
            }
        }

        public ItemData()
        {
            this.id = IdentificationGenerator.CreateGUID();
        }

        public ItemData(string name, string description, double weight,
            int maxStackSize, Sprite? icon, GameObject prefab)
        {
            this.id = IdentificationGenerator.CreateGUID();

            this.itemName = name;
            this.description = description;
            this.weight = weight;
            this.maxStackSize = maxStackSize;   
            this.icon = icon;   
            this.prefab = prefab;
        }

        public void SetName(string value)
        {
            this.itemName = value;
        }

        public void SetDescription(string value)
        {
            this.description = value;
        }

        public void SetWeight(double value)
        {
            this.weight = value;
        }

        public void SetMaxStackSize(int value)
        {
            this.maxStackSize = value;
        }

        public void SetIcon(Sprite value)
        {
            this.icon = value;
        }

        public void SetPrefab(GameObject value)
        {
            this.prefab = value;
        }

#if UNITY_EDITOR

        [Tooltip("Создать GUID для объекта если таковой не задан")]
        [ContextMenu("Create GUID")]
        public void SetItemGuidIfNotExist()
        {
            if (this.id == null || this.id == string.Empty)
                return;

            this.id = IdentificationGenerator.CreateGUID();
            UnityEditor.EditorUtility.SetDirty(this);
        }

#endif

    }
}
