namespace ExoLab.Assembly
{
    using DG.Tweening;
    using ExoLab.Interaction;
    using ExoLab.UI;
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    using static ExoLab.Constants.Constants;

    /// <summary>
    /// Orchestrator for the item inspection system.
    /// Composes specialized components for input handling, rotation, zoom, camera panning,
    /// and zone detection. Maintains backward compatibility with the original public API.
    /// </summary>
    public class ItemInspect : MonoBehaviour
    {
        private const float ResetViewDuration = Timings.Millisecond_1000;

        [Header("References")]
        [SerializeField] private Camera inspectCamera;
        [Tooltip("GraphicRaycaster from the target canvas (required for UI-based zone detection)")]
        [SerializeField] private GraphicRaycaster canvasRaycaster;

        private IItemInspectInputProvider inputProvider;
        private IItemInspectZoneDetector zoneDetector;
        private ItemInspectObjectRotator objectRotator;
        private ItemInspectCameraController cameraController;

        private ItemInspectOptions itemInspectOptions;
        private Vector3 defaultPosition;
        private Quaternion defaultRotation;
        private bool isInspecting = true;

        /// <summary>
        /// Configuration options for the inspection mode.
        /// Set externally (e.g., by <see cref="AssemblyModesController"/>).
        /// </summary>
        public ItemInspectOptions ItemInspectOptions
        {
            private get => itemInspectOptions;
            set => itemInspectOptions = value;
        }

        /// <summary>
        /// The default camera distance used when resetting the view.
        /// </summary>
        public float DefaultCameraDistance
        {
            get => cameraController?.DefaultCameraDistance ?? 0f;
            set
            {
                if (cameraController != null)
                    cameraController.DefaultCameraDistance = value;
            }
        }

        /// <summary>
        /// Fired when the inspected object's rotation changes.
        /// </summary>
        public event Action<Quaternion> OnRotationChanged;

        /// <summary>
        /// Fired when the zoom level changes.
        /// </summary>
        public event Action<float> OnZoomChanged;

        /// <summary>
        /// Fired when the camera position changes due to panning.
        /// </summary>
        public event Action OnCameraPositionChanged;

        private void Start()
        {
            Initialize();
        }

        private void OnEnable()
        {
            DraggableInventoryItem.OnBeginDragAction += DisableInspectMode;
            DraggableInventoryItem.OnEndDragAction += EnableInspectMode;
            InteractiveIKController.OnItemInspectRotationBlock += SetRotationBlockState;
            AssemblyModesController.onItemInspectOptions += UpdateOptions;
        }

        private void OnDisable()
        {
            DraggableInventoryItem.OnBeginDragAction -= DisableInspectMode;
            DraggableInventoryItem.OnEndDragAction -= EnableInspectMode;
            InteractiveIKController.OnItemInspectRotationBlock -= SetRotationBlockState;
            AssemblyModesController.onItemInspectOptions -= UpdateOptions;
        }

        private void Update()
        {
            if (isInspecting == false)
                return;

            if (zoneDetector == null || zoneDetector.IsCursorInZone() == false)
                return;

            objectRotator?.ProcessRotation();
            cameraController?.ProcessZoom();
            cameraController?.ProcessCameraPan();
        }

        /// <summary>
        /// Resets the inspected object and camera to their default positions/rotations.
        /// </summary>
        public void ResetToDefaultView()
        {
            if (itemInspectOptions?.TargetTransform == null || cameraController == null)
                return;

            itemInspectOptions.TargetTransform.DOLocalMove(defaultPosition, ResetViewDuration);
            itemInspectOptions.TargetTransform.DOLocalRotate(defaultRotation.eulerAngles, ResetViewDuration);
            cameraController.ResetZoom();
        }

        /// <summary>
        /// Toggles the inspection mode on/off.
        /// </summary>
        public void ToggleInspectMode()
        {
            isInspecting = !isInspecting;
        }

        /// <summary>
        /// Updates the inspection options and reinitializes internal state.
        /// Called externally when the active preset changes.
        /// </summary>
        /// <param name="options">New inspection options.</param>
        public void UpdateOptions(ItemInspectOptions options)
        {
            if (options == null)
            {
                Debug.LogError($"{nameof(ItemInspect)}: Received null options.");
                return;
            }

            itemInspectOptions = options;

            if (options.TargetTransform != null)
            {
                defaultPosition = options.TargetTransform.localPosition;
                defaultRotation = options.TargetTransform.localRotation;
            }

            RebuildZoneDetector();
            RebuildObjectRotator();
            RebuildCameraController();
        }

        private void Initialize()
        {
            if (inspectCamera == null)
            {
                Debug.LogError($"{nameof(ItemInspect)}: Camera component not found!");
                enabled = false;
                return;
            }

            inputProvider = new ItemInspectInputProvider();

            if (itemInspectOptions != null)
            {
                if (itemInspectOptions.TargetTransform != null)
                {
                    defaultPosition = itemInspectOptions.TargetTransform.localPosition;
                    defaultRotation = itemInspectOptions.TargetTransform.localRotation;
                }

                RebuildZoneDetector();
                RebuildObjectRotator();
                RebuildCameraController();
            }
        }

        private void RebuildZoneDetector()
        {
            if (itemInspectOptions == null)
                return;

            if (itemInspectOptions.UseGraphicRaycaster)
            {
                if (canvasRaycaster == null)
                {
                    Debug.LogError($"{nameof(ItemInspect)}: GraphicRaycaster is required but not assigned.");
                    zoneDetector = null;
                    return;
                }

                var eventSystem = EventSystem.current;
                if (eventSystem == null)
                {
                    Debug.LogError($"{nameof(ItemInspect)}: EventSystem.current is null.");
                    zoneDetector = null;
                    return;
                }

                zoneDetector = new ItemInspectGraphicZoneDetector(canvasRaycaster, eventSystem);
            }
            else
            {
                zoneDetector = new ItemInspectPhysicsZoneDetector(inspectCamera);
            }
        }

        private void RebuildObjectRotator()
        {
            if (inputProvider == null || itemInspectOptions == null)
                return;

            if (objectRotator != null)
            {
                objectRotator.OnRotationChanged -= ForwardRotationChanged;
            }

            objectRotator = new ItemInspectObjectRotator(inputProvider, itemInspectOptions);
            objectRotator.OnRotationChanged += ForwardRotationChanged;
        }

        private void RebuildCameraController()
        {
            if (inputProvider == null || inspectCamera == null || itemInspectOptions == null)
                return;

            if (cameraController != null)
            {
                cameraController.OnZoomChanged -= ForwardZoomChanged;
                cameraController.OnCameraPositionChanged -= ForwardCameraPositionChanged;
            }

            cameraController = new ItemInspectCameraController(inputProvider, inspectCamera, itemInspectOptions);
            cameraController.OnZoomChanged += ForwardZoomChanged;
            cameraController.OnCameraPositionChanged += ForwardCameraPositionChanged;
        }

        private void ForwardRotationChanged(Quaternion rotation)
        {
            OnRotationChanged?.Invoke(rotation);
        }

        private void ForwardZoomChanged(float distance)
        {
            OnZoomChanged?.Invoke(distance);
        }

        private void ForwardCameraPositionChanged()
        {
            OnCameraPositionChanged?.Invoke();
        }

        private void DisableInspectMode()
        {
            isInspecting = false;
        }

        private void EnableInspectMode()
        {
            isInspecting = true;
        }

        private void SetRotationBlockState(bool value)
        {
            objectRotator?.SetRotationBlocked(value);
        }
    }
}