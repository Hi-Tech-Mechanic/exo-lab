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

        private static NotificationController _instance;

        /// <summary>
        /// Singleton access. The first NotificationController in the scene will
        /// become the static instance. Destroy any duplicates.
        /// </summary>
        public static NotificationController Instance => _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.LogWarning("[NotificationController] Duplicate instance detected. Destroying.");
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // ──────────────────────────────────────────────
        //  Public API
        // ──────────────────────────────────────────────

        /// <summary>Show a standard information notification.</summary>
        public void ShowInfo(string title, string message, float duration = 3f)
        {
            if (infoHandler == null)
            {
                Debug.LogError("[NotificationController] InfoHandler is not assigned!");
                return;
            }

            var data = new NotificationData(title, message, duration: duration);
            infoHandler.Show(data);
        }

        /// <summary>Show a warning notification (longer duration, pulsing).</summary>
        public void ShowWarning(string title, string message, float duration = 6f)
        {
            if (warningHandler == null)
            {
                Debug.LogError("[NotificationController] WarningHandler is not assigned!");
                return;
            }

            var data = new NotificationData(title, message, duration: duration);
            warningHandler.Show(data);
        }

        /// <summary>Show a critical notification (interrupts others, camera shake).</summary>
        public void ShowCritical(string title, string message, float duration = 4f)
        {
            if (criticalHandler == null)
            {
                Debug.LogError("[NotificationController] CriticalHandler is not assigned!");
                return;
            }

            var data = new NotificationData(title, message, duration: duration);
            criticalHandler.Show(data);
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
                    infoHandler?.Show(data);
                    break;
                case NotificationType.Warning:
                    warningHandler?.Show(data);
                    break;
                case NotificationType.Critical:
                    criticalHandler?.Show(data);
                    break;
            }
        }

        /// <summary>Dismiss all notifications of all types.</summary>
        public void DismissAll()
        {
            infoHandler?.DismissAll();
            warningHandler?.DismissAll();
            criticalHandler?.DismissAll();
        }

        /// <summary>Dismiss all notifications of a specific type.</summary>
        public void DismissAll(NotificationType type)
        {
            switch (type)
            {
                case NotificationType.Info:
                    infoHandler?.DismissAll();
                    break;
                case NotificationType.Warning:
                    warningHandler?.DismissAll();
                    break;
                case NotificationType.Critical:
                    criticalHandler?.DismissAll();
                    break;
            }
        }

        // Clean up the static instance on destroy
        private void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }
    }
}