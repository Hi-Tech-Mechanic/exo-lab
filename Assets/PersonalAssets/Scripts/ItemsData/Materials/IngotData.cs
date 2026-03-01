namespace ExoLab.Data
{
    using UnityEngine;

    /// <summary>
    /// Локтевая бронепластина
    /// </summary>
    [CreateAssetMenu(fileName = "IngotData", menuName = "Materials/Ingot")]
    public class IngotData : ItemData
    {
        public IMaterial.MaterialType material;
    }
}
