namespace ExoLab.Data
{
    using ExoLab.Helpers;
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

        public string Id { get { return this.id; } set { this.id = value; } }

        public string Name => this.itemName;

        public string Description => this.description;

        public double Weight => this.weight;

        public int MaxStackSize => this.maxStackSize;

        public Sprite Icon => this.icon;

        public GameObject Prefab => this.prefab;

        public List<ItemCharacteristicTypes.ItemStringCharacteristic> Characteristics { get; } = new();

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

        /// <summary>
        /// Вернуть все характеристики предмета
        /// Кроме Name и Description
        /// </summary>
        /// <returns>Словарь: имя свойства - значение</returns>
        public virtual List<ItemCharacteristicTypes.ItemStringCharacteristic> GetAllStats()
        {
            var result = new List<ItemCharacteristicTypes.ItemStringCharacteristic>();

            var numericStats = this.GetNumericStats();
            result.AddRange(numericStats);

            result.AddRange(this.Characteristics);

            var name = "Max stack size";
            var value = $"{this.MaxStackSize} pcs.";
            var stat = new ItemCharacteristicTypes.ItemStringCharacteristic(name, value);
            result.Add(stat); 

            return result;
        }

        /// <summary>
        /// Тоже самое что <see cref="GetAllStats"/>,
        /// но названия характеристик переведены
        /// </summary>
        /// <returns>Словарь: имя свойства - значение</returns>
        public virtual List<ItemCharacteristicTypes.ItemStringCharacteristic> GetTranslatedAllStats()
        {
            var result = new List<ItemCharacteristicTypes.ItemStringCharacteristic>();

            var numericStats = this.GetTranslatedNumericStats();
            result.AddRange(numericStats);

            result.AddRange(this.Characteristics);

            var name = "Размер стака";
            var value = $"{this.MaxStackSize} шт.";
            var stat = new ItemCharacteristicTypes.ItemStringCharacteristic(name, value);
            result.Add(stat);

            return result;
        }

        /// <summary>
        /// Получить только числовые характеристики
        /// </summary>
        /// <param name="names"></param>
        /// <returns>Словарь: имя свойства - значение</returns>
        public virtual List<ItemCharacteristicTypes.ItemStringCharacteristic> GetNumericStats()
        {
            var result = new List<ItemCharacteristicTypes.ItemStringCharacteristic>();

            var name = nameof(this.Weight);
            var value = $"{this.Weight} kg.";
            var stat = new ItemCharacteristicTypes.ItemStringCharacteristic(name, value);
            result.Add(stat);

            return result;
        }

        /// <summary>
        /// Тоже самое что <see cref="GetNumericStats"/>,
        /// но названия характеристик переведены
        /// </summary>
        /// <returns></returns>
        public virtual List<ItemCharacteristicTypes.ItemStringCharacteristic> GetTranslatedNumericStats()
        {
            var result = new List<ItemCharacteristicTypes.ItemStringCharacteristic>();

            var name = "Вес";
            var value = $"{this.Weight} кг.";
            var stat = new ItemCharacteristicTypes.ItemStringCharacteristic(name, value);
            result.Add(stat);

            return result;
        }

        public void SetName(string value)
        {
            this.name = value;
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
