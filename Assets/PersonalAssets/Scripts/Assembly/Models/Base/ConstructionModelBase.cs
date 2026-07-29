namespace ExoLab.Assembly
{
    using ExoLab.Helpers;
    using ExoLab.StructuralСomponents;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Модель собранной какой-либо конструкций. Основа для всех сборных конструкций
    /// </summary>
    public class ConstructionModelBase : IConstructionModel
    {
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
        public List<ITypedStatistic<double>> GetSumOfAllNumericalCharacteristics()
        {
            var result = new List<ITypedStatistic<double>>();

            foreach (var component in this.components)
            {
                var stats = component.ItemData.NumericalCharacteristics;

                foreach (var stat in stats)
                {
                    var existingStat = result.FirstOrNull(x => x.Name == stat.Name);

                    if (existingStat != null)
                    {
                        var newStat = (ITypedStatistic<double>)Activator.CreateInstance(stat.GetType());
                        newStat.Value = existingStat.Value + stat.Value;

                        result.Remove(existingStat);
                        result.Add(newStat);
                    }
                    else
                    {
                        var copy = (ITypedStatistic<double>)Activator.CreateInstance(stat.GetType());
                        copy.Value = stat.Value;
                        result.Add(copy);
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