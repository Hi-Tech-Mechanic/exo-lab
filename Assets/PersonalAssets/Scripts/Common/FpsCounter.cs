namespace ExoLab.Tools
{
    using TMPro;
    using UnityEngine;

    public class FpsCounter : MonoBehaviour
    {
        private const float UpdateInterval = 0.5f;

        [SerializeField] private TextMeshProUGUI fpsLabel;

        private float lastUpdate;
        private int frameCount;
        private float fps;

        private void Update()
        {
            this.frameCount++;
            var currentInterval = Time.realtimeSinceStartup - lastUpdate;

            if (currentInterval >= UpdateInterval)
            {
                this.fps = this.frameCount / (currentInterval);
                this.frameCount = 0;
                this.lastUpdate = Time.realtimeSinceStartup;
            }
        }

        private void OnGUI()
        {
            this.SetValue();
        }

        private void SetValue()
        {
            this.fpsLabel.text = $"FPS: {this.fps:F0}";
        }
    }
}
