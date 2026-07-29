namespace ExoLab.Data
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Serialization;

    /// <summary>
    /// Необходимые состовляющие для корректной работы каждого компонента оборудования ствола скважины
    /// </summary>
    [CreateAssetMenu(fileName = "WellboreComponentItemData", menuName = "Inventory/Wellbore component data")]
    public class WellboreComponentItemData : AssemblyComponentData
    {
        [Tooltip("Максимальная скорость вращения")]
        [FormerlySerializedAs("MaxRotationSpeed")]
        [SerializeField] private double maxRotationSpeed;

        [Tooltip("Максимальная температура")]
        [FormerlySerializedAs("MaxTemperature")]
        [SerializeField] private double maxTemperature;

        private MaxRotationSpeedProperty maxRotationSpeedProperty;
        private MaxTemperatureProperty maxTemperatureProperty;

        public MaxRotationSpeedProperty MaxRotationSpeedProperty
        {
            get
            {
                if (this.maxRotationSpeedProperty == null)
                {
                    this.maxRotationSpeedProperty = new MaxRotationSpeedProperty();
                    this.maxRotationSpeedProperty.Value = this.maxRotationSpeed;
                }

                return this.maxRotationSpeedProperty;
            }
        }

        public MaxTemperatureProperty MaxTemperature
        {
            get
            {
                if (this.maxTemperatureProperty == null)
                {
                    this.maxTemperatureProperty = new MaxTemperatureProperty();
                    this.maxTemperatureProperty.Value = this.maxTemperature;

                }

                return this.maxTemperatureProperty;
            }
        }

        public override List<IStatistic> Characteristics
        {
            get
            {
                var result = new List<IStatistic>();

                result.AddRange(base.Characteristics);
                result.Add(this.MaxRotationSpeedProperty);
                result.Add(this.MaxTemperature);

                return result;
            }
        }
    }
}
