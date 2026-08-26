namespace ExoLab.Items
{
    using UnityEngine;

    /// <summary>
    /// Responsible for rendering an object into a texture.
    /// </summary>
    public interface IScreenshotRenderer
    {
        /// <summary>
        /// Renders the target object into a Texture2D.
        /// </summary>
        /// <param name="target">Object to render</param>
        /// <returns>Texture with the object screenshot</returns>
        Texture2D Render(GameObject target);
    }
}