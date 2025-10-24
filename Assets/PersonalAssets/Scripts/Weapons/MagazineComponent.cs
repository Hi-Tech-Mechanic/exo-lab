using Assets.PersonalAssets.ScriptableObjects;

/// <summary>
/// ������� ������
/// </summary>
public class MagazineComponent : ComponentBase, IBullets
{
    private MagazineItemData magazineItemData;

    public uint Bullets { get; set; }

    protected override void Initialize()
    {
        base.Initialize();
        this.Bullets = magazineItemData.Bullets;
    }
}
