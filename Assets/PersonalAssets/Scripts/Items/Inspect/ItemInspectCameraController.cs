namespace ExoLab.Assembly
{
    using DG.Tweening;
    using System;
    using UnityEngine;
    using static ExoLab.Constants.Constants;

    /// <summary>
    /// Handles zoom (scroll wheel) and camera panning (middle mouse button)
    /// for the inspection camera.
    /// </summary>
    public sealed class ItemInspectCameraController
    {
        private readonly IItemInspectInputProvider inputProvider;
        private readonly Camera inspectCamera;
        private readonly ItemInspectOptions options;

        private float currentCameraDistance;

        /// <summary>
        /// Fired when zoom level changes. Provides the current camera distance.
        /// </summary>
        public event Action<float> OnZoomChanged;

        /// <summary>
        /// Fired when the camera position changes due to panning.
        /// </summary>
        public event Action OnCameraPositionChanged;

        /// <summary>
        /// The default distance used for resetting the camera view.
        /// </summary>
        public float DefaultCameraDistance { get; set; }

        public ItemInspectCameraController(
            IItemInspectInputProvider inputProvider,
            Camera inspectCamera,
            ItemInspectOptions options)
        {
            this.inputProvider = inputProvider;
            this.inspectCamera = inspectCamera;
            this.options = options;

            currentCameraDistance = inspectCamera.transform.localPosition.z;
            DefaultCameraDistance = currentCameraDistance;
        }

        /// <summary>
        /// Processes scroll wheel input to zoom the camera in/out.
        /// Should be called once per frame.
        /// </summary>
        public void ProcessZoom()
        {
            if (options.ZoomEnabled == false)
                return;

            float scroll = inputProvider.MouseScrollWheel;
            if (scroll == 0f)
                return;

            currentCameraDistance += scroll * options.ZoomSpeed;
            currentCameraDistance = Mathf.Clamp(
                currentCameraDistance,
                -options.MaxCameraDistance,
                -options.MinCameraDistance);

            Vector3 localPosition = inspectCamera.transform.localPosition;
            inspectCamera.transform.localPosition = new Vector3(
                localPosition.x,
                localPosition.y,
                currentCameraDistance);

            OnZoomChanged?.Invoke(currentCameraDistance);
        }

        /// <summary>
        /// Processes middle mouse button input to pan the camera.
        /// Should be called once per frame.
        /// </summary>
        public void ProcessCameraPan()
        {
            if (inputProvider.IsMiddleMouseButtonPressed == false)
                return;

            float mouseX = inputProvider.MouseX;
            float mouseY = inputProvider.MouseY;

            Vector3 position = inspectCamera.transform.position;
            Vector3 targetPosition = new Vector3(-mouseX, -mouseY, position.z);

            const float panLerpFactor = 0.02f;
            inspectCamera.transform.localPosition = Vector3.Lerp(
                inspectCamera.transform.localPosition,
                targetPosition,
                panLerpFactor);

            OnCameraPositionChanged?.Invoke();
        }

        /// <summary>
        /// Resets the camera zoom to the default distance.
        /// </summary>
        public void ResetZoom()
        {
            inspectCamera.transform.DOLocalMoveZ(DefaultCameraDistance, Timings.Millisecond_1000);
        }
    }
}