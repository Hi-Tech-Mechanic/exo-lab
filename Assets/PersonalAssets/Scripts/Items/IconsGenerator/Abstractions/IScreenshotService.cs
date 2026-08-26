namespace ExoLab.Items
{
    using UnityEngine;

    /// <summary>
    /// High-level service for creating screenshots of scene objects.
    /// Coordinates the renderer and storage.
    /// </summary>
    public interface IScreenshotService
    {
        /// <summary>
        /// Captures the object screenshot and saves it to a file.
        /// </summary>
        /// <param name="target">Object to capture</param>
        /// <param name="fileName">File name without extension</param>
        /// <returns>Full path to the saved file, or null on error</returns>
        string CaptureToFile(GameObject target, string fileName);

        /// <summary>
        /// Captures the object screenshot and returns it as a Sprite.
        /// </summary>
        /// <param name="target">Object to capture</param>
        /// <returns>Sprite with the screenshot, or null on error</returns>
        Sprite CaptureAsSprite(GameObject target);
    }
}