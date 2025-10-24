public interface IMaterial : IStatistic
{
    public MaterialType Material { get; }

    public enum MaterialType
    {
        Iron,
        Copper,
        Tin,
        Bronze,
        Chromium,
        Titanium,
        Tungsten,
        Plastic
    }
}
