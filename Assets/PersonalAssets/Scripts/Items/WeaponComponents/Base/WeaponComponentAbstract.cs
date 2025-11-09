namespace ExoLab.StructuralСomponents.Weapon
{
    using ExoLab.Data;

    /// <summary>
    /// Базовый класс для физичных оруженых компонентов
    /// </summary>
    public abstract class WeaponComponentAbstract<T> : AssemblyComponentAbstract<T> where T : WeaponComponentItemData
    {
        private void Start()
        {
            //Debug.Log(TypedItemData.Description);
            //Debug.Log(TypedItemData.Name);
            //Debug.Log(TypedItemData.Material);
            //Debug.Log(TypedItemData.Durability);
        }
    }
}
