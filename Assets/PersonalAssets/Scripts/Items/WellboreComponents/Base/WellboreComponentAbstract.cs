namespace ExoLab.StructuralСomponents.Weapon
{
    using ExoLab.Data;

    /// <summary>
    /// Базовый класс для физичных компонентов ствола
    /// </summary>
    public abstract class WellboreComponentAbstract<T> : AssemblyComponentBase where T : WellboreComponentItemData
    {
        public new T TypedItemData => (T)base.itemData;

    }
}
