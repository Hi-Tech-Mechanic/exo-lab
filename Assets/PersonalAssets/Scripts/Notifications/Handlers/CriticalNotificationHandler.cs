namespace ExoLab.Notifications.Handlers
{
    using ExoLab.Notifications.Views;
    using DG.Tweening;
    using UnityEngine;

    /// <summary>
    /// Handler for Critical notifications.
    /// When a critical notification arrives, it interrupts any currently playing
    /// notification of this type, forcefully dismisses it, and displays the
    /// critical one immediately. Also triggers a camera shake for urgency.
    /// </summary>
    public sealed class CriticalNotificationHandler : BaseNotificationHandler<CriticalNotificationView>
    {
        private const int vibrato = 20;
        private const float randomness = 90F;

        [Header("Critical-Specific")]
        [SerializeField] private bool shakeCamera = true;
        [SerializeField, Range(0F, 5F)] private float cameraShakeDuration = 0.5f;
        [SerializeField, Range(-3F, 3F)] private float cameraShakeStrength = 0.3f;

        /// <summary>
        /// Critical notifications interrupt the current display and show immediately.
        /// </summary>
        public override void Show(NotificationData data)
        {
            // Interrupt whatever is currently showing
            if (this.IsPlaying && this.ActiveViews.Count > 0)
            {
                var currentView = this.ActiveViews[0];
                if (currentView != null)
                {
                    currentView.ForceKill();
                }
            }

            // Clear any pending notifications in the queue (critical takes priority)
            this.DisplayQueue.Clear();
            this.IsPlaying = false;

            // Trigger camera shake if enabled
            if (this.shakeCamera)
            {
                this.ShakeCamera();
            }

            // Show immediately
            base.Show(data);
        }

        /// <summary>
        /// Shakes the main camera to add urgency.
        /// </summary>
        private void ShakeCamera()
        {
            var camTransform = Camera.main?.transform;
            if (camTransform == null)
            {
                return;
            }

            camTransform.DOComplete();
            camTransform.DOShakePosition(
                this.cameraShakeDuration,
                this.cameraShakeStrength,
                vibrato,
                randomness,
                false,
                true);
        }

        /// <summary>
        /// Interrupts the current notification (called by the facade in bulk-dismiss scenarios).
        /// </summary>
        public override void InterruptCurrent()
        {
            if (this.ActiveViews.Count > 0)
            {
                var currentView = this.ActiveViews[0];
                currentView?.ForceKill();
            }

            this.DisplayQueue.Clear();
            this.IsPlaying = false;
        }
    }
}