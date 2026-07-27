namespace ExoLab.Notifications.Handlers
{
    using ExoLab.Notifications.Views;
    using UnityEngine;

    /// <summary>
    /// Handler for Warning notifications.
    /// Warning notifications have a longer default duration and a pulsing animation.
    /// </summary>
    public sealed class WarningNotificationHandler : BaseNotificationHandler<WarningNotificationView>
    {
        [Header("Warning-Specific")]
        [SerializeField, Range(0F, 10F)] private float defaultDuration = 6F;

        public override void Show(NotificationData data)
        {
            // Ensure warning duration is never shorter than the default
            var adjustedData = new NotificationData(
                data.Title,
                data.Message,
                data.Icon,
                Mathf.Max(data.Duration, defaultDuration),
                data.Tint
            );

            base.Show(adjustedData);
        }
    }
}