namespace ExoLab.UI
{
    using ExoLab.StructuralСomponents;
    using UnityEngine;
    using UnityEngine.EventSystems;

    /// <summary>
    /// Надстройка над обычным <see cref="DraggableInventoryItem"/> для комплектующих
    /// имеет возможность после перетаскивания встраиваться в конструкцию
    /// </summary>
    public class DraggableComponent : DraggableInventoryItem
    {
        private GameObject assemblyParent;

        protected override void Awake()
        {
            base.Awake();
            this.assemblyParent = GameObject.FindGameObjectWithTag("GameController"); //todo не хорошо что каждый раз берется так, нужен кеш
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);

            foreach (var hoveredItem in eventData.hovered)
            {
                if (hoveredItem.tag.Equals(Constants.Constants.Tags.AssemblyZone) == false)
                    continue;

                if (this.assemblyParent.activeInHierarchy == false)
                    continue;

                if (this.TryBindComponent())
                {
                    Destroy(this.gameObject);
                }
            }
        }

        /// <summary>
        /// Попытаться привязать компонент
        /// </summary>
        private bool TryBindComponent()
        {
            var currentComponentData = this.Item.Prefab.GetComponent<AssemblyComponentBase>();
            var targetComponentChilds = this.assemblyParent.transform.GetComponentsInChildren<AssemblyComponentBase>();
            
            foreach (var targetComponent in targetComponentChilds)
            {
                if (currentComponentData == null || targetComponentChilds == null)
                    continue;

                if (currentComponentData.CanBeAttached(targetComponent.TypedItemData))
                {
                    var currentItemObject = Instantiate(this.Item.Prefab, this.assemblyParent.transform);
                    var newCurrentComponentData = currentItemObject.GetComponent<AssemblyComponentBase>();
                    newCurrentComponentData.AttachAnObject(targetComponent.gameObject);
                    return true;
                }
            }

            return false;
        }
    }
}
