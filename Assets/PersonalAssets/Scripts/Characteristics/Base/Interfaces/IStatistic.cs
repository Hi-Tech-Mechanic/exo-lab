public interface IStatistic
{
    public string Name { get; }

    public CharacteristicTypes.Types Type { get; }

    public string UnitOfMeasurement { get; }

    /// <summary>
    /// Get formatted text: name with value and with unit of measurement
    /// </summary>
    public string FullFormattedValue { get; }
}
