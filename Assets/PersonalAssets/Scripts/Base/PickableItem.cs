namespace ExoLab.Interaction
{
    using UnityEngine;

    [RequireComponent(typeof(ItemBase))]
    public class PickableItem : InteractiveObject
    {
        public override void Interact()
        {
            var item = this.GetComponent<ItemBase>();
            item.Pickup();
        }

        protected override string GetTooltipText()
        {
            return "Взять";
        }
    }
}
