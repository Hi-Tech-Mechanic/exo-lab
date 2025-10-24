
namespace Assets.PersonalAssets.ScriptableObjects
{
    using UnityEngine;
    using Assets.PersonalAssets.Scripts.SuitComponents.ScriptableObjects;

    /// <summary>
    /// Необходимые состовляющие для корректной работы каждого компонента оружия
    /// </summary>
    [CreateAssetMenu(fileName = "WeaponComponentItemData", menuName = "Inventory/Weapon/Weapon component")]
    public class WeaponComponentItemData : ItemDataBase
    {
        /// <summary>
        /// Заданное вращение для объекта 
        /// </summary>
        public Vector3 Rotation;

        /// <summary>
        /// Заданная точка крепления данного объекта к другому объекту
        /// </summary>
        public Vector3 AttachmentPoint;
    }
}
