namespace Assets.PersonalAssets.Scripts.UI.Base
{
    using UnityEngine.EventSystems;
    using UnityEngine;

    /// <summary>
    /// Зачаток перетаскиваемого элемента
    /// </summary>
    public abstract class DraggableElementAbstract : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public abstract void OnBeginDrag(PointerEventData eventData);

        public abstract void OnDrag(PointerEventData eventData);

        public abstract void OnEndDrag(PointerEventData eventData);
    }
}
