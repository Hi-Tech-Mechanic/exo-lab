namespace ExoLab.Data
{
    using UnityEngine;

    /// <summary>
    /// Необходимые состовляющие для корректной работы каждого компонента оборудования ствола скважины
    /// </summary>
    [CreateAssetMenu(fileName = "WellboreComponentItemData", menuName = "Inventory/Wellbore component data")]
    public class WellboreComponentItemData : AssemblyComponentData
    {
        [Tooltip("Максимальная скорость вращения")]
        public double MaxRotationSpeed;

        [Tooltip("Максимальная температура")]
        public double MaxTemperature;
    }
}
