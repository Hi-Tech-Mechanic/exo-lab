namespace ExoLab.Items
{
    using ExoLab.Constants;
    using System.IO;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// Stores screenshots as PNG files on disk.
    /// </summary>
    public class FileScreenshotStorage : IScreenshotStorage
    {
        private readonly string screenshotsFolderPath;

        /// <summary>
        /// Creates a new file-based screenshot storage.
        /// </summary>
        public FileScreenshotStorage()
        {
            this.screenshotsFolderPath = Path.Combine(
                Application.persistentDataPath,
                Constants.GameResourcesPath.ScreenshotsFolder
            );

            this.EnsureFolderExists();
        }

        /// <summary>
        /// Full path to the screenshots folder.
        /// </summary>
        public string ScreenshotsFolderPath => this.screenshotsFolderPath;

        /// <summary>
        /// Saves the texture as a PNG file.
        /// </summary>
        /// <param name="texture">Texture to save</param>
        /// <param name="fileName">File name without extension</param>
        /// <returns>Full path to the saved file</returns>
        public string SaveAsPng(Texture2D texture, string fileName)
        {
            this.EnsureFolderExists();

            var safeFileName = this.SanitizeFileName(fileName);
            var filePath = Path.Combine(this.screenshotsFolderPath, $"{safeFileName}.png");

            var bytes = texture.EncodeToPNG();
            File.WriteAllBytes(filePath, bytes);

            Object.DestroyImmediate(texture);

            Debug.Log($"[{nameof(FileScreenshotStorage)}] Screenshot saved: {filePath}");

            return filePath;
        }

        /// <summary>
        /// Converts the texture into a Sprite.
        /// </summary>
        /// <param name="texture">Texture to convert</param>
        /// <returns>Sprite created from the texture</returns>
        public Sprite ToSprite(Texture2D texture)
        {
            return Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5F, 0.5F)
            );
        }

        /// <summary>
        /// Cleans the file name from invalid characters.
        /// </summary>
        /// <param name="fileName">Original file name</param>
        /// <returns>Sanitized file name</returns>
        private string SanitizeFileName(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var safeName = new string(fileName
                .Select(c => invalidChars.Contains(c) ? '_' : c)
                .ToArray());

            return safeName;
        }

        /// <summary>
        /// Ensures the screenshots folder exists.
        /// </summary>
        private void EnsureFolderExists()
        {
            if (!Directory.Exists(this.screenshotsFolderPath))
            {
                Directory.CreateDirectory(this.screenshotsFolderPath);
            }
        }
    }
}