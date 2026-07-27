using UnityEngine;

namespace ExoLab.Notifications
{
    /// <summary>
    /// Holds all data required to display a notification.
    /// Passed from the Controller through the Handler to the View.
    /// </summary>
    public readonly struct NotificationData
    {
        /// <summary>Headline text of the notification.</summary>
        public string Title { get; }

        /// <summary>Body/description text.</summary>
        public string Message { get; }

        /// <summary>Optional icon sprite. Null means use the handler's default.</summary>
        public Sprite Icon { get; }

        /// <summary>How long the notification stays visible (seconds).</summary>
        public float Duration { get; }

        /// <summary>Optional custom color tint. Null means use the handler's default.</summary>
        public Color? Tint { get; }

        public NotificationData(
            string title,
            string message,
            Sprite icon = null,
            float duration = 3f,
            Color? tint = null)
        {
            Title = title;
            Message = message;
            Icon = icon;
            Duration = Mathf.Max(0.5f, duration);
            Tint = tint;
        }
    }
}