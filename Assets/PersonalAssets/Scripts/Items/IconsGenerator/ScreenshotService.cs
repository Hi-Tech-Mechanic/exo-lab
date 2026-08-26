namespace ExoLab.Items
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Coordinates the renderer and storage to create screenshots.
    /// </summary>
    public class ScreenshotService : IScreenshotService
    {
        private readonly IScreenshotRenderer renderer;
        private readonly IScreenshotStorage storage;

        /// <summary>
        /// Creates a new screenshot service.
        /// </summary>
        /// <param name="renderer">Renderer used to capture objects</param>
        /// <param name="storage">Storage used to save or convert textures</param>
        public ScreenshotService(IScreenshotRenderer renderer, IScreenshotStorage storage)
        {
            this.renderer = renderer;
            this.storage = storage;
        }

        /// <summary>
        /// Captures the object screenshot and saves it to a file.
        /// </summary>
        /// <param name="target">Object to capture</param>
        /// <param name="fileName">File name without extension</param>
        /// <returns>Full path to the saved file, or null on error</returns>
        public string CaptureToFile(GameObject target, string fileName)
        {
            try
            {
                var texture = this.renderer.Render(target);
                return this.storage.SaveAsPng(texture, fileName);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{nameof(ScreenshotService)}] Failed to capture screenshot: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Captures the object screenshot and returns it as a Sprite.
        /// </summary>
        /// <param name="target">Object to capture</param>
        /// <returns>Sprite with the screenshot, or null on error</returns>
        public Sprite CaptureAsSprite(GameObject target)
        {
            try
            {
                var texture = this.renderer.Render(target);
                return this.storage.ToSprite(texture);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{nameof(ScreenshotService)}] Failed to capture screenshot: {ex.Message}");
                return null;
            }
        }
    }
}