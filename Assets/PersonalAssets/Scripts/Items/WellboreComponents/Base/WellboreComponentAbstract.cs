namespace ExoLab.StructuralСomponents.Weapon
{
    using ExoLab.Data;
    using ExoLab.Helpers;
    using System.Collections.Generic;

    /// <summary>
    /// Базовый класс для физичных компонентов ствола
    /// </summary>
    public abstract class WellboreComponentAbstract<T> : AssemblyComponentBase where T : WellboreComponentItemData
    {
        public new T TypedItemData => (T)base.itemData;

        private double? maxRotationSpeed;
        private double? maxTemperature;

        public virtual double MaxRotationSpeed
        {
            get
            {
                if (this.maxRotationSpeed != null)
                {
                    return (double)this.maxRotationSpeed;
                }

                this.maxRotationSpeed = this.TypedItemData.MaxRotationSpeed;
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
                if (this.maxTemperature != null)
                {
                    return (double)this.maxTemperature;
                }

                this.maxTemperature = this.TypedItemData.MaxTemperature;
                return (double)this.maxTemperature;
            }

            protected set
            {
                this.maxTemperature = value;
            }
        }

        public override Dictionary<string, object> GetNumericStats()
        {
            var result = new Dictionary<string, object>();

            result.AddRange(base.GetNumericStats());
            result[nameof(this.MaxRotationSpeed)] = this.MaxRotationSpeed;
            result[nameof(this.MaxTemperature)] = this.MaxTemperature;

            return result;
        }

        public override Dictionary<string, object> GetTranslatedNumericStats()
        {
            var result = new Dictionary<string, object>();

            result.AddRange(base.GetNumericStats());
            result["Макс. скорость вращения"] = this.MaxRotationSpeed;
            result["Макс. температура"] = this.MaxTemperature;

            return result;
        }
    }
}
