namespace ExoLab.Assembly
{
    using ExoLab.Helpers;
    using ExoLab.StructuralСomponents;
    using ExoLab.StructuralСomponents.Weapon;
    using ExoLab.UI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        [SerializeField]
        private GameObject statsContentPanel;

        [SerializeField]
        private GameObject statPrefab;

        [Space(5)]

        [Header("Компоненты")]
        [SerializeField]
        [Tooltip("Панель для перечисления компонентов")]
        private GameObject componentsContentPanel;

        [SerializeField]
        private GameObject componentPrefab;

        // Конвертируем все в double так как кастуем из типа object
        private double durability;
        private double weight;
        private double bullets;

        /// <summary>
        /// Список уже хранящихся строк характеристик,
        /// приведено сразу в текст для удобства
        /// </summary>
        private List<TextMeshProUGUI> statRows = new List<TextMeshProUGUI>();

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
                this.statRows = this.statsContentPanel.GetComponentsInChildren<TextMeshProUGUI>().ToList();
            }
        }

        /// <summary>
        /// Обработчик присоединения конструкции, который обновляет выводимые характеристики
        /// </summary>
        /// <param statValue="assemblyComponentTmp">Компонент который был навешан</param>
        private void AttachHandler(AssemblyComponentBase assemblyComponent)
        {
            this.AddComponentInList(assemblyComponent);

            var dictStatValues = this.GetStatValues(assemblyComponent);
            var statCount = dictStatValues.Count;

            if (this.statRows.Count == 0) // При первой отработке
            {
                CreateAndFillRows();
            }
            else
            {
                foreach (var child in this.statRows)
                {
                    Destroy(child.gameObject.transform.parent.parent.parent.gameObject); // todo refactor
                }

                this.statRows.Clear();
            }

            //foreach (var row in this.statRows)
            //{

            //    foreach (var statValue in dictStatValues)
            //    {
            //        // Если содержится данная характеристика в панели то ничего не делаем
            //        if (row.text != string.Empty)
            //            continue;

            //        this.FillStatRow(row, statValue);
            //    }
            //}

            return;

            void CreateAndFillRows()
            {
                if (this.statRows.Count < statCount)
                {
                    foreach (var statValue in dictStatValues)
                    {
                        var result = this.CreateAndFillStatRow(statValue);
                        this.statRows.Add(result.Item1);
                    }
                    //var remainder = statCount - this.statRows.Amount;
                    //for (var i = 0; i < remainder; i++)
                    //{
                  
                    //}
                }
            }
        }

        /// <summary>
        /// Создает строку с характеристикой
        /// </summary>
        private Tuple<TextMeshProUGUI, string, double> CreateAndFillStatRow(KeyValuePair<string, double> statValues)
        {


            Tuple<TextMeshProUGUI, string, double> result;
            var row = Instantiate(this.statPrefab, this.statsContentPanel.transform);
            var text = row.GetComponentInChildren<TextMeshProUGUI>();
            var classification = row.GetComponentInParent<ClassificationItem>();
            classification.Name = statValues.Key;
            FillStatRow(text, statValues);

            result = new (text, statValues.Key, statValues.Value);
            return result;
        }

        private void FillStatRow(TextMeshProUGUI statText, KeyValuePair<string, double> statValues)
        {
            statText.text = this.GetStatText(statValues.Key, statValues.Value.ToString());
            //var classification = statText.GetComponentInParent<ClassificationItem>();
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

        /// <summary>
        /// Получить количество характеристик у переданного компонента которое нужно вывести 
        /// Тип словария название - значение
        /// </summary>
        /// <param statValue="assemblyComponentTmp"></param>
        /// <returns></returns>
        private Dictionary<string, double> GetStatValues(AssemblyComponentBase assemblyComponent)
        {
            durability += assemblyComponent.Durability;
            weight += assemblyComponent.Weight;

            var properties = new Dictionary<string, double>
            {
                { nameof(assemblyComponent.Durability), assemblyComponent.Durability },
                { nameof(assemblyComponent.Weight), assemblyComponent.Weight }
            };

            switch (assemblyComponent)
            {
                case Receiver receiver:
                    break;
                case MuzzleAttachment muzzleAttachment:
                    break;
                case Magazine magazine:
                    bullets += magazine.Bullets;
                    properties.Add(nameof(magazine.Bullets), magazine.Bullets);
                    break;
            }

            return properties;
        }

        private void AddComponentInList(AssemblyComponentBase assemblyComponent)
        {
            this.componentList.Add(assemblyComponent);

            var component = Instantiate(this.componentPrefab, this.componentsContentPanel.transform);
            var componentText = component.GetComponent<TextDisplayer>();
            var componentImage = component.TryGetComponentWithTag<Image>(Constants.Constants.Tags.Icon);
            var componentData = component.GetComponent<AssemblyComponentBase>(); // todo изменить на отдельный класс хранения чисто для таких маленьких элементов
            componentData.SetItemData(assemblyComponent.TypedItemData);

            componentText.SetText(assemblyComponent.TypedItemData.Name);
            if (componentImage != null)
                componentImage.sprite = assemblyComponent.TypedItemData.Icon;
        }
    }
}
