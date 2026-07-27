using ExoLab.Notifications.Views;
using DG.Tweening;
using ExoLab.Constants;
using UnityEngine;

namespace ExoLab.Notifications.Handlers
{
    /// <summary>
    /// Handler for Critical notifications.
    /// When a critical notification arrives, it interrupts any currently playing
    /// notification of this type, forcefully dismisses it, and displays the
    /// critical one immediately. Also triggers a camera shake for urgency.
    /// </summary>
    public sealed class CriticalNotificationHandler : BaseNotificationHandler<CriticalNotificationView>
    {
        [Header("Critical-Specific")]
        [SerializeField] private bool shakeCamera = true;
        [SerializeField] private float cameraShakeDuration = 0.5f;
        [SerializeField] private float cameraShakeStrength = 0.3f;

        protected override void Start()
        {
            if (cameraShakeDuration <= 0f)
                cameraShakeDuration = 0.5f;
            if (cameraShakeStrength <= 0f)
                cameraShakeStrength = 0.3f;

            base.Start();
        }

        /// <summary>
        /// Critical notifications interrupt the current display and show immediately.
        /// </summary>
        public override void Show(NotificationData data)
        {
            // Interrupt whatever is currently showing
            if (IsPlaying && ActiveViews.Count > 0)
            {
                var currentView = ActiveViews[0];
                if (currentView != null)
                {
                    currentView.ForceKill();
                }
            }

            // Clear any pending notifications in the queue (critical takes priority)
            DisplayQueue.Clear();
            IsPlaying = false;

            // Trigger camera shake if enabled
            if (shakeCamera)
                ShakeCamera();

            // Show immediately
            base.Show(data);
        }

        /// <summary>
        /// Shakes the main camera to add urgency.
        /// </summary>
        private void ShakeCamera()
        {
            var camTransform = Camera.main?.transform;
            if (camTransform == null) return;

            camTransform.DOComplete();
            camTransform.DOShakePosition(
                cameraShakeDuration,
                cameraShakeStrength,
                20,
                90f,
                false,
                true);
        }

        /// <summary>
        /// Interrupts the current notification (called by the facade in bulk-dismiss scenarios).
        /// </summary>
        public override void InterruptCurrent()
        {
            if (ActiveViews.Count > 0)
            {
                var currentView = ActiveViews[0];
                currentView?.ForceKill();
            }
            DisplayQueue.Clear();
            IsPlaying = false;
        }
    }
}