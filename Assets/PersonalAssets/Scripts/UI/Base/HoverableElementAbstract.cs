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
        IPointerClickHandler
    {
        protected abstract void ActionAfterClick();

        protected abstract void ActionAfterPointerExit();

        protected abstract void ActionAfterPointerEnter();

        public void OnPointerClick(PointerEventData eventData)
        {
            if (CursorIsVisible() == false)
                return;

            this.ActionAfterClick();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (CursorIsVisible() == false)
                return;

            this.ActionAfterPointerEnter();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (CursorIsVisible() == false)
                return;

            this.ActionAfterPointerExit();
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
