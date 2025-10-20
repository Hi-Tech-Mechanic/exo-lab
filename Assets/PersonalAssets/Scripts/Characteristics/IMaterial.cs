using UnityEngine;

public interface IMaterial : IStatistic
{
    MaterialType Material { get; }

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
