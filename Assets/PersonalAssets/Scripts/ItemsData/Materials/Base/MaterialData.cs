namespace ExoLab
{
    using ExoLab.Data;
    using UnityEngine;

    /// <summary>
    /// Комплект данных для материалов
    /// </summary>
    [CreateAssetMenu(fileName = "MaterialData", menuName = "Materials/Material")]
    public class MaterialData : ItemData
    {
        public MaterialProperty.MaterialType material;
    }
}
