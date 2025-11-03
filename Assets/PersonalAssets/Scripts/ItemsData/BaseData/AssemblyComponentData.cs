namespace Assets.PersonalAssets.Scripts.SuitComponents.ScriptableObjects
{
    using System.Collections.Generic;
    using System;
    using UnityEngine;

    /// <summary>
    /// Данные об абстрактном компоненте, он может быть как оружейным так и от костюма,
    /// главная особенность в данных о креплении
    /// </summary>
    [CreateAssetMenu(fileName = "ComponentData", menuName = "Inventory/Component data")]
    public class AssemblyComponentData : ItemData
    {
        public double Durability;

        public IMaterial.MaterialType Material;

        [Header("Комплект данных отвечающий за привязку\nк конкретному объекту")]
        public List<AttachmentOptions> attachmentOptions;

        /// <summary>
        /// Комплект данных отвечающий за привязку к конкретному объекту
        /// </summary>
        [Serializable]
        public class AttachmentOptions
        {
            [Header("Данные родительского объекта\n(к которому присоединение)")]
            public ItemData parentObject;

            [Header("Координаты точки крепления\nданного объекта к родительскому")]
            public Vector3 AttachmentPoint;

            [Header("Вращение данного объекта")]
            public Vector3 Rotation;
        }
    }
}
