namespace ExoLab.Data
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
        [Space(5)]
        [Header("Базовая информация о компоненте")]
        [Space(5)]

        [Tooltip("Прочность")]
        public double Durability;

        [Tooltip("Из чего состоит")]
        public IMaterial.MaterialType Material;

        [Header("Комплект данных отвечающий за привязку\nк конкретному объекту")]
        public List<AttachmentOption> AttachmentOptions;

        /// <summary>
        /// Комплект данных отвечающий за привязку к конкретному объекту
        /// </summary>
        [Serializable]
        public class AttachmentOption
        {
            [Tooltip("Данные родительского объекта\n(к которому присоединение)")]
            public ItemData ParentData;

            [Tooltip("Координаты точки крепления\nданного объекта к родительскому")]
            public Vector3 AttachmentPoint;

            [Tooltip("Вращение данного объекта")]
            public Quaternion Rotation;
        }
    }
}
