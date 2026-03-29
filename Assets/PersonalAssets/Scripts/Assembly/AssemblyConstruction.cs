namespace ExoLab.Assembly
{
    using ExoLab.Helpers;
    using ExoLab.StructuralСomponents;
    using ExoLab.UI;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Сборщик конструкций
    /// </summary>
    public class AssemblyConstruction : MonoBehaviour
    {
        public static Action<AssemblyComponentBase> OnAttached;

        [Header("Характеристики")]
        [Tooltip("Панель для перечисления характеристик")]
        [SerializeField] private GameObject statsContentPanel;
        [SerializeField] private GameObject statPrefab;

        [Space(5)]

        [Header("Компоненты")]
        [Tooltip("Панель для перечисления компонентов")]
        [SerializeField] private GameObject componentsContentPanel;
        [SerializeField] private GameObject componentPrefab;

        // Конвертируем все в double так как кастуем из типа object
        private double durability;
        private double weight;
        private double bullets;

        private readonly Regex numberFilter = new Regex(@"\d+(\.\d+)?", RegexOptions.IgnoreCase);

        /// <summary>
        /// Вместо кучи отдельных полей идет один словарь
        /// </summary>
        private Dictionary<string, double> numericStats = new Dictionary<string, double>();

        /// <summary>
        /// Список уже хранящихся строк характеристик,
        /// приведено сразу в текст для удобства
        /// </summary>
        private List<GameObject> statRows = new List<GameObject>();

        /// <summary>
        /// Список компонентов
        /// </summary>
        private List<AssemblyComponentBase> componentList = new List<AssemblyComponentBase>();

        private void Awake()
        {
            this.Initialize();
        }

        private void OnEnable()
        {
            OnAttached += this.AttachHandler;
        }

        private void OnDisable()
        {
            OnAttached -= this.AttachHandler;
        }

        private void Initialize()
        {
            if (this.statRows.Count == 0)
            {
                foreach (Transform child in statsContentPanel.transform)
                {
                    this.statRows.Add(child.gameObject);
                }
            }
        }

        /// <summary>
        /// Обработчик присоединения конструкции, который обновляет выводимые характеристики
        /// </summary>
        /// <param statValue="assemblyComponentTmp">Компонент который был навешан</param>
        private void AttachHandler(AssemblyComponentBase assemblyComponent)
        {
            this.AddComponentInList(assemblyComponent);

            this.ClearStatRows();
            this.CreateAndFillStatRows(assemblyComponent);
        }

        private void ClearStatRows()
        {
            foreach (var child in this.statRows)
            {
                Destroy(child);
            }

            this.statRows.Clear();
        }

        /// <summary>
        /// Создает строку с характеристикой
        /// </summary>
        private void CreateAndFillStatRows(AssemblyComponentBase assemblyComponent)
        {
            var StatValues = assemblyComponent.GetNumericStats();

            foreach (var stat in StatValues)
            {
                var match = numberFilter.Match(stat.Value.ToString()).Value;
                var valueIsNumeric = double.TryParse(match, out var numericValue);

                if (valueIsNumeric == false)
                {
                    continue;
                }

                if (this.numericStats.ContainsKey(stat.Key))
                {
                    this.numericStats[stat.Key] += numericValue;
                }
                else
                {
                    this.numericStats.TryAdd(stat.Key, numericValue);
                }
            }

            foreach (var statValue in this.numericStats)
            {
                var statRow = Instantiate(this.statPrefab, this.statsContentPanel.transform);
                this.statRows.Add(statRow);

                var statText = statRow.GetComponentInChildren<TextMeshProUGUI>();
                statText.text = this.PintText($"{statValue.Key}: {statValue.Value.ToString()}");
                //statText.text = this.GetStatText(, statValues.Value.ToString());

                var classification = statRow.GetComponent<ClassificationItem>();
                classification.Name = statValue.Key.ToString();
            }
        }

        private string GetStatText(string propertyName, string propertyValue)
        {
            AssemblyComponentBase assemblyComponentTmp; // Просто пустышка для взятия имен

            propertyValue = this.PintText(propertyValue);
            switch (propertyName)
            {
                case nameof(assemblyComponentTmp.Durability):
                    return $"Прочность: {propertyValue}";
                case nameof(assemblyComponentTmp.Weight):
                    return $"Вес: {propertyValue}";
            }

            return string.Empty;
        }

        private string PintText(string value)
        {
            return $"<color=green>{value}</color>";
        }

        ///// <summary>
        ///// Получить количество характеристик у переданного компонента которое нужно вывести 
        ///// Тип словария название - значение
        ///// </summary>
        ///// <param statValue="assemblyComponentTmp"></param>
        ///// <returns></returns>
        //private Dictionary<string, double> GetStatValues(AssemblyComponentBase assemblyComponent)
        //{

        //    //this.durability += assemblyComponent.Durability;
        //    //this.weight += assemblyComponent.Weight;

        //    //var properties = new Dictionary<string, double>
        //    //{
        //    //    { nameof(assemblyComponent.Durability), assemblyComponent.Durability },
        //    //    { nameof(assemblyComponent.Weight), assemblyComponent.Weight }
        //    //};

        //    //switch (assemblyComponent)
        //    //{
        //    //    case Receiver receiver:
        //    //        break;
        //    //    case MuzzleAttachment muzzleAttachment:
        //    //        break;
        //    //    case Magazine magazine:
        //    //        bullets += magazine.Bullets;
        //    //        properties.Add(nameof(magazine.Bullets), magazine.Bullets);
        //    //        break;
        //    //}

        //    return properties;
        //}

        private void AddComponentInList(AssemblyComponentBase assemblyComponent)
        {
            this.componentList.Add(assemblyComponent);

            var component = Instantiate(this.componentPrefab, this.componentsContentPanel.transform);
            var componentText = component.GetComponent<TextDisplayer>();
            var componentImage = component.TryGetComponentWithTag<Image>(Constants.Constants.Tags.Icon);
            var componentData = component.GetComponent<AssemblyComponentBase>(); // todo изменить на отдельный класс хранения чисто для таких маленьких элементов
            componentData.SetItemData(assemblyComponent.TypedItemData);

            componentText.SetText(assemblyComponent.Name);
            if (componentImage != null)
                componentImage.sprite = assemblyComponent.TypedItemData.Icon;
        }
    }
}
