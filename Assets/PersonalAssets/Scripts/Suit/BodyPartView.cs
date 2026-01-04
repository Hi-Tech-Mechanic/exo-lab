namespace ExoLab
{
    using Exception;
    using UnityEngine;
    using UnityEngine.UI;

    internal class BodyPartView
    {
        private Image bodyImage;

        public BodyPartView(Image bodyImage, BodyPartModel model)
        {
            this.bodyImage = bodyImage;
            model.HealthChanged += DisplayHealthState;
        }

        /// <summary>
        /// Зеленый цвет
        /// </summary>
        private float maxHue = 120;

        /// <summary>
        /// Красный цвет
        /// </summary>
        private float lowHue = 0;

        private void DisplayHealthState(double currentHealth, double maxHealth)
        {
            var coefficient = currentHealth / maxHealth;

            var resultHue = this.maxHue * coefficient;
            if (resultHue < this.lowHue)
                resultHue = this.lowHue;

            var targetColor = Color.HSVToRGB(lowHue, 1, 1);
            this.bodyImage.color = targetColor;
        }
    }
}
