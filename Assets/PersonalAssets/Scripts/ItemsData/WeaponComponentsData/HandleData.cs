using UnityEngine;

namespace Assets.PersonalAssets.ScriptableObjects
{
    /// <summary>
    /// Характеристики рукоятки 
    /// </summary>
    [CreateAssetMenu(fileName = "HandleItemData", menuName = "Inventory/Weapon/Handle")]
    public class HandleData : WeaponComponentItemData
    {
        public int test;
    }
}
