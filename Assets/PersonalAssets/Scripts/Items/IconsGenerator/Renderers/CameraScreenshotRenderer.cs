namespace ExoLab.Items
{
    using UnityEngine;

    /// <summary>
    /// Renders screenshots using a dedicated Camera.
    /// The camera renders in the background and does not block the main camera.
    /// </summary>
    public class CameraScreenshotRenderer : IScreenshotRenderer
    {
        private readonly Camera renderCamera;
        private readonly int textureSize;
        private readonly LayerMask renderLayer;

        private RenderTexture workingRenderTexture;

        /// <summary>
        /// Creates a new camera-based screenshot renderer.
        /// </summary>
        /// <param name="camera">Camera used for rendering</param>
        /// <param name="textureSize">Texture size (width and height)</param>
        /// <param name="renderLayer">Layer on which objects are rendered</param>
        public CameraScreenshotRenderer(Camera camera, int textureSize, LayerMask renderLayer)
        {
            this.renderCamera = camera;
            this.textureSize = textureSize;
            this.renderLayer = renderLayer;

            this.ConfigureCamera();
        }

        /// <summary>
        /// Renders the target object into a Texture2D.
        /// </summary>
        /// <param name="target">Object to render</param>
        /// <returns>Texture with the object screenshot</returns>
        public Texture2D Render(GameObject target)
        {
            var renderTexture = this.GetOrCreateRenderTexture();
            this.renderCamera.targetTexture = renderTexture;

            // Temporarily switch the object and all its children to the render layer
            var renderLayerIndex = LayerHelper.GetFirstEnabledLayer(this.renderLayer);
            var originalLayers = LayerHelper.CollectLayers(target);
            LayerHelper.SetLayerRecursively(target, renderLayerIndex);

            this.renderCamera.Render();

            // Restore the original layers
            LayerHelper.RestoreLayers(originalLayers);

            var texture = this.ReadPixelsFromRenderTexture(renderTexture);

            this.renderCamera.targetTexture = null;

            return texture;
        }

        /// <summary>
        /// Releases the working RenderTexture resources.
        /// </summary>
        public void Cleanup()
        {
            if (this.workingRenderTexture != null)
            {
                this.workingRenderTexture.Release();
                Object.DestroyImmediate(this.workingRenderTexture);
                this.workingRenderTexture = null;
            }
        }

        /// <summary>
        /// Configures the camera to render in the background.
        /// </summary>
        private void ConfigureCamera()
        {
            this.renderCamera.cullingMask = this.renderLayer;
            this.renderCamera.clearFlags = CameraClearFlags.SolidColor;
            this.renderCamera.backgroundColor = Color.clear;
            this.renderCamera.orthographic = true;
            this.renderCamera.orthographicSize = 1.0f;

            // The camera renders only manually via Render(),
            // so disable it to not block the main camera
            this.renderCamera.enabled = false;

            // Negative depth renders behind the main camera
            this.renderCamera.depth = -100;

            // The camera should not output to the screen
            this.renderCamera.targetTexture = null;
        }

        /// <summary>
        /// Gets or creates the working RenderTexture.
        /// </summary>
        private RenderTexture GetOrCreateRenderTexture()
        {
            if (this.workingRenderTexture == null ||
                this.workingRenderTexture.width != this.textureSize ||
                this.workingRenderTexture.height != this.textureSize)
            {
                if (this.workingRenderTexture != null)
                {
                    this.workingRenderTexture.Release();
                    Object.DestroyImmediate(this.workingRenderTexture);
                }

                this.workingRenderTexture = new RenderTexture(
                    this.textureSize,
                    this.textureSize,
                    24,
                    RenderTextureFormat.Default
                );

                // Explicitly create the texture so it's ready for rendering
                this.workingRenderTexture.Create();

                // Warm up the render pipeline.
                // The first render into a new RenderTexture can produce a blank image,
                // so render once before the actual capture.
                this.WarmUpRenderTexture();
            }

            return this.workingRenderTexture;
        }

        /// <summary>
        /// Performs a warm-up render into the working RenderTexture.
        /// This ensures the first actual capture is not blank.
        /// </summary>
        private void WarmUpRenderTexture()
        {
            var previousTarget = this.renderCamera.targetTexture;
            this.renderCamera.targetTexture = this.workingRenderTexture;
            this.renderCamera.Render();
            this.renderCamera.targetTexture = previousTarget;
        }

        /// <summary>
        /// Reads pixels from the RenderTexture into a Texture2D.
        /// </summary>
        private Texture2D ReadPixelsFromRenderTexture(RenderTexture renderTexture)
        {
            var previousActive = RenderTexture.active;
            RenderTexture.active = renderTexture;

            var texture = new Texture2D(
                this.textureSize,
                this.textureSize,
                TextureFormat.RGBA32,
                false
            );

            texture.ReadPixels(new Rect(0, 0, this.textureSize, this.textureSize), 0, 0);
            texture.Apply();

            RenderTexture.active = previousActive;

            return texture;
        }
    }
}