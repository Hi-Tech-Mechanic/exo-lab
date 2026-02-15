namespace ExoLab.UI
{
    using ExoLab.Assembly;
    using ExoLab.Data;
    using ExoLab.Helpers;
    using ExoLab.Interaction;
    using ExoLab.StructuralСomponents;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using Constants = Constants.Constants;

    /// <summary>
    /// Надстройка над обычным <see cref="DraggableInventoryItem"/> для комплектующих
    /// имеет возможность после перетаскивания встраиваться в конструкцию
    /// </summary>
    public class DraggableComponent : DraggableInventoryItem
    {
        [Tooltip ("Материал для предпросмотра соединяемого компонента")]
        [SerializeField] private Material previewMaterial;

        /// <summary>
        /// Первоначальный материал компонента
        /// </summary>
        private Material startMaterial;

        /// <summary>
        /// Корневой узел конструкции, пустышка
        /// </summary>
        private GameObject constructionRoot;

        /// <summary>
        /// Отрисованные элементы, точки крепления, модели компонентов
        /// </summary>
        private List<GameObject> visualizedElements = new List<GameObject>();
        
        /// <summary>
        /// Данный объект
        /// </summary>
        private AssemblyComponentBase? cachedCurrentComponent;
        /// <summary>
        /// Массив всех компонентов во всей конструкции
        /// </summary>
        private List<AssemblyComponentBase> cachedAllComponents = new List<AssemblyComponentBase>();

        protected override void Awake()
        {
            base.Awake();

            this.constructionRoot = Caches.Instance.ConstructionRoot;
        }

        private void OnEnable()
        {
            AssemblyModesController.OnChangedConstructionRoot += this.UpdateConstructionRoot;
        }

        private void OnDisable()
        {
            AssemblyModesController.OnChangedConstructionRoot -= this.UpdateConstructionRoot;
        }

        private void UpdateConstructionRoot(GameObject newConstructionRoot)
        {
            this.constructionRoot = newConstructionRoot;
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);

            if (IsValidState() == false)
                return;

            this.CacheFields();
            this.DrawPreviewComponent();
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);

            if (IsValidState() == false)
                return;

            foreach (var hoveredItem in eventData.hovered)
            {
                if (hoveredItem.tag.Equals(Constants.Tags.AssemblyZone) == false)
                    continue;

                var success = this.TryBindComponent();

                if (success)
                {
                    Destroy(this.gameObject);
                }
            }

            this.ClearCache();
        }

        /// <summary>
        /// Попытаться привязать компонент
        /// </summary>
        private bool TryBindComponent()
        {
            if (this.cachedCurrentComponent == null || this.cachedAllComponents.Count == 0 || this.visualizedElements.Count == 0)
                return false;

            GameObject selectedComponent = cachedCurrentComponent.gameObject;

            // Если множественный выбор
            if (visualizedElements.Count > 1)
             {
                var ray = Caches.Instance.AssemblyCamera.ScreenPointToRay(Input.mousePosition);
                var hits = Physics.RaycastAll(ray, 100F);

                foreach (var hit in hits)
                {
                    var hitedObject = hit.transform.gameObject;
                    if (hitedObject.layer != ((int)Constants.Layers.Component))
                        continue;

                    selectedComponent = hitedObject;
                    selectedComponent.GetComponent<Renderer>().material = this.startMaterial;
                    this.visualizedElements.Remove(selectedComponent);
                    break;
                }
            }

            foreach (var parentComponent in this.cachedAllComponents)
            {
                if (this.cachedCurrentComponent.CanBeAttached(parentComponent.TypedItemData))
                {
                    var currentItemObject = Instantiate(selectedComponent, this.constructionRoot.transform);
                    var newCurrentComponentData = currentItemObject.GetComponent<AssemblyComponentBase>();
                    newCurrentComponentData.AttachAnObject(parentComponent.gameObject);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Отрисовать затравку присоединяемого компонента
        /// </summary>
        private void DrawPreviewComponent()
        {
            if (this.cachedCurrentComponent == null || cachedAllComponents.Count == 0)
                return;

            foreach (var targetComponent in this.cachedAllComponents)
            {
                var option = this.cachedCurrentComponent.TryGetAttachmentOptionAfterCompared(targetComponent.TypedItemData);
                if (option != null)
                {
                    var previewModel = Instantiate(this.cachedCurrentComponent.TypedItemData.Prefab, targetComponent.transform);
                    previewModel.transform.localPosition = option.AttachmentPoint;
                    
                    var previewModelMaterial = previewModel.GetComponent<Renderer>();
                    this.startMaterial = previewModelMaterial.material;
                    previewModelMaterial.material = this.previewMaterial;

                    this.visualizedElements.Add(previewModel);
                }
            }
        }

        /// <summary>
        /// Кешируем после взятия предмета непосредственно, для экономии ресурсов
        /// </summary>
        private void CacheFields()
        {
            this.cachedCurrentComponent = this.Item.Prefab.GetComponent<AssemblyComponentBase>();
            this.cachedAllComponents = this.constructionRoot.transform.GetComponentsInChildren<AssemblyComponentBase>().ToList();
        }

        /// <summary>
        /// Подчищаем ненужное, но не все 
        /// </summary>
        private void ClearCache()
        {
            foreach (var element in this.visualizedElements)
            {
                Destroy(element);
            }

            this.visualizedElements.Clear();
            this.cachedAllComponents.Clear();
        }

        /// <summary>
        /// Базовые проверки на валидность состояния
        /// </summary>
        /// <returns></returns>
        private bool IsValidState()
        {
            if (this.constructionRoot.activeInHierarchy == false)
                return false;

            return true;
        }
    }
}
