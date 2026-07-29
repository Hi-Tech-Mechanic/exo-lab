namespace ExoLab.UI
{
    using TMPro;
    using Unity.VisualScripting;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Визуализатор информации о предмете
    /// </summary>
    internal class ItemInfoPanel : MonoBehaviour
    {
        [SerializeField]
        private Color headerColor; // todo автоматически подтягиваться будет из качества предмета

        [SerializeField]
        private TextMeshProUGUI headerText;

        [SerializeField]
        private TextMeshProUGUI descriptionText;

        [SerializeField]
        [Tooltip("Объект в котором должна находиться картинка предмета")]
        private Image iconHolder;

        [SerializeField]
        [Tooltip ("Объект в котором должен хранится список характеристик предмета")]
        private GameObject statsHolder;

        [SerializeField]
        [Tooltip ("Префаб строки с характеристикой")]
        private GameObject statTextPrefab;

        private string cachedHexColor;

        private RectTransform rectTransform;
        private float allStatsHeight;

        private void Awake()
        {
            this.cachedHexColor = this.headerColor.ToHexString();
            this.rectTransform = this.GetComponent<RectTransform>();
        }

        public void Initialize(ItemBase itemBase)
        {
            this.FillStats(itemBase);
            this.SetHeaderText(itemBase.ItemData.Name);
            this.SetDescriptionText(itemBase.ItemData.Description);
            this.SetIcon(itemBase.ItemData.Icon);

            this.SetPanelSize();
        }

        private void SetPanelSize()
        {
            var startStatsHeight = this.statsHolder.GetComponent<RectTransform>().rect.height;

            if (this.allStatsHeight > startStatsHeight)
            {
                var newHeight = (this.allStatsHeight - startStatsHeight);
                this.rectTransform.sizeDelta += new Vector2(0, newHeight);
            }
        }

        private void FillStats(ItemBase itemBase)
        {
            var statsData = itemBase.ItemData.GetAllStats();
            var prefabHeight = this.statTextPrefab.GetComponent<RectTransform>().rect.height;
            this.allStatsHeight = prefabHeight * statsData.Count;

            foreach (var statData in statsData)
            {
                var statObject = Instantiate(this.statTextPrefab, this.statsHolder.transform);
                var textDisplayer = statObject.GetComponent<TextDisplayer>();
                var text = $"{statData.Name}: {statData.Value}";
                textDisplayer.SetText(text);
            }
        }

        private void SetHeaderText(string text)
        {
            this.headerText.text = $"<color=#{this.cachedHexColor}>{text}</color>";
        }

        private void SetDescriptionText(string text)
        {
            this.descriptionText.text = text;
        }

        private void SetIcon(Sprite icon)
        {
            this.iconHolder.sprite = icon;
        }
    }
}
