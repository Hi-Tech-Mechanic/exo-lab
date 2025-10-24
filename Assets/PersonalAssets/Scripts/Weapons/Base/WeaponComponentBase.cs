using Assets.PersonalAssets.ScriptableObjects;
using UnityEngine;

/// <summary>
/// Базовый класс для физичных оруженых компонентов
/// </summary>
public class WeaponComponentBase : ComponentBase, IAttachmentPoint, IRotation
{
    public Vector3 AttachmentPoint { get; set; }
    public Vector3 Rotation { get; set; }

    protected override void Initialize<T>(T weaponComponentItemDataGeneric)
    {
        if (weaponComponentItemDataGeneric is not WeaponComponentItemData weaponComponentItemData)
            return;

        base.Initialize(weaponComponentItemData);
        this.AttachmentPoint = weaponComponentItemData.AttachmentPoint;
        this.Rotation = weaponComponentItemData.Rotation;
    }
}
