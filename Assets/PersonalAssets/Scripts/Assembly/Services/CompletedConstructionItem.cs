namespace ExoLab.Assembly.Services
{
    using ExoLab.Data;
    using ExoLab.Helpers;
    using ExoLab.StructuralСomponents;
    using System.Collections.Generic;
    using System.Linq;
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
            this.UpdateCharacteristics(model.Components);

            var characteristics = this.GetTotalStats();
            var weight = characteristics.FirstOrNullStruct(x => x.Name.Equals("Weight", System.StringComparison.OrdinalIgnoreCase))?.Value ?? 0;

            this.ItemData.Id = model.StructureId;
            this.ItemData.SetName($"Construction: {this.ItemData.Id}");
            this.ItemData.SetDescription($"Description of the construction: {this.ItemData.Id}");
            this.ItemData.SetWeight(weight);
            this.ItemData.SetMaxStackSize(1);
        }

        private void UpdateCharacteristics(IReadOnlyList<AssemblyComponentBase> components)
        {
            this.ItemData.Characteristics.Clear();

            foreach (var component in components)
            {
                this.ItemData.Characteristics.AddRange(component.TypedItemData.Characteristics);
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

        // TODO

        /// <summary>
        /// Получить сумму всех характеристик компонентов
        /// </summary>
        public List<ItemCharacteristicTypes.ItemNumericCharacteristic> GetTotalStats()
        {
            var result = new List<ItemCharacteristicTypes.ItemNumericCharacteristic>();

            foreach (var component in this.attachedComponents)
            {
                var stats = component.ItemData.GetNumericStats();

                foreach (var stat in stats)
                {
                    if (double.TryParse(stat.Value.ToString(), out var numericValue))
                    {
                        var index = -1;
                        ItemCharacteristicTypes.ItemNumericCharacteristic? existringElement = null;

                        var existingElements = result.Where(x => x.Name == stat.Name);
                        if (existingElements.Count() > 0)
                        {
                            existringElement = existingElements.First();
                            index = result.IndexOf((ItemCharacteristicTypes.ItemNumericCharacteristic)existringElement);
                        }

                        if (index != -1)
                        {
                            var newValue = ((ItemCharacteristicTypes.ItemNumericCharacteristic)existringElement).Value + numericValue;
                            var newElement = new ItemCharacteristicTypes.ItemNumericCharacteristic(stat.Name, newValue);

                            result.Remove((ItemCharacteristicTypes.ItemNumericCharacteristic)existringElement);
                            result.Add(newElement);
                        }
                        else
                        {
                            var newElement = new ItemCharacteristicTypes.ItemNumericCharacteristic(stat.Name, numericValue);

                            result.Add(newElement);
                        }
                    }
                }
            }

            return result;
        }
    }
}