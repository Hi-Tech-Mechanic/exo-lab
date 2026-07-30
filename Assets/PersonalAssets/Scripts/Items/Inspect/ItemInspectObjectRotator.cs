namespace ExoLab.Assembly
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Handles rotation of the inspected object based on mouse input.
    /// Supports per-axis rotation toggling and rotation blocking.
    /// </summary>
    public sealed class ItemInspectObjectRotator
    {
        private readonly IItemInspectInputProvider inputProvider;
        private readonly ItemInspectOptions options;
        private bool rotationBlocked;

        /// <summary>
        /// Fired when the inspected object's rotation changes.
        /// Provides the new rotation value.
        /// </summary>
        public event Action<Quaternion> OnRotationChanged;

        public ItemInspectObjectRotator(IItemInspectInputProvider inputProvider, ItemInspectOptions options)
        {
            this.inputProvider = inputProvider;
            this.options = options;
        }

        /// <summary>
        /// Blocks or unblocks rotation via left mouse button.
        /// Right mouse button always works regardless of this flag.
        /// </summary>
        public void SetRotationBlocked(bool blocked)
        {
            rotationBlocked = blocked;
        }

        /// <summary>
        /// Processes mouse input and rotates the target object accordingly.
        /// Should be called once per frame when inspection is active.
        /// </summary>
        public void ProcessRotation()
        {
            bool leftMousePressed = inputProvider.IsLeftMouseButtonPressed;
            bool rightMousePressed = inputProvider.IsRightMouseButtonPressed;

            // Allow rotation with right mouse button even if rotation is blocked
            if (rotationBlocked && rightMousePressed == false)
                return;

            if (leftMousePressed == false && rightMousePressed == false)
                return;

            Transform target = options.TargetTransform;
            if (target == null)
                return;

            if (options.RotateByCoordinate_X)
            {
                float mouseX = inputProvider.MouseX * options.RotationSpeed;
                target.Rotate(Vector3.up, -mouseX, Space.World);
            }

            if (options.RotateByCoordinate_Y)
            {
                float mouseY = inputProvider.MouseY * options.RotationSpeed;
                target.Rotate(Vector3.right, mouseY, Space.World);
            }

            OnRotationChanged?.Invoke(target.rotation);
        }
    }
}