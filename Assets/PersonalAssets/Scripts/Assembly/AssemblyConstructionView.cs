namespace ExoLab.Assembly
{
    using ExoLab.Helpers;
    using ExoLab.StructuralСomponents;
    using ExoLab.UI;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// View-слой для отображения характеристик и компонентов сборной конструкции.
    /// Отвечает только за UI-рендеринг. Содержит ссылки на панели и префабы для инспектора.
    /// </summary>
    public class AssemblyConstructionView : MonoBehaviour
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

        private readonly List<GameObject> statRows = new();
        private readonly List<GameObject> componentRows = new();

        private void Awake()
        {
            this.Initialize();
        }

        private void Initialize()
        {
            if (this.statRows.Count == 0)
            {
                foreach (Transform child in this.statsContentPanel.transform)
                {
                    this.statRows.Add(child.gameObject);
                }
            }
        }

        /// <summary>
        /// Очистить все строки характеристик
        /// </summary>
        public void ClearStatRows()
        {
            foreach (var child in this.statRows)
            {
                Destroy(child);
            }

            this.statRows.Clear();
        }

        /// <summary>
        /// Создать и заполнить строки характеристик на основе сумм статов модели
        /// </summary>
        public void CreateStatRows(List<IStatistic> stats)
        {
            foreach (var stat in stats)
            {
                var statRow = Instantiate(this.statPrefab, this.statsContentPanel.transform);
                this.statRows.Add(statRow);

                var statTextComponent = statRow.GetComponentInChildren<TextMeshProUGUI>();
                var statText = HelperText.GetGreenText(stat.FullFormattedValue);
                statTextComponent.text = statText;

                var classification = statRow.GetComponent<ClassificationItem>();
                classification.Name = stat.Name;
            }
        }

        /// <summary>
        /// Добавить строку компонента в список
        /// </summary>
        public void AddComponentRow(AssemblyComponentBase assemblyComponent)
        {
            var component = Instantiate(this.componentPrefab, this.componentsContentPanel.transform);
            this.componentRows.Add(component);

            var componentText = component.GetComponent<TextDisplayer>();
            var componentImage = component.TryGetComponentWithTag<Image>(Constants.Constants.Tags.Icon);
            var componentData = component.GetComponent<AssemblyComponentBase>();
            componentData.SetItemData(assemblyComponent.TypedItemData);

            componentText.SetText(assemblyComponent.ItemData.Name);
            if (componentImage != null)
            {
                componentImage.sprite = assemblyComponent.TypedItemData.Icon;
            }
        }

        /// <summary>
        /// Очистить все строки компонентов
        /// </summary>
        public void ClearComponentRows()
        {
            foreach (var child in this.componentRows)
            {
                Destroy(child);
            }

            this.componentRows.Clear();
        }
    }
}