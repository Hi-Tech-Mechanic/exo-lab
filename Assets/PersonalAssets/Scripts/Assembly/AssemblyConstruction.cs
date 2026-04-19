namespace ExoLab.Assembly
{
    using ExoLab.Helpers;
    using ExoLab.StructuralСomponents;
    using ExoLab.UI;
    using System;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Сборщик конструкций.
    /// TODO должен при активации подтягивать из выбранного сборного предмета характеристики
    /// и обновлять окно с выводом настроек
    /// </summary>
    public class AssemblyConstruction : MonoBehaviour
    {
        [Header("Характеристики")]
        [Tooltip("Панель для перечисления характеристик")]
        [SerializeField] private GameObject statsContentPanel;
        [SerializeField] private GameObject statPrefab;

        [Space(5)]

        [Header("Компоненты")]
        [Tooltip("Панель для перечисления компонентов")]
        [SerializeField] private GameObject componentsContentPanel;
        [SerializeField] private GameObject componentPrefab;

        /// <summary>
        /// Модель для взаимодействия с сборными объектами
        /// </summary>
        private AssembledConstructionModelBase constructionModel = new();

        /// <summary>
        /// Список уже хранящихся строк характеристик,
        /// приведено сразу в текст для удобства
        /// </summary>
        private List<GameObject> statRows = new List<GameObject>();

        private void Awake()
        {
            this.Initialize();
        }

        private void OnEnable()
        {
            GameEvents.Assembly.ComponentOnAttached += this.AttachHandler;
        }

        private void OnDisable()
        {
            GameEvents.Assembly.ComponentOnAttached -= this.AttachHandler;
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
            var statValues = assemblyComponent.GetNumericStats();
            var numericStats = constructionModel.GetStatSums();

            foreach (var statValue in numericStats)
            {
                var statRow = Instantiate(this.statPrefab, this.statsContentPanel.transform);
                this.statRows.Add(statRow);

                var statText = statRow.GetComponentInChildren<TextMeshProUGUI>();
                statText.text = HelperText.GetColoredText($"{statValue.Key}: {statValue.Value.ToString()}");

                var classification = statRow.GetComponent<ClassificationItem>();
                classification.Name = statValue.Key.ToString();
            }
        }

        private void AddComponentInList(AssemblyComponentBase assemblyComponent)
        {
            this.constructionModel.AddComponent(assemblyComponent);

            var component = Instantiate(this.componentPrefab, this.componentsContentPanel.transform);
            var componentText = component.GetComponent<TextDisplayer>();
            var componentImage = component.TryGetComponentWithTag<Image>(Constants.Constants.Tags.Icon);
            var componentData = component.GetComponent<AssemblyComponentBase>(); // todo изменить на отдельный класс хранения чисто для таких маленьких элементов
            componentData.SetItemData(assemblyComponent.TypedItemData);

            componentText.SetText(assemblyComponent.Name);
            if (componentImage != null)
            {
                componentImage.sprite = assemblyComponent.TypedItemData.Icon;
            }
        }
    }
}
