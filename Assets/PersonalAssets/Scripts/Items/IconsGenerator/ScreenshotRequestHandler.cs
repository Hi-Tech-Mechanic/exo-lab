namespace ExoLab.Items
{
    using UnityEngine;

    /// <summary>
    /// Subscribes to screenshot request events and delegates them to the screenshot service.
    /// </summary>
    public class ScreenshotRequestHandler : MonoBehaviour, ISubsribable
    {
        /// <summary>
        /// Shared instance for accessing the screenshot service.
        /// </summary>
        public static ScreenshotRequestHandler Instance { get; private set; }

        [Header("Render settings")]
        [Tooltip("Camera used for rendering objects")]
        [SerializeField] private Camera renderCamera;

        [Tooltip("Texture size (width and height)")]
        [SerializeField] private int textureSize = 512;

        [Tooltip("Layer on which objects are rendered")]
        [SerializeField] private LayerMask renderLayer;

        private IScreenshotService screenshotService;
        private CameraScreenshotRenderer cameraScreenshotRenderer;

        /// <summary>
        /// Exposes the screenshot service to other components.
        /// </summary>
        public IScreenshotService Service => this.screenshotService;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            this.cameraScreenshotRenderer = new CameraScreenshotRenderer(
                this.renderCamera,
                this.textureSize,
                this.renderLayer
            );

            var storage = new FileScreenshotStorage();

            this.screenshotService = new ScreenshotService(this.cameraScreenshotRenderer, storage);
        }

        private void OnEnable()
        {
            this.SubscribeEvents();
        }

        private void OnDisable()
        {
            this.UnsubscribeEvents();
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }

            this.cameraScreenshotRenderer?.Cleanup();
        }

        public void SubscribeEvents()
        {
            GameEvents.ScreenshotEvents.OnScreenshotRequested += this.OnScreenshotRequestedHandler;
        }

        public void UnsubscribeEvents()
        {
            GameEvents.ScreenshotEvents.OnScreenshotRequested -= this.OnScreenshotRequestedHandler;
        }

        /// <summary>
        /// Handler for screenshot request events.
        /// Delegates the capture to the screenshot service.
        /// </summary>
        /// <param name="target">Object to capture</param>
        /// <param name="fileName">File name without extension</param>
        private void OnScreenshotRequestedHandler(GameObject target, string fileName)
        {
            this.screenshotService.CaptureToFile(target, fileName);
        }
    }
}