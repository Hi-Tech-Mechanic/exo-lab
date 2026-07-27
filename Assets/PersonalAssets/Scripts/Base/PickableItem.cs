namespace ExoLab.Interaction
{
    using UnityEngine;

    [RequireComponent(typeof(ItemBase))]
    public class PickableItem : InteractiveObject
    {
        public override void Interact()
        {
            var item = this.GetComponent<ItemBase>();
            var itemData = item.GetBaseItemData();
            GameEvents.UserEvents.RaiseItemCollected(itemData, 1); // TODO пока по одной штуке всегда

            Destroy(this.gameObject);
        }

        protected override string GetTooltipText()
        {
            return "Взять";
        }
    }
}
