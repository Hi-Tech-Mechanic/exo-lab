namespace ExoLab.UI
{
    using ExoLab.Data;
    using ExoLab.Helpers;
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
        [SerializeField] private GameObject connectionPointPrefab;
        
        /// <summary>
        /// Корневой узел конструкции, пустышка
        /// </summary>
        private GameObject constructionRoot;

        /// <summary>
        /// Отрисованные точки крепления
        /// </summary>
        private List<GameObject> connectionPoints = new List<GameObject>();
        
        private AssemblyComponentBase? cachedAssemblyComponent;
        private List<AssemblyComponentBase> cachedTargetAssemblyComponents = new List<AssemblyComponentBase>();

        protected override void Awake()
        {
            base.Awake();

            this.constructionRoot = Caches.Instance.ConstructionRoot;
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);

            if (IsValidState() == false)
                return;

            this.CacheFields();
            this.DrawConnectionPoints();
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

                if (this.TryBindComponent())
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
            if (this.cachedAssemblyComponent == null || cachedTargetAssemblyComponents.Count == 0)
                return false;

            foreach (var targetComponent in this.cachedTargetAssemblyComponents)
            {
                if (this.cachedAssemblyComponent.CanBeAttached(targetComponent.TypedItemData))
                {
                    var currentItemObject = Instantiate(this.Item.Prefab, this.constructionRoot.transform);
                    var newCurrentComponentData = currentItemObject.GetComponent<AssemblyComponentBase>();
                    newCurrentComponentData.AttachAnObject(targetComponent.gameObject);
                    return true;
                }
            }

            return false;
        }

        private void DrawConnectionPoints()
        {
            if (this.cachedAssemblyComponent == null || cachedTargetAssemblyComponents.Count == 0)
                return;

            foreach (var targetComponent in this.cachedTargetAssemblyComponents)
            {
                if (this.cachedAssemblyComponent.CanBeAttached(targetComponent.TypedItemData))
                {
                    foreach (var option in cachedAssemblyComponent.TypedItemData.AttachmentOptions)
                    {
                        var pivotPoint = this.cachedAssemblyComponent.gameObject.TryGetChildWithTag(Constants.Tags.PivotPoint);
                        if (pivotPoint == null)
                        {
                            Debug.LogError($"Не найден {nameof(Constants.Tags.PivotPoint)} у компонента");
                            continue;
                        }

                        var connectionPoint = Instantiate(this.connectionPointPrefab, targetComponent.transform);
                        var newPosition = option.AttachmentPoint + pivotPoint.transform.localPosition;
                        connectionPoint.transform.localPosition = newPosition;
                        connectionPoints.Add(connectionPoint);
                    }
                }
            }
        }

        /// <summary>
        /// Кешируем после взятия предмета непосредственно, для экономии ресурсов
        /// </summary>
        private void CacheFields()
        {
            this.cachedAssemblyComponent = this.Item.Prefab.GetComponent<AssemblyComponentBase>();
            this.cachedTargetAssemblyComponents = this.constructionRoot.transform.GetComponentsInChildren<AssemblyComponentBase>().ToList();
        }

        /// <summary>
        /// Подчищаем ненужное, но не все 
        /// </summary>
        private void ClearCache()
        {
            foreach (var point in this.connectionPoints)
            {
                Destroy(point);
            }

            this.connectionPoints.Clear();
            this.cachedTargetAssemblyComponents.Clear();
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
