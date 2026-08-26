namespace ExoLab.Assembly
{
    using ExoLab.Assembly.Services;
    using ExoLab.StructuralСomponents;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// Контроллер сборки конструкции (точка входа).
    /// Координирует работу Model и View, обрабатывает события.
    /// </summary>
    public class AssemblyConstructionController : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance for accessing the controller from other components.
        /// </summary>
        public static AssemblyConstructionController Instance { get; private set; }

        [Header("Construction roots")]
        [Tooltip("Parent transform for weapon constructions")]
        [SerializeField] private Transform weaponRoot;

        [Tooltip("Parent transform for exoskeleton constructions")]
        [SerializeField] private Transform exoskeletonRoot;

        [Tooltip("Parent transform for wellbore constructions")]
        [SerializeField] private Transform wellboreRoot;

        [SerializeField] private AssemblyConstructionView constructionView;
        [Tooltip("Place where the completed construction appears")]
        [SerializeField] private Transform constructionParent;

        [Header("Screenshot settings")]
        [Tooltip("Root point where the prefab is spawned for the icon generator")]
        [SerializeField] private Transform screenshotRootPoint;

        private IConstructionModel constructionModel;

        /// <summary>
        /// Root point where the prefab is spawned for the icon generator.
        /// </summary>
        public Transform ScreenshotRootPoint => this.screenshotRootPoint;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        /// <summary>
        /// Returns the active root transform based on the current assembly mode.
        /// </summary>
        public Transform ActiveConstructionRoot
        {
            get
            {
                if (AssemblyModesController.ActiveConstructionRoot == this.weaponRoot)
                {
                    return this.weaponRoot;
                }

                if (AssemblyModesController.ActiveConstructionRoot == this.exoskeletonRoot)
                {
                    return this.exoskeletonRoot;
                }

                if (AssemblyModesController.ActiveConstructionRoot == this.wellboreRoot)
                {
                    return this.wellboreRoot;
                }

                return this.constructionParent;
            }
        }

        /// <summary>
        /// Текущая модель конструкции
        /// </summary>
        public IConstructionModel ConstructionModel => this.constructionModel;

        private void OnEnable()
        {
            GameEvents.AssemblyEvents.ComponentOnAttached += this.OnComponentAttachedHandler;
        }

        private void OnDisable()
        {
            GameEvents.AssemblyEvents.ComponentOnAttached -= this.OnComponentAttachedHandler;
        }

        private void OnComponentAttachedHandler(AssemblyComponentBase assemblyComponent)
        {
            this.OnComponentAttached(assemblyComponent);
        }

        /// <summary>
        /// Установить модель конструкции (например, SuitConstructionModel, WeaponConstructionModel)
        /// </summary>
        public void SetConstructionModel(IConstructionModel model)
        {
            this.constructionModel = model;
        }

        /// <summary>
        /// Обработчик присоединения компонента
        /// </summary>
        public void OnComponentAttached(AssemblyComponentBase assemblyComponent)
        {
            this.constructionModel.AddComponent(assemblyComponent);
            this.constructionView.AddComponentRow(assemblyComponent);

            this.constructionView.ClearStatRows();

            var stats = this.constructionModel.GetSumOfAllNumericalCharacteristics();
            var typedStats = new List<IStatistic>();
            typedStats.AddRange(stats.Select(x => (IStatistic)x));

            this.constructionView.CreateStatRows(typedStats);
        }

        #region public API

        public void StartConstruction()
        {
            this.constructionModel = new ConstructionModelBase();
        }

        /// <summary>
        /// Завершить сборку и получить GameObject готового предмета
        /// </summary>
        public void CompleteConstruction()
        {
            var construction = ConstructionCompletionService.CompleteConstruction(
                this.constructionModel,
                this.screenshotRootPoint
            );

            var item = construction.GetComponent<CompletedConstructionItem>();

            GameEvents.UserEvents.RaiseItemCollected(item.ItemData, 1);
            // TODO DEBUG 
            GameEvents.ScreenshotEvents.RaiseScreenshotRequested(item.gameObject, item.ItemData.Name);
        }

        /// <summary>
        /// Сохранить текущую конструкцию
        /// </summary>
        public void SaveConstruction()
        {
            this.constructionModel.Save();
        }

        /// <summary>
        /// Загрузить конструкцию
        /// </summary>
        public void LoadConstruction()
        {
            this.constructionModel.Load();
        }

        #endregion
    }
}