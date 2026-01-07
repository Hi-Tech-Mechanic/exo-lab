namespace ExoLab.UI
{
    using ExoLab.StructuralСomponents;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using static ExoLab.Constants.Constants;

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

                if (this.assemblyParent.activeInHierarchy == true)
                {
                    this.BindComponent();
                    Destroy(this.gameObject);
                }
            }
        }

        /// <summary>
        /// Привязать компонент
        /// </summary>
        private void BindComponent()
        {
            var currentItem = this.Item.Prefab.GetComponent<AssemblyComponentBase>();
            var targetComponentChilds = this.assemblyParent.transform.GetComponentsInChildren<AssemblyComponentBase>();
            
            foreach (var child in targetComponentChilds)
            {
                if (currentItem == null || targetComponentChilds == null)
                    continue;

                if (currentItem.CanBeAttached(child.TypedItemData))
                {
                    var trueItem = Instantiate(this.Item.Prefab, this.assemblyParent.transform);
                    trueItem.GetComponent<AssemblyComponentBase>().AttachAnObject(child.gameObject);
                }
            }
        }
    }
}
