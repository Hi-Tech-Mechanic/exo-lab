namespace Assets.PersonalAssets.Scripts.SuitComponents.ScriptableObjects
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "DisplayedItemData", menuName = "Inventory/Displayed item data")]
    public class DisplayedItemData : ItemData
    {
        public Sprite ItemIcon;
        public Color QualityColor;
    }
}
