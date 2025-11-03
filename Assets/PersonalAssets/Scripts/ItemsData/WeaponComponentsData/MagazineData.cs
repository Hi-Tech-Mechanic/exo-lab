using UnityEngine;

namespace Assets.PersonalAssets.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MagazineItemData", menuName = "Inventory/Weapon/Magazine")]
    public class MagazineData : WeaponComponentItemData
    {
        public uint Bullets;
    }
}
