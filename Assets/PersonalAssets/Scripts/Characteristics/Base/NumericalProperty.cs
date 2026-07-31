/// <summary>
/// Wrapper for numerical properties
/// </summary>
public class NumericalProperty : ITypedStatistic<double>
{
    public double Value { get ; set; }

    public string Name { get; protected set; }

    public string UnitOfMeasurement { get; protected set; }

    public string FullFormattedValue
    {
        get
        {
            return $"{this.Name}: {this.Value} {this.UnitOfMeasurement}";
        }
    }

    public CharacteristicTypes.Types Type { get; protected set; }

    public NumericalProperty(string name, double value, CharacteristicTypes.Types type, string unitOfMeasurement)
    {
        this.Name = name;
        this.Value = value;
        this.UnitOfMeasurement = unitOfMeasurement;
        this.Type = type;
    }

    public NumericalProperty(NumericalProperty numericalProperty)
    {
        this.Name = numericalProperty.Name;
        this.Value = numericalProperty.Value;
        this.UnitOfMeasurement = numericalProperty.UnitOfMeasurement;
        this.Type = numericalProperty.Type;
    }
}
