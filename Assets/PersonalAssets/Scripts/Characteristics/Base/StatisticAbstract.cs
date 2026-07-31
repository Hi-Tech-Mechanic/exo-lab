namespace ExoLab
{
    using ExoLab.Data;
    using ExoLab.Localization;

    public abstract class StatisticAbstract<T> : ITypedStatistic<T>
    {
        /// <summary>
        /// Глобальный источник локализации для всех характеристик.
        /// Устанавливается один раз при инициализации игры.
        /// </summary>
        public static CharacteristicLocalization Localization => Caches.Instance.Items.CharacteristicLocalization;

        public abstract CharacteristicTypes.Types Type { get; }

        public string Name
        {
            get
            {
                if (Localization != null)
                {
                    string name = Localization.GetName(this.GetType(), Environment.CurrentLanguage);

                    if (string.IsNullOrEmpty(name) == false)
                    {
                        return name;
                    }
                }

                return this.GetType().Name;
            }
        }

        public virtual T Value { get; set; }

        public string FullFormattedValue
        {
            get
            {
                return $"{this.Name}: {this.Value} {this.UnitOfMeasurement}";
            }
        }

        public virtual string UnitOfMeasurement
        {
            get
            {
                if (Localization != null)
                {
                    return Localization.GetUnit(this.GetType(), Environment.CurrentLanguage);
                }

                return string.Empty;
            }
        }
    }
}
