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
            GameEvents.RaiseItemCollected(itemData);

            Destroy(this.gameObject);
        }

        protected override string GetTooltipText()
        {
            return "Взять";
        }
    }
}
