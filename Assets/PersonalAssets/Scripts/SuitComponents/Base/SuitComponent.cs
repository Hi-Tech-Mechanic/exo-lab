using UnityEngine;

public class SuitComponent : SuitComponentBase, IDamageable
{
    public override string Description => string.Empty;

    public override double Weight { get; set; }

    public override double Hardness { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    
    public override IMaterial.MaterialType Material { get; }

    public void GetDamage()
    {
        throw new System.NotImplementedException();
    }
}
