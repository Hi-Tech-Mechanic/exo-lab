namespace ExoLab.Assembly.Services
{
    using ExoLab.Helpers;
    using ExoLab.StructuralСomponents;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Представляет завершённую сборную конструкцию как предмет.
    /// Хранит список компонентов и итоговые характеристики.
    /// </summary>
    public class CompletedConstructionItem : ItemBase
    {
        [SerializeField] private List<AssemblyComponentBase> attachedComponents = new();

        /// <summary>
        /// Список прикреплённых компонентов (только для чтения)
        /// </summary>
        public IReadOnlyList<AssemblyComponentBase> AttachedComponents => this.attachedComponents.AsReadOnly();

        /// <summary>
        /// Инициализировать предмет данными из модели конструкции
        /// </summary>
        public void Initialize(IConstructionModel model)
        {
            this.SetObjectName(model);
            this.UpdateAttachedComponents(model.Components);

            this.SetId(model);
            this.SetName(model);
            this.SetDescription(model);
            this.SetMaxStackSize(model);

            var characteristics = model.GetSumOfAllNumericalCharacteristics();

            this.SetWeight(characteristics);
            this.AddOtherCharacteristics(characteristics);
        }

        /// <summary>
        /// Add non default properties
        /// </summary>
        private void AddOtherCharacteristics(List<NumericalProperty> characteristics)
        {
            foreach (var characteristic in characteristics)
            {
                if (characteristic.Type is not CharacteristicTypes.Types.Weight &&
                    characteristic.Type is not CharacteristicTypes.Types.MaxStackSize)
                {
                    this.ItemData.Characteristics.Add(characteristic);
                }
            }
        }

        private void UpdateAttachedComponents(IReadOnlyList<AssemblyComponentBase> components)
        {
            this.attachedComponents.Clear();

            foreach (var component in components)
            {
                this.attachedComponents.Add(component);
            }
        }

        private void SetObjectName(IConstructionModel model)
        {
            this.name = $"CompletedConstruction_{model.StructureId ?? "Unknown"}";
        }

        private void SetId(IConstructionModel model)
        {
            this.ItemData.Id = model.StructureId;
        }

        private void SetName(IConstructionModel model)
        {
            this.ItemData.SetName($"Construction: {model.StructureId}");
        }

        private void SetDescription(IConstructionModel model)
        {
            this.ItemData.SetDescription($"Description of the construction: {model.StructureId}");
        }

        private void SetMaxStackSize(IConstructionModel model)
        {
            this.ItemData.SetMaxStackSize(1);
        }

        private void SetWeight(List<NumericalProperty> characteristics)
        {
            var weight = characteristics.FirstOrNull(x => x.Type == CharacteristicTypes.Types.Weight);
            this.ItemData.SetWeight(weight.Value);
        }
    }
}