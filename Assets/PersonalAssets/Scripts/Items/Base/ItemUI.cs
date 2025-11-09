namespace ExoLab.UI
{
    using TMPro;
    using UnityEngine;

    public class ItemUI : Item
    {
        public Sprite Icon { get; set; }

        private TextMeshProUGUI nameText;

        protected override void Awake()
        {
            base.Awake();

            this.nameText = GetComponentInChildren<TextMeshProUGUI>();
            this.FillName();
        }

        private void FillName()
        {
            this.nameText.text = this.Name;
        }
    }
}
