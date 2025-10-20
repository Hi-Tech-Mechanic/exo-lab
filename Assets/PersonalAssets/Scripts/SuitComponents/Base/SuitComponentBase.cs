using UnityEngine;

public abstract class SuitComponentBase :
    MonoBehaviour,
    ISuitComponent,
    IDescription,
    IWeight,
    IHardness,
    IMaterial
{
    public abstract double Hardness { get; set; }

    public abstract double Weight { get; set; }

    public abstract string Description { get; }

    public abstract IMaterial.MaterialType Material { get; }
}
