namespace ExoLab.UI
{
    using TMPro;
    using UnityEngine.UI;
    using ExoLab.Constants;
    using ExoLab.Data;

    /// <summary>
    /// Визуальная часть предмета
    /// </summary>
    public class ItemView : ItemAbstract<ItemData>
    {
        private const string nameIdentifier = "Txt_ItemName";
        private const string amountIdentifier = "Txt_Amount";

        private TextMeshProUGUI nameText;
        private TextMeshProUGUI amountText;

        private Image iconHolder;

        protected override void Start()
        {
            base.Start();
            this.Initialize();

            this.FillAmount(10); //todo временно test
        }

        private void Initialize()
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

            this.FillName();
            this.FillIcon();
        }

        private void FillName()
        {
            this.nameText.text = this.Name;
        }

        private void FillIcon()
        {
            this.iconHolder.sprite = this.itemData.Icon;
        }

        private void FillAmount(int amount)
        {
            this.amountText.text = amount.ToString();
        }
    }
}
