namespace ExoLab.Data
{
    using System.Collections.Generic;
    using UnityEngine;

    public interface IItemData
    {
        public string Id { get; set; }

        public string Name { get; }

        public string Description { get; }

        public WeightProperty Weight { get; }

        public MaxStackSizeProperty MaxStackSize { get; }

        public List<IStatistic> Characteristics { get; }

        public List<ITypedStatistic<double>> NumericalCharacteristics { get; }

        public Sprite Icon { get; }

        public GameObject Prefab { get; }

        public void SetName(string value);

        public void SetDescription(string value);

        public void SetWeight(double value);

        public void SetMaxStackSize(int value);
    }
}
