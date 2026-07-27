using ExoLab.Notifications.Handlers;
using UnityEngine;

namespace ExoLab.Notifications
{
    /// <summary>
    /// Facade for the entire notification system.
    /// Provides a single, simple API for the rest of the game to show notifications.
    ///
    /// Usage (via Inspector or code):
    ///   NotificationController.Instance.ShowInfo("Title", "Message");
    ///   NotificationController.Instance.ShowWarning("Title", "Message");
    ///   NotificationController.Instance.ShowCritical("Title", "Message");
    ///   NotificationController.Instance.DismissAll();
    /// </summary>
    public sealed class NotificationController : MonoBehaviour
    {
        [Header("Handler References (assign in Inspector)")]
        [SerializeField] private InfoNotificationHandler infoHandler;
        [SerializeField] private WarningNotificationHandler warningHandler;
        [SerializeField] private CriticalNotificationHandler criticalHandler;

        private static NotificationController instance;

        /// <summary>
        /// Singleton access. The first NotificationController in the scene will
        /// become the static instance. Destroy any duplicates.
        /// </summary>
        public static NotificationController Instance => instance;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Debug.LogWarning($"[{nameof(NotificationController)}] Duplicate instance detected. Destroying.");
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Clean up the static instance on destroy
        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        #region Public API

        /// <summary>Show a standard information notification.</summary>
        public void ShowInfo(string title, string message, float duration = 3f)
        {
            if (this.infoHandler == null)
            {
                Debug.LogError($"[{nameof(NotificationController)}] InfoHandler is not assigned!");
                return;
            }

            var data = new NotificationData(title, message, duration: duration);
            this.infoHandler.Show(data);
        }

        /// <summary>Show a warning notification (longer duration, pulsing).</summary>
        public void ShowWarning(string title, string message, float duration = 6f)
        {
            if (this.warningHandler == null)
            {
                Debug.LogError($"[{nameof(NotificationController)}] WarningHandler is not assigned!");
                return;
            }

            var data = new NotificationData(title, message, duration: duration);
            this.warningHandler.Show(data);
        }

        /// <summary>Show a critical notification (interrupts others, camera shake).</summary>
        public void ShowCritical(string title, string message, float duration = 4f)
        {
            if (this.criticalHandler == null)
            {
                Debug.LogError($"[{nameof(NotificationController)}] CriticalHandler is not assigned!");
                return;
            }

            var data = new NotificationData(title, message, duration: duration);
            this.criticalHandler.Show(data);
        }

        // ──────────────────────────────────────────────
        //  Advanced / Convenience Overloads
        // ──────────────────────────────────────────────

        /// <summary>Show a notification of a specific type with full data control.</summary>
        public void Show(NotificationType type, NotificationData data)
        {
            switch (type)
            {
                case NotificationType.Info:
                    this.infoHandler?.Show(data);
                    break;
                case NotificationType.Warning:
                    this.warningHandler?.Show(data);
                    break;
                case NotificationType.Critical:
                    this.criticalHandler?.Show(data);
                    break;
            }
        }

        /// <summary>Dismiss all notifications of all types.</summary>
        public void DismissAll()
        {
            this.infoHandler?.DismissAll();
            this.warningHandler?.DismissAll();
            this.criticalHandler?.DismissAll();
        }

        /// <summary>Dismiss all notifications of a specific type.</summary>
        public void DismissAll(NotificationType type)
        {
            switch (type)
            {
                case NotificationType.Info:
                    this.infoHandler?.DismissAll();
                    break;
                case NotificationType.Warning:
                    this.warningHandler?.DismissAll();
                    break;
                case NotificationType.Critical:
                    this.criticalHandler?.DismissAll();
                    break;
            }
        }

        #endregion
    }
}