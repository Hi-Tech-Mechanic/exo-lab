namespace ExoLab.Data
{
    using System;
    using UnityEngine;
    using UnityEngine.Serialization;
    using System.Collections.Generic;

    /// <summary>
    /// Данные об абстрактном компоненте, он может быть как оружейным так и от костюма,
    /// главная особенность в данных о креплении
    /// </summary>
    [CreateAssetMenu(fileName = "ComponentData", menuName = "Inventory/Component data")]
    public class AssemblyComponentData : ItemData
    {
        [Space(5)]
        [Header("Base information about component")]
        [Space(5)]

        [FormerlySerializedAs("Durability")]
        [SerializeField] private double durability;

        [Tooltip("What the object is made of")]
        [FormerlySerializedAs("Material")]
        [SerializeField] private MaterialProperty.MaterialType material;

        [Header("Комплект данных отвечающий за привязку\nк конкретному объекту")]
        public List<AttachmentOption> AttachmentOptions;

        [Header("Перечень совместимых типов компонентов и их количество")]
        public List<CompabilityComponent> CompabilityComponents;

        private DurabilityProperty? durabilityProperty;
        private MaterialProperty? materialProperty;

        public DurabilityProperty Durability
        {
            get
            {
                if (this.durabilityProperty == null)
                {
                    this.durabilityProperty = new DurabilityProperty();
                    this.durabilityProperty.Value = this.durability;
                }

                return this.durabilityProperty; 
            }
        }

        public MaterialProperty Material
        {
            get
            {
                if (this.materialProperty == null)
                {
                    this.materialProperty = new MaterialProperty();
                    this.materialProperty.Value = this.material.ToString();
                }

                return this.materialProperty;
            }
        }

        public override List<IStatistic> Characteristics
        {
            get
            {
                var result = new List<IStatistic>();

                result.AddRange(base.Characteristics);
                result.Add(this.Durability);
                result.Add(this.Material);

                return result;
            }
        }

        /// <summary>
        /// Комплект данных отвечающий за привязку к конкретному объекту
        /// </summary>
        [Serializable]
        public class AttachmentOption
        {
            [Tooltip("Данные родительского объекта\n(к которому присоединение)")]
            public AssemblyComponentData ParentData;

            [Tooltip("Координаты точки крепления\nданного объекта к родительскому")]
            public Vector3 AttachmentPoint;

            [Tooltip("Вращение данного объекта")]
            public Quaternion Rotation;

            [Tooltip("Масштаб данного объекта")]
            public Vector3 Scale = new Vector3(1F, 1F, 1F);
        }

        /// <summary>
        /// Перечень совместимых компонентов и их количество
        /// </summary>
        [Serializable]
        public class CompabilityComponent //todo пока нет, не так нужно
        {
            [Tooltip("Тип объекта")]
            public Constants.Constants.Components.ComponentTypes componentType;

            [Tooltip("Допустимое количество данного компонента")]
            public int Count;
        }
    }
}
