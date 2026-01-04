namespace ExoLab.UI
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    /// <summary>
    /// Описание элемента который откликается при наведении мышкой
    /// </summary>
    public abstract class HoverableElementAbstract :
        MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerClickHandler,
        IPointerMoveHandler
    {
        protected abstract void ActionAfterClick();

        protected abstract void ActionAfterPointerExit();

        protected abstract void ActionAfterPointerEnter();

        protected abstract void ActionAfterPointerMove();

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (CursorIsVisible() == false)
                return;

            this.ActionAfterClick();
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (CursorIsVisible() == false)
                return;

            this.ActionAfterPointerEnter();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (CursorIsVisible() == false)
                return;

            this.ActionAfterPointerExit();
        }

        public virtual void OnPointerMove(PointerEventData eventData)
        {
            if (CursorIsVisible() == false)
                return;

            this.ActionAfterPointerMove();
        }

        /// <summary>
        /// Пропускаем выполнение если курсор не виден.
        /// Иначе можно будет взаимодействовать с элементом даже не видя курсор
        /// </summary>
        /// <returns></returns>
        private static bool CursorIsVisible()
        {
            return Cursor.visible;
        }

      
    }
}
