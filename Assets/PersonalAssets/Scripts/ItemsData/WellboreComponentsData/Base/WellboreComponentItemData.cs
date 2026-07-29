namespace ExoLab.Data
{
    using UnityEngine;
    using System.Collections.Generic;
    using UnityEngine.Serialization;

    /// <summary>
    /// Необходимые состовляющие для корректной работы каждого компонента оборудования ствола скважины
    /// </summary>
    [CreateAssetMenu(fileName = "WellboreComponentItemData", menuName = "Inventory/Wellbore component data")]
    public class WellboreComponentItemData : AssemblyComponentData
    {
        [Tooltip("Максимальная скорость вращения")]
        [FormerlySerializedAs("MaxRotationSpeed")]
        public double maxRotationSpeed;

        [Tooltip("Максимальная температура")]
        [FormerlySerializedAs("MaxTemperature")]
        public double maxTemperature;

        public virtual double MaxRotationSpeed
        {
            get
            {
                if (this.maxRotationSpeed != 0)
                {
                    return (double)this.maxRotationSpeed;
                }

                this.maxRotationSpeed = this.MaxRotationSpeed;
                return (double)this.maxRotationSpeed;
            }

            protected set
            {
                this.maxRotationSpeed = value;
            }
        }

        public virtual double MaxTemperature
        {
            get
            {
                if (this.maxTemperature != 0)
                {
                    return (double)this.maxTemperature;
                }

                this.maxTemperature = this.MaxTemperature;
                return (double)this.maxTemperature;
            }

            protected set
            {
                this.maxTemperature = value;
            }
        }

        public override List<ItemCharacteristicTypes.ItemStringCharacteristic> GetNumericStats()
        {
            var result = new List<ItemCharacteristicTypes.ItemStringCharacteristic>();

            result.AddRange(base.GetNumericStats());

            var stat = new ItemCharacteristicTypes.ItemStringCharacteristic(nameof(this.MaxRotationSpeed), this.MaxRotationSpeed.ToString());
            result.Add(stat);

            stat = new ItemCharacteristicTypes.ItemStringCharacteristic(nameof(this.MaxTemperature), this.MaxTemperature.ToString());
            result.Add(stat);

            return result;
        }

        public override List<ItemCharacteristicTypes.ItemStringCharacteristic> GetTranslatedNumericStats()
        {
            var result = new List<ItemCharacteristicTypes.ItemStringCharacteristic>();

            result.AddRange(base.GetNumericStats());

            var stat = new ItemCharacteristicTypes.ItemStringCharacteristic("Макс. скорость вращения", this.MaxRotationSpeed.ToString());
            result.Add(stat);

            stat = new ItemCharacteristicTypes.ItemStringCharacteristic("Макс. температура", this.MaxTemperature.ToString());
            result.Add(stat);

            return result;
        }
    }
}
