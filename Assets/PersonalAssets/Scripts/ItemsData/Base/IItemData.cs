namespace ExoLab.Data
{
    using System.Collections.Generic;
    using UnityEngine;

    public interface IItemData
    {
        public string Id { get; set; }

        public string Name { get; }

        public string Description { get; }

        public double Weight { get; }

        public int MaxStackSize { get; }

        public List<ItemCharacteristicTypes.ItemStringCharacteristic> Characteristics { get; }

        public Sprite Icon { get; }

        public GameObject Prefab { get; }

        public List<ItemCharacteristicTypes.ItemStringCharacteristic> GetAllStats();
        
        public List<ItemCharacteristicTypes.ItemStringCharacteristic> GetTranslatedAllStats();

        public List<ItemCharacteristicTypes.ItemStringCharacteristic> GetNumericStats();

        public List<ItemCharacteristicTypes.ItemStringCharacteristic> GetTranslatedNumericStats();

        public void SetName(string value);

        public void SetDescription(string value);

        public void SetWeight(double value);

        public void SetMaxStackSize(int value);
    }
}
