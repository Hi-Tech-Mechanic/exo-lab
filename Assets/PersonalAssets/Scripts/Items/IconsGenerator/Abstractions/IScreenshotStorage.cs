namespace ExoLab.Items
{
    using UnityEngine;

    /// <summary>
    /// Responsible for saving a screenshot texture.
    /// </summary>
    public interface IScreenshotStorage
    {
        /// <summary>
        /// Saves the texture as a PNG file.
        /// </summary>
        /// <param name="texture">Texture to save</param>
        /// <param name="fileName">File name without extension</param>
        /// <returns>Full path to the saved file</returns>
        string SaveAsPng(Texture2D texture, string fileName);

        /// <summary>
        /// Converts the texture into a Sprite.
        /// </summary>
        /// <param name="texture">Texture to convert</param>
        /// <returns>Sprite created from the texture</returns>
        Sprite ToSprite(Texture2D texture);
    }
}