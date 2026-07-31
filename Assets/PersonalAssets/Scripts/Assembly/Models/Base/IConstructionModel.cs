namespace ExoLab.Assembly
{
    using ExoLab.StructuralСomponents;
    using System.Collections.Generic;

    /// <summary>
    /// Интерфейс модели сборной конструкции
    /// </summary>
    public interface IConstructionModel
    {
        /// <summary>
        /// Идентификационный номер структуры
        /// </summary>
        public string StructureId { get; }

        /// <summary>
        /// Список всех компонентов структуры (только для чтения)
        /// </summary>
        public IReadOnlyList<AssemblyComponentBase> Components { get; }

        /// <summary>
        /// Добавить компонент в конструкцию
        /// </summary>
        public void AddComponent(AssemblyComponentBase component);

        /// <summary>
        /// Удалить компонент из конструкции
        /// </summary>
        public void RemoveComponent(AssemblyComponentBase component);

        /// <summary>
        /// Получить все характеристики конструкции
        /// </summary>
        public List<NumericalProperty> GetSumOfAllNumericalCharacteristics();

        /// <summary>
        /// Сохранить конструкцию
        /// </summary>
        public void Save();

        /// <summary>
        /// Загрузить конструкцию
        /// </summary>
        public void Load();
    }
}