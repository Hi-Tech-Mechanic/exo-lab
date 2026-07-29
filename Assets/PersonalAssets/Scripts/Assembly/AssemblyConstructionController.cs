namespace ExoLab.Assembly
{
    using ExoLab.Assembly.Services;
    using ExoLab.StructuralСomponents;
    using UnityEngine;

    /// <summary>
    /// Контроллер сборки конструкции (точка входа).
    /// Координирует работу Model и View, обрабатывает события.
    /// </summary>
    public class AssemblyConstructionController : MonoBehaviour
    {
        [SerializeField] private AssemblyConstructionView constructionView;
        [Tooltip("Place where the completed construction appears")]
        [SerializeField] private Transform constructionParent;

        private IConstructionModel constructionModel;

        /// <summary>
        /// Текущая модель конструкции
        /// </summary>
        public IConstructionModel ConstructionModel => this.constructionModel;

        private void Awake()
        {
            this.constructionModel = new ConstructionModelBase();
        }

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

            var stats = this.constructionModel.GetAllCharacteristicSums();
            this.constructionView.CreateStatRows(stats);
        }

        /// <summary>
        /// Завершить сборку и получить GameObject готового предмета
        /// </summary>
        public void CompleteConstruction()
        {
            var construction = ConstructionCompletionService.CompleteConstruction(
                this.constructionModel,
                this.constructionParent
            );

            var item = construction.GetComponent<CompletedConstructionItem>();

            GameEvents.UserEvents.RaiseItemCollected(item.ItemData, 1);
        }

        /// <summary>
        /// Завершить сборку и получить GameObject готового предмета
        /// </summary>
        public GameObject CompleteConstruction(Transform parent = null)
        {
            return ConstructionCompletionService.CompleteConstruction(
                this.constructionModel,
                parent
            );
        }

        /// <summary>
        /// Собрать конструкцию в сцене, инстанциировав префабы всех компонентов
        /// </summary>
        public void AssembleInScene(Transform root)
        {
            ConstructionCompletionService.AssembleInScene(
                this.constructionModel,
                root
            );
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
    }
}