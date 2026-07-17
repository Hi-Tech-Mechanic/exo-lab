namespace ExoLab.UI
{
    using System;

    /// <summary>
    /// Вызывает <see cref="ItemInfoPanel"/> при наведении курсора на предмет
    /// </summary>
    public class InteractionItem : InteractionElementAbstract
    {
        [NonSerialized] private ItemBase? cachedItemBase;

        protected override void ActionAfterClick()
        {
            if (this.cachedItemBase == null)
            {
                this.cachedItemBase = this.GetComponent<ItemBase>();
            }

            GameEvents.UserEvents.RaiseItemClicked(this.cachedItemBase);
        }

        protected override void ActionAfterPointerEnter()
        {
            if (this.cachedItemBase == null)
            {
                this.cachedItemBase = this.GetComponent<ItemBase>();
            }

            GameEvents.UserEvents.RaiseItemHovered(this.cachedItemBase);
        }

        protected override void ActionAfterPointerMove()
        {
            GameEvents.UserEvents.RaiseItemMoved();
        }

        protected override void ActionAfterPointerExit()
        {
            GameEvents.UserEvents.RaiseItemUnhovered();
        }
    }
}
