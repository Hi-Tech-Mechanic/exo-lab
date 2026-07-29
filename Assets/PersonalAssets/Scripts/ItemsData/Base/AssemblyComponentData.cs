namespace ExoLab.Data
{
    using System.Collections.Generic;
    using System;
    using UnityEngine;

    /// <summary>
    /// Данные об абстрактном компоненте, он может быть как оружейным так и от костюма,
    /// главная особенность в данных о креплении
    /// </summary>
    [CreateAssetMenu(fileName = "ComponentData", menuName = "Inventory/Component data")]
    public class AssemblyComponentData : ItemData
    {
        [Space(5)]
        [Header("Базовая информация о компоненте")]
        [Space(5)]

        [Tooltip("Прочность")]
        public double Durability;

        [Tooltip("Из чего состоит")]
        public IMaterial.MaterialType Material;

        [Header("Комплект данных отвечающий за привязку\nк конкретному объекту")]
        public List<AttachmentOption> AttachmentOptions;

        [Header("Перечень совместимых типов компонентов и их количество")]
        public List<CompabilityComponent> CompabilityComponents;

        /// <summary>
        /// Комплект данных отвечающий за привязку к конкретному объекту
        /// </summary>
        [Serializable]
        public class AttachmentOption
        {
            [Tooltip("Данные родительского объекта\n(к которому присоединение)")]
            public AssemblyComponentData ParentData;

            [Tooltip("Координаты точки крепления\nданного объекта к родительскому")]
            public Vector3 AttachmentPoint;

            [Tooltip("Вращение данного объекта")]
            public Quaternion Rotation;

            [Tooltip("Масштаб данного объекта")]
            public Vector3 Scale = new Vector3(1F, 1F, 1F);
        }

        /// <summary>
        /// Перечень совместимых компонентов и их количество
        /// </summary>
        [Serializable]
        public class CompabilityComponent //todo пока нет, не так нужно
        {
            [Tooltip("Тип объекта")]
            public Constants.Constants.Components.ComponentTypes componentType;

            [Tooltip("Допустимое количество данного компонента")]
            public int Count;
        }

        public override List<ItemCharacteristicTypes.ItemStringCharacteristic> GetAllStats()
        {
            var result = new List<ItemCharacteristicTypes.ItemStringCharacteristic>();

            var allStats = base.GetAllStats();
            result.AddRange(allStats);

            var name = nameof(this.Material);
            var value = this.Material.ToString();
            var stat = new ItemCharacteristicTypes.ItemStringCharacteristic(name, value);
            result.Add(stat);

            foreach (var option in this.AttachmentOptions)
            {
                name = "Parent detail";
                value = option.ParentData.Name;
                stat = new ItemCharacteristicTypes.ItemStringCharacteristic(name, value);
                result.Add(stat);
            }

            return result;
        }

        public override List<ItemCharacteristicTypes.ItemStringCharacteristic> GetTranslatedAllStats()
        {
            var result = new List<ItemCharacteristicTypes.ItemStringCharacteristic>();

            var allStats = base.GetTranslatedAllStats();
            result.AddRange(allStats);

            var name = "Материал";
            var value = this.Material.ToString();
            var stat = new ItemCharacteristicTypes.ItemStringCharacteristic(name, value);
            result.Add(stat);

            foreach (var option in this.AttachmentOptions)
            {
                name = "Родительская деталь";
                value = option.ParentData.Name;
                stat = new ItemCharacteristicTypes.ItemStringCharacteristic(name, value);
                result.Add(stat);
            }

            return result;
        }

        public override List<ItemCharacteristicTypes.ItemStringCharacteristic> GetNumericStats()
        {
            var result = new List<ItemCharacteristicTypes.ItemStringCharacteristic>();

            var numericStats = base.GetNumericStats();
            result.AddRange(numericStats);

            var name = nameof(this.Durability);
            var value = this.Durability.ToString();
            var stat = new ItemCharacteristicTypes.ItemStringCharacteristic(name, value);
            result.Add(stat);

            return result;
        }

        public override List<ItemCharacteristicTypes.ItemStringCharacteristic> GetTranslatedNumericStats()
        {
            var result = new List<ItemCharacteristicTypes.ItemStringCharacteristic>();

            var translatedNumericStats = base.GetTranslatedNumericStats();
            result.AddRange(translatedNumericStats);

            var name = "Прочность";
            var value = this.Durability.ToString();
            var stat = new ItemCharacteristicTypes.ItemStringCharacteristic(name, value);
            result.Add(stat);

            return result;
        }
    }
}
