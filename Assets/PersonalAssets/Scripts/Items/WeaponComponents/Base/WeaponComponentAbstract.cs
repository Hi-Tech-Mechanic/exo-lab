namespace ExoLab.StructuralСomponents.Weapon
{
    using ExoLab.Data;

    /// <summary>
    /// Базовый класс для физичных оруженых компонентов
    /// </summary>
    public abstract class WeaponComponentAbstract<T> : AssemblyComponentBase where T : WeaponComponentItemData
    {
        public new T TypedItemData => (T)base.itemData;
    }
}
