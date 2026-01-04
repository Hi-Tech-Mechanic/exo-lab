namespace Assets.PersonalAssets.Scripts.UI
{
    using ExoLab.Helpers;
    using TMPro;
    using UnityEngine;

    /// <summary>
    /// Раскулачить данный класс, худ в одном классе не должен быть
    /// </summary>
    public class HUD : MonoBehaviour
    {
        public static HUD Instance { get; private set; }

        [SerializeField]
        private TextMeshProUGUI tooltipText;

        private HUD()
        {
            Instance = this;
        }

        public void DisplayTooltipText(string text)
        {
            this.tooltipText.SetTextIfChanged(text);
            this.tooltipText.gameObject.SetActive(true);
        }

        public void HideTooltipText()
        {
            this.tooltipText.gameObject.SetActive(false);
        }
    }
}
