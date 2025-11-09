namespace ExoLab.Data
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "MagazineData", menuName = "Inventory/Weapon/Magazine")]
    public class MagazineData : WeaponComponentItemData
    {
        public uint Bullets;
    }
}
