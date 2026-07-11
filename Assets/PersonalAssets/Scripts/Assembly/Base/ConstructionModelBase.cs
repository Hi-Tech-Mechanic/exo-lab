namespace ExoLab
{
    using ExoLab.StructuralСomponents;
    using System.Collections.Generic;
    //using LiteDB;
    using System.Text.RegularExpressions;
    using ExoLab.Helpers;

    /// <summary>
    /// Модель собранной какой-либо конструкций. Основа для всех сборных конструкций
    /// </summary>
    public class ConstructionModelBase
    {
        /// <summary>
        /// Идентификационный номер структуры
        /// </summary>
        //[BsonId]
        public string StructureId;

        /// <summary>
        /// Список всех компонентов структуры
        /// </summary>
        protected List<AssemblyComponentBase> Components = new();

        private readonly Regex numberFilter = new Regex(@"\d+(\.\d+)?", RegexOptions.IgnoreCase);

        public ConstructionModelBase() { }

        public ConstructionModelBase(string id)
        {
            this.StructureId = id;
        }

        public void AddComponent(AssemblyComponentBase component)
        {
            Components.Add(component);
        }

        public void RemoveComponent(AssemblyComponentBase component)
        {
            Components.Remove(component);
        }

        /// <summary>
        /// Получить суммы всех характеристик компонентов <see cref="Components"/>>
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, double> GetStatSums()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            foreach (var component in this.Components)
            {
                var stats = component.GetNumericStats();

                foreach (var stat in stats)
                {
                    var match = numberFilter.Match(stat.Value.ToString()).Value;
                    var valueIsNumeric = double.TryParse(match, out var numericValue);

                    if (valueIsNumeric == false)
                    {
                        continue;
                    }

                    if (result.ContainsKey(stat.Key))
                    {
                        result[stat.Key] += numericValue;
                    }
                    else
                    {
                        result.TryAdd(stat.Key, numericValue);
                    }
                }
            }

            return result;
        }

        protected virtual void Save()
        {
            // TODO
        }

        protected virtual void Load()
        {
             // TODO
        }

        
        private string GetStatText(string propertyName, string propertyValue)
        {
            AssemblyComponentBase assemblyComponentTmp; // Просто пустышка для взятия имен

            propertyValue = HelperText.GetColoredText(propertyValue);
            switch (propertyName)
            {
                case nameof(assemblyComponentTmp.Durability):
                    return $"Прочность: {propertyValue}";
                case nameof(assemblyComponentTmp.Weight):
                    return $"Вес: {propertyValue}";
            }

            return string.Empty;
        }

        /// <summary>
        /// todo сделать через абстракцию и подсчет опять же через double вместо жесткого приведения
        /// Получить количество характеристик у переданного компонента которое нужно вывести 
        /// Тип словария название - значение
        /// </summary>
        /// <param statValue="assemblyComponentTmp"></param>
        /// <returns></returns>
        private Dictionary<string, double> GetStatValues(AssemblyComponentBase assemblyComponent)
        {
            //this.durability += assemblyComponent.Durability;
            //this.weight += assemblyComponent.Weight;

            //var properties = new Dictionary<string, double>
            //{
            //    { nameof(assemblyComponent.Durability), assemblyComponent.Durability },
            //    { nameof(assemblyComponent.Weight), assemblyComponent.Weight }
            //};

            //switch (assemblyComponent)
            //{
            //    case Receiver receiver:
            //        break;
            //    case MuzzleAttachment muzzleAttachment:
            //        break;
            //    case Magazine magazine:
            //        bullets += magazine.Bullets;
            //        properties.Add(nameof(magazine.Bullets), magazine.Bullets);
            //        break;
            //}

            //return properties;
            return new Dictionary<string, double>();
        }
    }
}
