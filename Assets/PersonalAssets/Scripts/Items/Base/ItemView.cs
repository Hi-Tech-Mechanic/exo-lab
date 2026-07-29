namespace ExoLab.UI
{
    using TMPro;
    using UnityEngine.UI;
    using ExoLab.Constants;
    using ExoLab.Data;
    using UnityEngine;

    /// <summary>
    /// Визуальная часть предмета
    /// </summary>
    [RequireComponent(typeof(ItemBase))]
    public class ItemView : MonoBehaviour
    {
        private const string nameIdentifier = "Txt_ItemName";
        private const string amountIdentifier = "Txt_Amount";

        private IItemData itemData;

        private TextMeshProUGUI nameText;
        private TextMeshProUGUI amountText;
        private Image iconHolder;

        protected void Start()
        {
            this.InitializeComponents();

            this.FillName();
            this.FillIcon();
        }

        public void SetItemData(StoredItem storedItem)
        {
            this.itemData = storedItem.ItemData;
            this.FillAmount(storedItem.Amount);
        }

        private void InitializeComponents()
        {
            var texts = GetComponentsInChildren<TextMeshProUGUI>();

            foreach (var text in texts)
            {
                if (text.name.Equals(nameIdentifier))
                {
                    this.nameText = text;
                }
                else if (text.name.Equals(amountIdentifier))
                {
                    this.amountText = text;
                }
            }

            var images = GetComponentsInChildren<Image>();

            foreach (var image in images)
            {
                if (image.tag.Equals(Constants.Tags.Icon))
                {
                    this.iconHolder = image;
                }
            }
        }

        private void InitializeComponentsIfNotExist()
        {
            if (this.nameText == null || this.amountText == null)
            {
                this.InitializeComponents();
            }
        }

        private void FillName()
        {
            this.InitializeComponentsIfNotExist();

            this.nameText.text = this.itemData.Name;
        }

        private void FillIcon()
        {
            this.InitializeComponentsIfNotExist();

            this.iconHolder.sprite = this.itemData.Icon;
        }

        private void FillAmount(int amount)
        {
            this.InitializeComponentsIfNotExist();

            this.amountText.text = amount.ToString();
        }
    }
}
