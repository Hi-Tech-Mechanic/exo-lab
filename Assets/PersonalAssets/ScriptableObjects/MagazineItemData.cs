using UnityEngine;

namespace Assets.PersonalAssets.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MagazineItemData", menuName = "Inventory/Weapon/Magazine")]
    public class MagazineItemData : WeaponComponentItemData
    {
        public uint Bullets;
    }
}
