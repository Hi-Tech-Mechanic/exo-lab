namespace ExoLab.UI
{
    using ExoLab.StructuralСomponents;
    using TMPro;
    using Unity.VisualScripting;
    using UnityEngine;
    using UnityEngine.UI;

    internal class ItemInfoInspector : MonoBehaviour
    {
        [SerializeField]
        private Color headerColor; // todo автоматически подтягиваться будет из качества предмета

        [SerializeField]
        private TextMeshProUGUI headerText;

        [SerializeField]
        private TextMeshProUGUI descriptionText;

        [SerializeField]
        private Image iconHolder;

        [SerializeField]
        private GameObject statsHolder;

        [SerializeField]
        private GameObject statPrefab;

        private string cachedHexColor;

        private void Awake()
        {
            this.cachedHexColor = this.headerColor.ToHexString();
        }

        public void Initialize(AssemblyComponentBase assemblyComponent)
        {
            var stats = assemblyComponent.GetTranslatedNumericStats();

            foreach (var item in stats)
            {
                var stat = Instantiate(this.statPrefab, this.statsHolder.transform);
                var textDisplayer = stat.GetComponent<TextDisplayer>();
                var text = $"{item.Key}: {item.Value}";
                textDisplayer.SetText(text);
            }

            this.SetHeaderText(assemblyComponent.TypedItemData.Name);
            this.SetDescriptionText(assemblyComponent.TypedItemData.Description);
            this.SetIcon(assemblyComponent.TypedItemData.Icon);
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
