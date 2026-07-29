namespace ExoLab.Assembly
{
    using ExoLab.Data;
    using ExoLab.Helpers;
    using ExoLab.StructuralСomponents;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Модель собранной какой-либо конструкций. Основа для всех сборных конструкций
    /// </summary>
    public class ConstructionModelBase : IConstructionModel
    {
        private readonly Regex numberFilter = new Regex(@"\d+(\.\d+)?", RegexOptions.IgnoreCase);

        private readonly List<AssemblyComponentBase> components = new();

        /// <summary>
        /// Идентификационный номер структуры
        /// </summary>
        public string StructureId { get; protected set; }

        /// <summary>
        /// Список всех компонентов структуры (только для чтения)
        /// </summary>
        public IReadOnlyList<AssemblyComponentBase> Components => this.components.AsReadOnly();

        public ConstructionModelBase() 
        {
            this.StructureId = IdentificationGenerator.CreateGUID();
        }

        public void AddComponent(AssemblyComponentBase component)
        {
            this.components.Add(component);
        }

        public void RemoveComponent(AssemblyComponentBase component)
        {
            this.components.Remove(component);
        }

        /// <summary>
        /// Получить суммы всех характеристик компонентов <see cref="Components"/>
        /// </summary>
        /// <returns></returns>
        public List<ItemCharacteristicTypes.ItemStringCharacteristic> GetAllCharacteristicSums()
        {
            var result = new List<ItemCharacteristicTypes.ItemStringCharacteristic>();

            foreach (var component in this.components)
            {
                var stats = component.ItemData.GetNumericStats();

                foreach (var stat in stats)
                {
                    var match = this.numberFilter.Match(stat.Value.ToString()).Value;
                    var valueIsNumeric = double.TryParse(match, out var numericValue);

                    if (valueIsNumeric == false)
                    {
                        continue;
                    }

                    int index = -1;
                    ItemCharacteristicTypes.ItemStringCharacteristic? existingStat = null;
                    var existingStats = result.Where(x => x.Name == stat.Name);

                    if (existingStats.Count() != 0)
                    {
                        existingStat = existingStats.First();
                    }
                    if (existingStat != null)
                    {
                         index = result.IndexOf((ItemCharacteristicTypes.ItemStringCharacteristic)existingStat);
                    }

                    if (index != -1)
                    {
                        match = this.numberFilter.Match(stat.Value.ToString()).Value;
                        double.TryParse(match, out var oldValue);
                        var newValue = oldValue + numericValue;
                        var newStat = new ItemCharacteristicTypes.ItemStringCharacteristic(stat.Name, newValue.ToString());

                        result[index] = newStat;
                    }
                    else
                    {
                        var newStat = new ItemCharacteristicTypes.ItemStringCharacteristic(stat.Name, numericValue.ToString());
                        result.Add(newStat);
                    }
                }
            }

            return result;
        }

        public virtual void Save()
        {
            // TODO
        }

        public virtual void Load()
        {
            // TODO
        }
    }
}