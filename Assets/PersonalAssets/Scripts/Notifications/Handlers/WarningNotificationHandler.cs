using ExoLab.Notifications.Views;
using UnityEngine;

namespace ExoLab.Notifications.Handlers
{
    /// <summary>
    /// Handler for Warning notifications.
    /// Warning notifications have a longer default duration and a pulsing animation.
    /// </summary>
    public sealed class WarningNotificationHandler : BaseNotificationHandler<WarningNotificationView>
    {
        [Header("Warning-Specific")]
        [SerializeField] private float defaultDuration = 6f;

        protected override void Start()
        {
            if (defaultDuration <= 0f)
                defaultDuration = 6f;

            base.Start();
        }

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