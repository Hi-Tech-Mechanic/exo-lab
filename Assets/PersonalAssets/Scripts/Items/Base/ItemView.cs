namespace ExoLab.UI
{
    using TMPro;
    using UnityEngine.UI;
    using ExoLab.Constants;

    /// <summary>
    /// Предмет который может отображаться в слоте
    /// </summary>
    public class ItemView : Item
    {
        private TextMeshProUGUI nameText;
        private TextMeshProUGUI amountText;

        private Image iconHolder;

        protected override void Awake()
        {
            base.Awake();
            this.Initialize();

            this.FillName();
            this.FillIcon();
            this.FillAmount(10); //todo временно test
        }

        private void Initialize()
        {
            var texts = GetComponentsInChildren<TextMeshProUGUI>();

            foreach (var text in texts)
            {
                if (text.name.Equals("Txt_ItemName"))
                {
                    this.nameText = text;
                }
                else if (text.name.Equals("Txt_Amount"))
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
