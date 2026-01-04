namespace ExoLab.Helpers
{
    using TMPro;

    public static class UnityExtensions
    {
        public static void SetTextIfChanged(this TextMeshProUGUI textComponent, string newText)
        {
            if (textComponent.text != newText)
                textComponent.text = newText;
        }
    }
}
